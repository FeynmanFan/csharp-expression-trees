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
                currentExpression = CreateExpression<bool>(survived.Value, null, "Survived", passengerParameter);
            }

            if (pClass != null)
            {
                currentExpression = CreateExpression<int>(pClass.Value, currentExpression, "PClass", passengerParameter);
            }

            if (sex != null)
            {
                currentExpression = CreateExpression<SexValue>(sex.Value, currentExpression, "Sex", passengerParameter);
            }

            if (age != null)
            {
                currentExpression = CreateExpression<decimal>(age.Value, currentExpression, "Age", passengerParameter);
            }

            if (minimumFare != null)
            {
                currentExpression = CreateExpression<decimal>(age.Value, currentExpression, "Age", passengerParameter, ">");
            }

            if (currentExpression != null)
            {
                var expr = Expression.Lambda<Func<Passenger, bool>>(currentExpression, false, new List<ParameterExpression> { passengerParameter });
                var func = expr.Compile();

                this.Passengers = this.Passengers.Where(func);
            }

            return this.Passengers;
        }

        private static Expression CreateExpression<T>(object value, Expression currentExpression, string propertyName, ParameterExpression objectParameter, string operatorType = "=")
        {
            var valueToTest = Expression.Constant((T)value);

            var propertyToCall = Expression.Property(objectParameter, propertyName);

            Expression operatorExpression = null;

            switch(operatorType)
            {
                case "=":
                    operatorExpression = Expression.Equal(propertyToCall, valueToTest);
                    break;
                case ">":
                    operatorExpression = Expression.GreaterThan(propertyToCall, valueToTest);
                    break;
            }

            if (currentExpression == null)
            {
                currentExpression = operatorExpression;
            }
            else
            {
                var previousExpression = currentExpression;

                currentExpression = Expression.And(previousExpression, operatorExpression);
            }

            return currentExpression;
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