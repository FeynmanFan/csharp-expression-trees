namespace TitanicExplorer.Pages
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using TitanicExplorer.Data;
    using System.IO;
    using static TitanicExplorer.Data.Passenger;

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
            var age = ParseNullInt(Request.Form["age"]);
            var minimumFare = ParseNullDecimal(Request.Form["minimumFare"]);

            this.Passengers = FilterPassengers(survived, pClass, sex, age, minimumFare);
        }

        private IEnumerable<Passenger> FilterPassengers(bool? survived, int? pClass, SexValue? sex, int? age, decimal? minimumFare)
        {
            if (survived != null)
            {
                this.Passengers = this.Passengers.Where(x => x.Survived == survived.Value);
            }

            if (pClass != null)
            {
                this.Passengers = this.Passengers.Where(x => x.PClass == pClass.Value);
            }

            if (sex != null)
            {
                this.Passengers = this.Passengers.Where(x => x.Sex == sex.Value);
            }

            if (age != null)
            {
                this.Passengers = this.Passengers.Where(x => x.Age == age.Value);
            }

            if (minimumFare != null)
            {
                this.Passengers = this.Passengers.Where(x => x.Fare >= minimumFare.Value);
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