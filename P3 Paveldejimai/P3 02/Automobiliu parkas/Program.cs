using System;
using System.IO;
using System.Linq;

namespace Automobiliu_parkas
{
    class Program
    {
        public const int NumberOfBranches = 2;                 //Filialu skaicius
        public const int CMaxCars = 50;                 //Daugiausia leistina masinu skaicius
        public const int CMaxBranches = 3;
        public const string CDr = @"../../Apziura.txt"; //Rezultatu failas
        public const string CDn = @"../../Rezultatai.txt"; //Rezulatu failas

        static void Main(string[] args)
        {
            string[] filePaths; // Failo vieta
            Branch[] branches = BranchMethod(FileMax(out filePaths), filePaths); //Kuriama filalu klase

            foreach (string path in filePaths)
                ReadCarData(path, branches);

            ///Seniausios masinos filale
            ///-----------------------------------------------------------------------------

            double year;
            var oldCars = FindOldestCars(branches, out year);
            Console.WriteLine(year + "\r\n");
            Print(oldCars);

            ///Gaunamos visos transporto priemones
            ///-----------------------------------------------------------------------------

            var allCars = new CarContainer(CMaxBranches * CMaxCars);
            for (int i = 0; i < NumberOfBranches; i++)
                for (int j = 0; j < branches[i].allCars.Count; j++)
                    allCars.AddCar(branches[i].allCars.GetCar(j));

            ///-----------------------------------------------------------------------------
            ///Populiariausios markes

            string Popular; //Populiariausia marke
            int Count;      //Populiariausios markes kiekiu skaicius
            Popular = GetMostPopularCarName(allCars, out Count); //Randama populiariausia marke
            Console.WriteLine("\r\nPopuliariausios masinos {0} ju kiekis - {1}", Popular, Count);


            ///Rusiavimas
            ///-----------------------------------------------------------------------------

            SaveSortedCars(branches, CDn);

            ///Pasibaiges galiojimas technikines
            ///-----------------------------------------------------------------------------

            var expiredCars = new CarContainer(CMaxBranches * CMaxCars);
            expiredCars = allCars.ExpiredCars();
            SaveExpiredCars(expiredCars, CDr);

            Console.ReadKey();
        }


        /// <summary>
        /// Randa failu skaiciu
        /// </summary>
        /// <param name="filePaths"></param>
        /// <returns></returns>
        static int FileMax(out string[] filePaths)
        {
            filePaths = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.csv");
            int i = 0;

            foreach (string files in filePaths)
                i++;

            return i;
        }

        static Branch[] BranchMethod(int fileNum, string[] filePaths)
        {
            Branch[] branches = new Branch[fileNum];

            for (int i = 0; i < fileNum; i++)
            {
                using (StreamReader reader = new StreamReader(filePaths[i]))
                {
                    string line = reader.ReadLine();
                    branches[i] = new Branch(line);
                }
            }

            return branches;
        }

        /// <summary>
        /// Paima filalo miesto pavadinima
        /// </summary>
        /// <param name="branches"></param>
        /// <param name="town"></param>
        /// <returns></returns>
        static Branch GetBranch(Branch[] branches, string town)
        {
            for (int i = 0; i < NumberOfBranches; i++)
                if (branches[i].Town == town)
                    return branches[i];

            return null;
        }

        /// <summary>
        /// Duomenu skaitymas
        /// </summary>
        /// <param name="file"></param>
        /// <param name="branches"></param>
        static void ReadCarData(string file, Branch[] branches)
        {
            string town = null;
            using (StreamReader reader = new StreamReader(@file))
            {
                string line = null;
                line = reader.ReadLine();

                if (null != line)
                    town = line;         //Nuskaito miesto pav

                Branch branch = GetBranch(branches, town);
                line = reader.ReadLine();
                branch.Address = line;   //Nuskaito adresa 
                line = reader.ReadLine();
                branch.TelNumber = line; //Nuskaito telefono numeri

                while (null != (line = reader.ReadLine()))
                {
                    string[] values = line.Split(';');
                    char type = Convert.ToChar(line[0]);                  //Transporto priemone
                    string carNumber = values[1];                         //Valstybinis numeris
                    string carName = values[2];                           //Gamintojas
                    string carModel = values[3];                          //Modelis
                    DateTime year = DateTime.Parse(values[4]);            // Metai
                    DateTime expirationDate = DateTime.Parse(values[5]);  // Technikėnes galiojimo laiaks
                    string fuel = values[6];                              // Kuro tipas
                    string averageconsumption = values[7];                // 100km/h

                    switch (type)
                    {
                        case 'L':
                            LengvasisAutomobilis lengAuto = new LengvasisAutomobilis(carNumber, carName, carModel, year,
                                expirationDate, fuel, averageconsumption);
                            if (!branch.lengCar.Contains(lengAuto))
                                branch.lengCar.AddCar(lengAuto);
                            branch.allCars.AddCar(lengAuto);
                            break;

                        case 'K':
                            int priekabosTalpa = int.Parse(values[8]);
                            KrovininisAutomobilis krovAuto = new KrovininisAutomobilis(carNumber, carName, carModel, year,
                                expirationDate, fuel, averageconsumption, priekabosTalpa);
                            if (!branch.krovCar.Contains(krovAuto))
                                branch.krovCar.AddCar(krovAuto);
                            branch.allCars.AddCar(krovAuto);
                            break;

                        case 'A':
                            int vietuSkc = int.Parse(values[8]);
                            Autobusas autoBus = new Autobusas(carNumber, carName, carModel, year,
                                expirationDate, fuel, averageconsumption, vietuSkc);
                            if (!branch.autoCar.Contains(autoBus))
                                branch.autoCar.AddCar(autoBus);
                            branch.allCars.AddCar(autoBus);
                            break;
                    }
                }
            }
        }

