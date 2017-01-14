using System;

namespace Automobiliu_parkas
{
    class LengvasisAutomobilis : TransportoPriemone
    {
        public LengvasisAutomobilis(string carNumber, string carName, string carModel, DateTime year, DateTime expirationDate, string fuel, string averageConsumption)
           : base(carNumber, carName, carModel, year, expirationDate, fuel, averageConsumption)
        {
        }

        public LengvasisAutomobilis(string data)
            : base(data)
        {
            SetData(data);
        }


        public override String ToString()
        {
            return String.Format("Valstybinis numeris: {0, -3} | Gamintojas: {1, -7} | Modelis: {2, -7} | Pagaminimo metai ir menuo: {3, 4} | Technines galiojimo data: {4, 4} | Kuras: {5 , -9} | 100km/h: {6 , 4} |"
                , Number, Name, Model, Year.ToShortDateString(), ExpirationDate.ToShortDateString(), Fuel, AverageConsumption);
        }

    }
}
