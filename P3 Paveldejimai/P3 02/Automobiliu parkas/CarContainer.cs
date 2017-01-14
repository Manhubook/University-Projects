using System;
using System.Linq;

namespace Automobiliu_parkas
{
    class CarContainer
    {
        private TransportoPriemone[] cars { get; set; }
        public int Count { get; private set; }

        public CarContainer(int size) { cars = new TransportoPriemone[size]; }

        public void AddCar(TransportoPriemone car) { cars[Count++] = car; }

        public TransportoPriemone GetCar(int index) { return cars[index]; }

        public bool Contains(TransportoPriemone _car) { return cars.Contains(_car); }

        public void Sort()
        {
            for (int i = 0; i < Count - 1; i++)
            {
                TransportoPriemone minValueTranPriemone = cars[i];
                int minValueIndex = i;
                for (int j = i + 1; j < Count; j++)
                {
                    if (cars[j] <= minValueTranPriemone)
                    {
                        minValueTranPriemone = cars[j];
                        minValueIndex = j;
                    }
                }
                cars[minValueIndex] = cars[i];
                cars[i] = minValueTranPriemone;
            }
        }

        /// <summary>
        /// Skaiciuoja filalo vid. transporto priemones pagaminimo metus
        /// </summary>
        /// <param name="cars"></param>
        /// <returns></returns>
        public double CarYearAverage()
        {
            double sum = 0;
            double averageCarYear = 0;

            for (int j = 0; j < Count; j++)
            {
                sum += GetCar(j).Year.Year;
            }

            averageCarYear = sum / Count;
            averageCarYear = Math.Round(averageCarYear, MidpointRounding.AwayFromZero);

            return averageCarYear;
        }

        /// <summary>
        /// Iesko transportu priemoniu, kuriu tech. gal. laikas yra pasibaiges
        /// </summary>
        /// <returns></returns>
        public CarContainer ExpiredCars()
        {
            var expiredCars = new CarContainer(Program.CMaxBranches * Program.CMaxCars);
            DateTime expireTime = DateTime.Now.AddMonths(3);
            DateTime now = DateTime.Now;
            for (int i = 0; i < Count; i++)
                if (GetCar(i).ExpirationDate < expireTime && GetCar(i).ExpirationDate > now)
                    expiredCars.AddCar(GetCar(i));

            return expiredCars;
        }
    }
}
