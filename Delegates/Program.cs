using System;

namespace Delegates
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var guitars = new List<Guitar>();

            guitars.Add(new Guitar(PickupType.Electric, StringType.Steel, "Cherry Red Strat"));
            guitars.Add(new Guitar(PickupType.AcousticElectric, StringType.Nylon, "Takamine EG-116")); ;
            guitars.Add(new Guitar(PickupType.Acoustic, StringType.Steel, "Martin D-X1E"));

            Func<Guitar, bool> nylon = guitar => guitar.Strings == StringType.Nylon;

            var nylonGuitars = guitars.Where(nylon);

            // var electricGuitars = guitars.Where(guitar => guitar.Pickup == PickupType.Electric);

            IEnumerable<Guitar> electricGuitars = Enumerable.Where<Guitar>(guitars, guitar => guitar.Pickup == PickupType.Electric);

            foreach (var guitar in electricGuitars.ToList())
            {
                Console.WriteLine(guitar.Name);
            }
        }
    }
}