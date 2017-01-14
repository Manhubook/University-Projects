using System;

namespace Automobiliu_parkas
{
    abstract class TransportoPriemone
    {
        public string Number { get; set; }              //Numeris
        public string Name { get; set; }                //Pavadinimas
        public string Model { get; set; }               //Modelis
        public DateTime Year { get; set; }              //Pagaminimo metai
        public DateTime ExpirationDate { get; set; }    //Technikines galiojimo laikas
        public string Fuel { get; set; }                //Kuro tipas
        public string AverageConsumption { get; set; }  //Kuros sanaudos 

        public TransportoPriemone(string data)
        {
            SetData(data);
        }

        public TransportoPriemone(string number, string name, string model, DateTime year, DateTime expirationDate, string fuel, string averageConsumption)
        {
            Number = number;
            Name = name;
            Model = model;
            Year = year;
            ExpirationDate = expirationDate;
            Fuel = fuel;
            AverageConsumption = averageConsumption;
        }

        public virtual void SetData(string line)
        {
            string[] values = line.Split(';');
            Number = values[1];
            Name = values[2];
            Model = values[3];
            Year = DateTime.Parse(values[4]);
            ExpirationDate = DateTime.Parse(values[5]);
            Fuel = values[6];
            AverageConsumption = values[7];
        }

        public override String ToString()
        {
            return String.Format("Valstybinis numeris: {0,3} | Gamintojas: {1,8} | Modelis: {2,10} | Pagaminimo metai ir menuo: {3,5} | Technines galiojimo data: {4,5} | Kuras: {5 ,9} | 100km/h: {6 , 3}|"
                , Number, Name, Model, Year, ExpirationDate, Fuel, AverageConsumption);
        }

        public static bool operator <=(TransportoPriemone lhs, TransportoPriemone rhs)
        {
            int x = String.Compare(lhs.Name, rhs.Name, StringComparison.CurrentCulture);

            int y = String.Compare(lhs.Model, rhs.Model, StringComparison.CurrentCulture);

            return (x < 0 || (x == 0)) && (y < 0 || (y == 0));
        }

        public static bool operator >=(TransportoPriemone lhs, TransportoPriemone rhs)
        {
            int x = String.Compare(lhs.Name, rhs.Name, StringComparison.CurrentCulture);

            int y = String.Compare(lhs.Model, rhs.Model, StringComparison.CurrentCulture);

            return (x > 0 || (x == 0)) && (y > 0 || (y == 0));
        }

    }
}
