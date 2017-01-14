using System;

namespace Automobiliu_parkas
{
    class KrovininisAutomobilis : TransportoPriemone
    {
        public KrovininisAutomobilis(string carNumber, string carName, string carModel, DateTime year, DateTime expirationDate, string fuel, string averageConsumption, int priekabosTalpa)
            : base(carNumber, carName, carModel, year, expirationDate, fuel, averageConsumption)
        {
            PriekabosTalpa = priekabosTalpa;
        }

        public KrovininisAutomobilis(string data)
            : base(data)
        {
            SetData(data);
        }

        public override void SetData(string line)
        {
            base.SetData(line);
            string[] values = line.Split(';');
            PriekabosTalpa = int.Parse(values[8]);
        }

        public int PriekabosTalpa { get; set; }

        public override String ToString()
        {
            return String.Format("Valstybinis numeris: {0, -3} | Gamintojas: {1, -7} | Modelis: {2, -7} | Pagaminimo metai ir menuo: {3, -4} | Technines galiojimo data: {4, -4} | Kuras: {5 , -9} | 100km/h: {6 , 4} | Talpa: {7, 2} |"
                , Number, Name, Model, Year.ToShortDateString(), ExpirationDate.ToShortDateString(), Fuel, AverageConsumption, PriekabosTalpa);
        }

    }
}
