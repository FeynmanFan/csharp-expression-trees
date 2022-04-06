namespace TitanicExplorer.Pages
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using TitanicExplorer.Data;
    using System.IO;
    using static TitanicExplorer.Data.Passenger;
    using System.Linq.Expressions;

    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;

            var sampleDataPath = Path.GetTempFileName();

            System.IO.File.WriteAllText(sampleDataPath, DataFiles.passengers);

            this.Passengers = Passenger.LoadFromFile(sampleDataPath);
        }

        public IEnumerable<Passenger> Passengers
        {
            get; private set;
        }

        public void OnGet()
        {

        }

        public void OnPost()
        {
            var survived = Request.Form["survived"]!= "" ? ParseSurvived(Request.Form["survived"]) : null;
            var pClass = ParseNullInt(Request.Form["pClass"]);
            var sex = Request.Form["sex"] != "" ? ParseSex(Request.Form["sex"]) : null;
            var age = ParseNullDecimal(Request.Form["age"]);
            var minimumFare = ParseNullDecimal(Request.Form["minimumFare"]);

            this.Passengers = FilterPassengers(survived, pClass, sex, age, minimumFare);
        }

        private IEnumerable<Passenger> FilterPassengers(bool? survived, int? pClass, SexValue? sex, decimal? age, decimal? minimumFare)
        {
            Expression currentExpression = null;

            var passengerParameter = Expression.Parameter(typeof(Passenger));

            if (survived != null)
            {
                var survivedValue = Expression.Constant(survived.Value);

                var passengerSurvived = Expression.Property(passengerParameter, "Survived");

                currentExpression = Expression.Equal(passengerSurvived, survivedValue);
            }

            if (pClass != null)
            {
                var pClassValue = Expression.Constant(pClass.Value);

                var passengerPClass = Expression.Property(passengerParameter, "PClass");

                var pClassEquals = Expression.Equal(passengerPClass, pClassValue);

                if (currentExpression == null)
                {
                    currentExpression = pClassEquals;
                }
                else
                {
                    var previousExpression = currentExpression;

                    currentExpression = Expression.And(previousExpression, pClassEquals);
                }
            }

            if (sex != null)
            {
                var sexValue = Expression.Constant(sex.Value);

                var passengerSex = Expression.Property(passengerParameter, "Sex");

                var sexEquals = Expression.Equal(passengerSex, sexValue);

                if (currentExpression == null)
                {
                    currentExpression = sexEquals;
                }
                else
                {
                    var previousExpression = currentExpression;

                    currentExpression = Expression.And(previousExpression, sexEquals);
                }
            }

            if (age != null)
            {
                var ageValue = Expression.Constant(age.Value);

                var passengerAge = Expression.Property(passengerParameter, "Age");

                var ageEquals = Expression.Equal(passengerAge, ageValue);

                if (currentExpression == null)
                {
                    currentExpression = ageEquals;
                }
                else
                {
                    var previousExpression = currentExpression;

                    currentExpression = Expression.And(previousExpression, ageEquals);
                }
            }

            if (minimumFare != null)
            {
                var minumFareValue = Expression.Constant(minimumFare.Value);

                var passengerFare= Expression.Property(passengerParameter, "Fare");

                var fareGreaterThan = Expression.GreaterThan(passengerFare, minumFareValue);

                if (currentExpression == null)
                {
                    currentExpression = fareGreaterThan;
                }
                else
                {
                    var previousExpression = currentExpression;

                    currentExpression = Expression.And(previousExpression, fareGreaterThan);
                }

            }

            if (currentExpression != null)
            {
                var expr = Expression.Lambda<Func<Passenger, bool>>(currentExpression, false, new List<ParameterExpression> { passengerParameter });
                var func = expr.Compile();

                this.Passengers = this.Passengers.Where(func);
            }

            return this.Passengers;
        }



        public decimal? ParseNullDecimal(string value)
        {
            if (decimal.TryParse(value, out decimal result))
            {
                return result;
            }

            return null;
        }

        public int? ParseNullInt(string value)
        {
            if (int.TryParse(value, out int result))
            {
                return result;
            }

            return null;
        }

        public SexValue? ParseSex(string value)
        {
             return value == "male" ? SexValue.Male : SexValue.Female;
        }

        public bool? ParseSurvived(string value)
        {
            return value == "Survived" ? true : false;
        }
    }
}