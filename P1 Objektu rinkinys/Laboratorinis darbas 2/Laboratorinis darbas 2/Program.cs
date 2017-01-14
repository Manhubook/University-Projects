using System;
using System.IO;

namespace Laboratorinis_darbas_2
{
    class Program
    {
        class Car
        {
            public string number { get; set; }
            public string name { get; set; }
            public string model { get; set; }
            public DateTime year { get; set; }
            public DateTime expireYear { get; set; }
            public string fuelType { get; set; }
            public double fuelConsumption { get; set; }

            public Car() { }

            public Car(string number, string name, string model,
                DateTime year, DateTime expireYear, string fuelType, double fuelConsumption)
            {
                this.number = number;
                this.name = name;
                this.model = model;
                this.year = year;
                this.expireYear = expireYear;
                this.fuelType = fuelType;
                this.fuelConsumption = fuelConsumption;
            }

            public override string ToString()
            {
                return string.Format("{0, -6} | {1, -8} | {2, -10} | {3} | {4:d} | {5, -9} | {6}", number, name, model,
                    year.ToShortDateString(), expireYear.ToShortDateString(), fuelType, fuelConsumption);
            }
        }


        public const int MaxNumberOfCars = 50;
        public const string CDf = @"..\..\Duomenys.txt";
        public const string CDr = @"..\..\Senienos.txt";
        public const string CDn = @"..\..\Apziura.txt";

        static void Main(string[] args)
        {
            Car[] _car;
            int carCount;

            ReadData(out _car, out carCount, CDf);

            //------------------------------------------------------
            //Labiausiai pasikartojintis automobilio pavadinimas ir ju kiekis

            string popular;
            int count;

            PopularCar(_car, carCount, out popular, out count);
            Console.WriteLine("Populiariausia marke {0} ir ju kiekis {1} \r\n", popular, count);

            //------------------------------------------------------
            //Volvo markės automobilių sąrašas 

            int vwCount;
            Car[] volvo = FindAllVolvo(_car, carCount, out vwCount);
            Console.WriteLine("Visi Volvo automobiliai: \r\n");
            for (int i = 0; i < vwCount; i++)
                Console.WriteLine("{0} {1} {2}", volvo[i].name, volvo[i].model, volvo[i].year.ToShortDateString());

            //------------------------------------------------------
            //Senienos

            int oldCount;
            Car[] oldCar = FindOldCar(_car, carCount, out oldCount);

            //------------------------------------------------------
            //Technikinės galiojamas

            int expiredCount;
            Car[] expiredCars = ExpiredCars(_car, carCount, out expiredCount);

            //------------------------------------------------------

            SaveOldCars(oldCar, oldCount, CDr);
            SaveExpiredCars(expiredCars, expiredCount, CDn);

            Console.ReadKey();
        }

        /// <summary>
        /// Duomenu skaitymas is failo
        /// </summary>
        /// <param name="cars"></param>
        /// <param name="carCount"></param>
        /// <param name="fv"></param>
        static void ReadData(out Car[] cars, out int carCount, string fv)
        {
            carCount = 0;
            cars = new Car[MaxNumberOfCars];
            string line = "";

            using (var reader = File.OpenText(fv))
            {
                while (null != (line = reader.ReadLine()))
                {
                    string[] values = line.Split(';');
                    string carNumber = values[0];  //Valstybinis numeris
                    string carName = values[1];    //Gamintojas
                    string carModel = values[2];   //Modelis
                    DateTime year = DateTime.Parse(values[3]);            // Metai
                    DateTime expirationDate = DateTime.Parse(values[4]);  // Technikėnes galiojimo laiaks
                    string fuel = values[5];                              // Kuro tipas
                    double averageconsumption = double.Parse(values[6]);  // 100km/h

                    Car car = new Car(carNumber, carName, carModel, year, expirationDate, fuel, averageconsumption);
                    cars[carCount++] = car;
                }
            }
        }

        /// <summary>
        /// Ieškomas daugiausiai pasikartojamas gamintojo pavadinimas, bei automobilių kiekis.
        /// </summary>
        /// <param name="cars"></param>
        /// <param name="carcount"></param>
        /// <param name="popular"></param>
        /// <param name="count"></param>
        static void PopularCar(Car[] cars, int carcount, out string popular, out int count)
        {
            popular = "not found";
            count = 1;
            int tempCount = 1;
            string tempName = "";

            for (int i = 0; i < carcount; i++)
            {
                tempName = cars[i].name;
                for (int j = 0; j < carcount; j++)
                {
                    if (tempName == cars[j].name && j != i)
                    {
                        tempCount++;
                        if (tempCount > count)
                        {
                            count = tempCount;
                            popular = cars[i].name;
                        }
                    }
                    else
                        tempCount = 1;
                }
            }
        }

        /// <summary>
        /// Volvo markės automobilių sąrašas 
        /// </summary>
        /// <param name="cars"></param>
        /// <param name="count"></param>
        /// <param name="VolvoCount"></param>
        /// <returns></returns>
        static Car[] FindAllVolvo(Car[] cars, int count, out int vwCount)
        {
            var Volvo = new Car[MaxNumberOfCars];
            vwCount = 0;
            for (int i = 0; i < count; i++)
                if (cars[i].name.ToLower().Equals("volvo"))
                    Volvo[vwCount++] = cars[i];
            return Volvo;
        }

        /// <summary>
        /// Senesnių nei 10 metų automobilių sąrašas
        /// </summary>
        /// <param name="cars"></param>
        /// <param name="count"></param>
        /// <param name="oldCount"></param>
        /// <returns></returns>
        static Car[] FindOldCar(Car[] cars, int count, out int oldCount)
        {
            var oldCars = new Car[MaxNumberOfCars];
            oldCount = 0;
            DateTime now = DateTime.Now;

            for (int i = 1; i < count; i++)
                if (cars[i].year.AddYears(10) < now)
                    oldCars[oldCount++] = cars[i];

            return oldCars;
        }

        /// <summary>
        /// Pasibaigęs technikinės galiojimo laikas
        /// </summary>
        /// <param name="cars"></param>
        /// <param name="count"></param>
        /// <param name="expiredCount"></param>
        /// <returns></returns>
        static Car[] ExpiredCars(Car[] cars, int count, out int expiredCount)
        {
            var expiredCars = new Car[MaxNumberOfCars];
            expiredCount = 0;
            DateTime time = DateTime.Now.AddMonths(1);

            for (int i = 0; i < count; i++)
                if (cars[i].expireYear < time)
                    expiredCars[expiredCount++] = cars[i];

            return expiredCars;
        }

        static void SaveOldCars(Car[] oldCar, int oldCount, string fv)
        {
            using (var fr = File.CreateText(fv))
            {
                fr.WriteLine("Automobilių sąrašas, senesnių nei 10 metų \r\n");
                for (int i = 0; i < oldCount; i++)
                    fr.WriteLine(oldCar[i]);
            }
        }

        static void SaveExpiredCars(Car[] expiredCars, int expiredCount, string fv)
        {
            using (var fr = File.CreateText(fv))
            {
                fr.WriteLine("Automobiliu sąrašąs, kuriu yra pasibaigęs techninės apžiūros galiojimas,"
                    + "arba liko mažiau nei mėnuo \r\n");
                for (int i = 0; i < expiredCount; i++)
                    fr.WriteLine(expiredCars[i]);
            }
        }

    }
}

