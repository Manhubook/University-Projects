using System;

namespace Automobiliu_parkas
{
    class TransportoPriemone
    {
        public string CarNumber { get; set; }  //Numeris
        public string CarName { get; set; }  //Pavadinimas
        public string CarModel { get; set; }  //Modelis
        public DateTime Year { get; set; }  //Pagaminimo metai
        public DateTime ExpirationDate { get; set; }  //Technikines galiojimo laikas
        public string Fuel { get; set; }  //Kuro tipas
        public string AverageConsumption { get; set; }  //Kuros sanaudos 

        public TransportoPriemone() { }

        public TransportoPriemone(string carNumber, string carName, string carModel, DateTime year,
            DateTime expirationDate, string fuel, string averageConsumption)
        {
            CarNumber = carNumber;
            CarName = carName;
            CarModel = carModel;
            Year = year;
            ExpirationDate = expirationDate;
            Fuel = fuel;
            AverageConsumption = averageConsumption;
        }

        public override String ToString()
        {
            return String.Format("Valstybinis numeris: {0,3} | Gamintojas: {1,8} | Modelis: {2,10} | " +
                " Pagaminimo metai ir menuo: {3,5} | Technines galiojimo data: {4,5} | Kuras: {5 ,9} | 100km/h: {6 , 3}|",
                CarNumber, CarName, CarModel, Year, ExpirationDate, Fuel, AverageConsumption);
        }

        public static bool operator <=(TransportoPriemone lhs, TransportoPriemone rhs)
        {
            int x = String.Compare(lhs.CarName, rhs.CarName, StringComparison.CurrentCulture);
            int y = String.Compare(lhs.CarModel, rhs.CarModel, StringComparison.CurrentCulture);

            return x <= 0 && y <= 0;
        }

        public static bool operator >=(TransportoPriemone lhs, TransportoPriemone rhs)
        {
            int x = String.Compare(lhs.CarName, rhs.CarName, StringComparison.CurrentCulture);
            int y = String.Compare(lhs.CarModel, rhs.CarModel, StringComparison.CurrentCulture);

            return x >= 0 && y >= 0;
        }
    }
}
