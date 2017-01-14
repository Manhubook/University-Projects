using System;

namespace Automobiliu_parkas
{
    class Autobusas : TransportoPriemone
    {
        public Autobusas(string carNumber, string carName, string carModel, DateTime year, DateTime expirationDate, string fuel, string averageConsumption, int vietuSkc)
            : base(carNumber, carName, carModel, year, expirationDate, fuel, averageConsumption)
        {
            VietuSkc = vietuSkc;
        }

        public int VietuSkc { get; set; }

        public override String ToString()
        {
            return String.Format("Valstybinis numeris: {0, -3} | Gamintojas: {1, -7} | Modelis: {2, -7} | Pagaminimo metai ir menuo: {3, -4} | Technines galiojimo data: {4, -4} | Kuras: {5 , -9} | 100km/h: {6 , 4} | Sedimu vietu skc: {7, 2} |", CarNumber, CarName, CarModel, Year.ToShortDateString(), ExpirationDate.ToShortDateString(), Fuel, AverageConsumption, VietuSkc);
        }
    }
}
