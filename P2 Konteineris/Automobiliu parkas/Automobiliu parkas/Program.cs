using System;
using System.IO;
using System.Linq;

namespace Automobiliu_parkas
{
    class Car
    {
        public string Number { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public DateTime Year { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Fuel { get; set; }
        public string Consumption { get; set; }

        public Car(string carNumber, string carName, string carModel, DateTime year,
            DateTime expirationDate, string fuel, string consumption)
        {
            Number = carNumber;
            Name = carName;
            Model = carModel;
            Year = year;
            ExpirationDate = expirationDate;
            Fuel = fuel;
            Consumption = consumption;
        }

        public override string ToString()
        {
            return string.Format("Valstybinis numeris: {0,3} | Gamintojas: {1,8} | Modelis: {2,10} |"
                + "Pagaminimo metai ir menuo: {3,5} | Technines galiojimo data: {4,5} | Kuras: {5 ,9} | 100km/h: {6 , 3}|", 
                Number, Name, Model, Year.ToShortDateString(), ExpirationDate.ToShortDateString(), Fuel, Consumption);
        }

        public override bool Equals(object obj) { return this.Equals(obj as Car); }

        public bool Equals(Car car)
        {
            if (Object.ReferenceEquals(car, null))
                return false;

            if (this.GetType() != car.GetType())
                return false;

            return (Number == car.Number) && (ExpirationDate == car.ExpirationDate);
        }

        public override int GetHashCode() { return Number.GetHashCode() ^ ExpirationDate.GetHashCode(); }

        public static bool operator >(Car lhs, Car rhs)
        {
            if (object.ReferenceEquals(lhs, null))
            {
                if (object.ReferenceEquals(rhs, null))
                    return true;

                return false;
            }
            return lhs.Equals(rhs);
        }

        public static bool operator <(Car lhs, Car rhs) { return !(lhs.Name == rhs.Name); }
    }

    class Branch
    {
        public string Town { get; set; }
        public string Address { get; set; }
        public string TelNumber { get; set; }
        public CarContainer cars { get; private set; }

        public Branch(string town)
        {
            Town = town;
            cars = new CarContainer(Program.MaxNumberOfCars);
        }
    }

    class CarContainer
    {
        private Car[] cars { get; set; }
        public int Count { get; private set; }

        public CarContainer(int size) { cars = new Car[size]; }

        public void AddCar(Car car) { cars[Count++] = car; }

        public Car GetCar(int index) { return cars[index]; }

        public bool Contains(Car car) { return cars.Contains(car); }

        /// <summary>
        /// Automobiliu metu vidurkio skaiciavimas
        /// </summary>
        /// <returns></returns>
        public double AverageYear()
        {
            double sum = 0;
            double averageCarYear = 0;

            for (int i = 0; i < Count; i++)
                sum += GetCar(i).Year.Year;
            averageCarYear = sum / Count;
            averageCarYear = Math.Round(averageCarYear, MidpointRounding.AwayFromZero);
            return averageCarYear;
        }

        /// <summary>
        /// Pasibaiges technikinis
        /// </summary>
        /// <returns></returns>
        public CarContainer ExpirationDateEnded()
        {
            var _expiredCars = new CarContainer(Program.MaxNumberOfCarNames * Program.NumberOfBranches);
            DateTime now = DateTime.Now;
            DateTime Plus = DateTime.Now.AddMonths(1);

            for (int i = 0; i < Count; i++)
                if (GetCar(i).ExpirationDate == now || GetCar(i).ExpirationDate < Plus)
                    _expiredCars.AddCar(GetCar(i));

            return _expiredCars;
        }

        /// <summary>
        /// Gauna visas masinas
        /// </summary>
        /// <param name="branches"></param>
        /// <returns></returns>
        public CarContainer AllCars(Branch[] branches)
        {
            var allCars = new CarContainer(Program.MaxNumberOfCars * Program.NumberOfBranches);
            for (int i = 0; i < Program.NumberOfBranches; i++)
                for (int j = 0; j < branches[i].cars.Count; j++)
                    allCars.AddCar(branches[i].cars.GetCar(j));

            return allCars;
        }
    }

    class Program
    {
        public const string CDr = "../../Klaidos.csv";
        public const string CDn = "../../Apziura.csv";
        public const int NumberOfBranches = 2; //Filialu skaicius
        public const int MaxNumberOfCarNames = 100;
        public const int MaxNumberOfCars = 50;

        static void Main(string[] args)
        {
            string[] filePaths;
            Branch[] branches = BranchMethod(FileMax(out filePaths), filePaths);

            foreach (string path in filePaths)
                ReadCarData(path, branches);

            ///-----------------------------------------------------------
            ///Senienos

            var OldCars = new Branch(branches[0].Town);
            double[] year = new double[NumberOfBranches];
            for (int i = 0; i < NumberOfBranches; i++)
                year[i] = branches[i].cars.AverageYear();

            OldCars = branches[FindOldestYear(year)];
            Console.WriteLine("Senienos \r\n");
            Print(OldCars);

            ///-----------------------------------------------------------
            ///Populiariausios markes

            string Popular;
            int Count;
            Popular = GetMostPopularCarName(branches, out Count);
            Console.WriteLine("\r\nPopuliariausios masinos {0} ju kiekis - {1}", Popular, Count);

            ///-----------------------------------------------------------
            ///Dvigubai uzregistruotos masinos

            string town;
            CarContainer doublePlacedCars = GetDoublePlacedCars(branches[0], branches[1], out town);

            ///-----------------------------------------------------------
            ///Galiojimo laikas

            var expiredCars = new CarContainer(MaxNumberOfCars * NumberOfBranches);
            expiredCars = expiredCars.AllCars(branches);
            expiredCars = expiredCars.ExpirationDateEnded();


            SaveDoublePlacedCars(doublePlacedCars, branches, town);
            SaveExpiredCars(expiredCars);

            Console.ReadKey();
        }

        private static void Print(Branch _cars)
        {
            for (int i = 0; i < _cars.cars.Count; i++)
                Console.WriteLine(_cars.cars.GetCar(i).ToString());
        }

        /// <summary>
        /// Suranda seniausius metus
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        static int FindOldestYear(double[] year)
        {
            double tempYear = 0;
            int index = 0;
            for (int i = 0; i < NumberOfBranches; i++)
            {
                if (tempYear < year[i])
                {
                    tempYear = year[i];
                    index = i;
                }
            }

            return index;
        }

        private static int FileMax(out string[] filePaths)
        {

            filePaths = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.csv");
            int i = 0;

            foreach (string files in filePaths)
                i++;

            return i;
        }

        private static Branch[] BranchMethod(int fileNum, string[] filePaths)
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

        private static Branch GetBranch(Branch[] branches, string town)
        {
            for (int i = 0; i < NumberOfBranches; i++)
                if (branches[i].Town == town)
                    return branches[i];

            return null;
        }

        private static void ReadCarData(string file, Branch[] branches)
        {
            string town = null;

            using (StreamReader reader = new StreamReader(@file))
            {
                string line = null;
                line = reader.ReadLine();
                if (null != line)
                    town = line;

                Branch branch = GetBranch(branches, town);

                line = reader.ReadLine();
                branch.Address = line;

                line = reader.ReadLine();
                branch.TelNumber = line;

                while (null != (line = reader.ReadLine()))
                {
                    string[] values = line.Split(';');
                    string carNumber = values[0];  //Valstybinis numeris
                    string carName = values[1];  //Gamintojas
                    string carModel = values[2];  //Modelis
                    DateTime year = DateTime.Parse(values[3]); // Metai
                    DateTime expirationDate = DateTime.Parse(values[4]);  // Technikėnes galiojimo laiaks
                    string fuel = values[5]; // Kuro tipas
                    string averageconsumption = values[6];  // 100km/h
                    Car car = new Car(carNumber, carName, carModel, year, expirationDate, fuel, averageconsumption);
                    branch.cars.AddCar(car);
                }
            }
        }

        /// <summary>
        ///  Ieskoma visu masinu markes pavadinimai
        /// </summary>
        /// <param name="branches"></param>
        /// <param name="CarNames"></param>
        /// <param name="CarNameCount"></param>
        private static void GetCarName(Branch[] branches, out string[] CarNames, out int CarNameCount)
        {
            CarNames = new string[MaxNumberOfCarNames];
            CarNameCount = 0;

            for (int i = 0; i < NumberOfBranches; i++)
                for (int j = 0; j < branches[i].cars.Count; j++)
                    if (!CarNames.Contains(branches[i].cars.GetCar(j).Name))
                        CarNames[CarNameCount++] = branches[i].cars.GetCar(j).Name;
        }

        private static CarContainer FilterByCarNames(Branch[] branches, string carName)
        {
            CarContainer filteredCars = new CarContainer(MaxNumberOfCars);
            for (int i = 0; i < NumberOfBranches; i++)
                for (int j = 0; j < branches[i].cars.Count; j++)
                    if (branches[i].cars.GetCar(j).Name == carName)
                        filteredCars.AddCar(branches[i].cars.GetCar(j));

            return filteredCars;
        }

        /// <summary>
        /// Ieškomas daugiausiai pasikartojamas gamintojo pavadinimas, bei automobilių kiekis.
        /// </summary>
        /// <param name="branches"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static string GetMostPopularCarName(Branch[] branches, out int count)
        {
            string popular = "not found";
            count = 0;

            int CarNamesCount;
            string[] CarNames;

            GetCarName(branches, out CarNames, out CarNamesCount);
            for (int i = 0; i < NumberOfBranches; i++)
            {
                for (int j = 0; j < branches[i].cars.Count; j++)
                {
                    CarContainer filtered = FilterByCarNames(branches, CarNames[j]);
                    if (filtered.Count > count)
                    {
                        popular = CarNames[j];
                        count = filtered.Count;
                    }
                }
            }

            return popular;
        }

        /// <summary>
        /// Dvigubai uzregistruoti automobiliai
        /// </summary>
        /// <param name="branch1"></param>
        /// <param name="branch2"></param>
        /// <param name="Town"></param>
        /// <returns></returns>
        private static CarContainer GetDoublePlacedCars(Branch branch1, Branch branch2, out string Town)
        {
            Town = "";
            CarContainer doublePlacedDogs = new CarContainer(MaxNumberOfCars);
            for (int i = 0; i < branch1.cars.Count; i++)
            {
                if (branch2.cars.Contains(branch1.cars.GetCar(i)))
                {
                    doublePlacedDogs.AddCar(branch1.cars.GetCar(i));
                    Town = branch1.Town;
                }
            }
            return doublePlacedDogs;
        }

        /// <summary>
        /// Rezultu rašymas į failą
        /// </summary>
        /// <param name="cars"></param>
        /// <param name="branches"></param>
        /// <param name="town"></param>
        private static void SaveDoublePlacedCars(CarContainer cars, Branch[] branches, string town)
        {
            using (StreamWriter writer = new StreamWriter(CDr))
            {
                writer.WriteLine("Dvigubai uzregistruotos masinos: \r\n");
                for (int i = 0; i < cars.Count; i++)
                    writer.WriteLine("{0} {1} {2}", cars.GetCar(i).Number, cars.GetCar(i).Model, town);
            }
        }

        private static void SaveExpiredCars(CarContainer expiredCars)
        {
            using (StreamWriter writer = new StreamWriter(CDn))
                for (int i = 0; i < expiredCars.Count; i++)
                    writer.WriteLine("{0} {1} {2}", expiredCars.GetCar(i).Name, expiredCars.GetCar(i).Model,
                        expiredCars.GetCar(i).Number, expiredCars.GetCar(i).ExpirationDate);

        }
    }
}