        static CarContainer FindOldestCars(Branch[] branches, out double year)
        {
            year = branches[0].allCars.CarYearAverage(); ;
            var oldCars = branches[0].allCars;
            double tempyear = 0;

            for (int i = 0; i < NumberOfBranches; i++)
            {
                tempyear = branches[i].allCars.CarYearAverage();
                if (year > tempyear)
                {
                    year = tempyear;
                    oldCars = branches[i].allCars;
                }
            }

            return oldCars;
        }

        static void Print(CarContainer cars)
        {
            for (int i = 0; i < cars.Count; i++)
                Console.WriteLine(cars.GetCar(i).ToString());
        }

        /// <summary>
        /// Ieskomi visi markes pavadinimai
        /// </summary>
        /// <param name="AllCars"></param>
        /// <param name="CarNames"></param>
        /// <param name="CarNameCount"></param>
        private static void GetCarName(CarContainer AllCars, out string[] CarNames, out int CarNameCount)
        {
            CarNames = new string[CMaxCars];
            CarNameCount = 0;

            for (int i = 0; i < AllCars.Count; i++)
                if (!CarNames.Contains(AllCars.GetCar(i).CarName))
                    CarNames[CarNameCount++] = AllCars.GetCar(i).CarName;
        }

        /// <summary>
        /// Filtruoja transporto priemones pavadinimus ir deda juos i nauja konteineri
        /// </summary>
        /// <param name="AllCars"></param>
        /// <param name="carName"></param>
        /// <returns></returns>
        private static CarContainer FilterByCarNames(CarContainer AllCars, string carName)
        {
            CarContainer filteredCars = new CarContainer(CMaxCars);
            for (int i = 0; i < AllCars.Count; i++)
                if (AllCars.GetCar(i).CarName == carName)
                    filteredCars.AddCar(AllCars.GetCar(i));

            return filteredCars;
        }

        /// <summary>
        /// Ieškomas daugiausiai pasikartojamas gamintojo pavadinimas, bei automobilių kiekis.
        /// </summary>
        /// <param name="AllCars"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static string GetMostPopularCarName(CarContainer AllCars, out int count)
        {
            string popular = "not found";
            count = 0;
            int CarNamesCount;
            string[] CarNames;

            GetCarName(AllCars, out CarNames, out CarNamesCount);
            for (int j = 0; j < AllCars.Count; j++)
            {
                CarContainer filtered = FilterByCarNames(AllCars, CarNames[j]);
                if (filtered.Count > count)
                {
                    popular = CarNames[j];
                    count = filtered.Count;
                }
            }

            return popular;
        }

        static void SaveSortedCars(Branch[] branches, string fv)
        {
            using (var fr = File.CreateText(fv))
            {
                fr.WriteLine("Surisiuotos transporto priemones \r\n");
                string s = new string('=', branches[0].allCars.GetCar(0).ToString().Length);
                fr.WriteLine(s);
                for (int i = 0; i < NumberOfBranches; i++)
                {
                    branches[i].allCars.Sort();
                    for (int j = 0; j < branches[i].allCars.Count; j++)
                        fr.WriteLine(branches[i].allCars.GetCar(j).ToString());
                    fr.WriteLine(s);
                }
            }
        }

        static void SaveExpiredCars(CarContainer expiredCars, string fv)
        {
            using (var fr = File.CreateText(fv))
            {
                fr.WriteLine("Transporto priemones, kuriu tech. gal. laikas yra pasibaiges\r\n");

                for (int i = 0; i < expiredCars.Count; i++)
                    fr.WriteLine("{0, -7} {1, -6} {2, -7} {3 :d}", expiredCars.GetCar(i).CarName,
                       expiredCars.GetCar(i).CarModel, expiredCars.GetCar(i).CarNumber, expiredCars.GetCar(i).ExpirationDate);
            }
        }
    }
}
