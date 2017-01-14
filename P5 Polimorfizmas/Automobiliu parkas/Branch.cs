using System;
using System.Linq;

namespace Automobiliu_parkas
{
    class Branch
    {
        private TransportoPriemone[] _transportoPriemone;
        public string Town       { get; private set; } //Miesto pav
        public string Address    { get; set; }        //Adresas
        public string TelNumber  { get; set; }        //Tlf num.
        public int Count         { get; private set; } //Transportu priemoniu skc
        public double branchYear { get; private set; } //Filalo vid. tran. prie. metai

        public Branch(int size) { _transportoPriemone = new TransportoPriemone[size]; }

        public Branch(string town = "")
        {
            Town = town;
            _transportoPriemone = new TransportoPriemone[Program.MaxNumberOfCars];
        }

        public void AddCar(TransportoPriemone car) { _transportoPriemone[Count++] = car; }

        public TransportoPriemone GetCar(int i) { return _transportoPriemone[i]; }

        /// <summary>
        /// Sudeda 2 klase i 1
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Branch operator +(Branch a, Branch b)
        {
            var c = new Branch(a.Town);
            for (int i = 0; i < a.Count; i++)
                c.AddCar(a.GetCar(i));
            for (int i = 0; i < b.Count; i++)
                c.AddCar(b.GetCar(i));

            return c;
        }

        /// <summary>
        /// Gauna visos transporto priemones is visu filalu
        /// </summary>
        /// <param name="branches"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public Branch GetAllCars(Branch[] branches, int number)
        {
            var allCars = new Branch(Program.MaxNumberOfBranches * Program.MaxNumberOfCars);
            for (int i = 0; i < number; i++)
                if (branches[i] != null)
                    for (int j = 0; j < branches[i].Count; j++)
                        allCars.AddCar(branches[i].GetCar(j));

            return allCars;
        }


        /// Skaiciuoja filalo vid. transportu priemoniu pagaminimo metus
        /// </summary>
        /// <param name="cars"></param>
        /// <returns></returns>
        public double GetAvarageYear()
        {
            var sum = 0;
            double averageYear = 0;
            for (int j = 0; j < Count; j++)
                sum += GetCar(j).Year.Year;
            averageYear = sum / Count;
            averageYear = Math.Round(averageYear, MidpointRounding.AwayFromZero);

            return averageYear;
        }

        /// <summary>
        /// Suranda, kuriame filale yra senesnes masinos ir to filalo duomenys saugo i nauja konteineri
        /// </summary>
        /// <param name="branches"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public Branch GetOldestCars(Branch[] branches, int number)
        {
            var _oldestCars = new Branch(Program.MaxNumberOfBranches * Program.MaxNumberOfCars);
            _oldestCars = branches[0];
            double Year = branches[0].GetAvarageYear();
            _oldestCars.branchYear = Year;
            for (int i = 0; i < number; i++)
                if (branches[i] != null)
                    if (Year > branches[i].GetAvarageYear())
                    {
                        _oldestCars = branches[i];
                        Year = branches[i].GetAvarageYear();
                        branchYear = Year;
                    }

            return _oldestCars;
        }

        /// <summary>
        /// Gauna visas transportu priemoniu markes pav
        /// </summary>
        /// <param name="carNames"></param>
        public void GetCarNames(out string[] carNames)
        {
            int j = 0;
            carNames = new string[Program.MaxNumberOfCars * Program.MaxNumberOfBranches];
            for (int i = 0; i < Count; i++)
                if (!carNames.Contains(GetCar(i).Name))
                    carNames[j++] = GetCar(i).Name;
        }

        /// <summary>
        /// Filtruoja markes pav, kad liktu tik  pavadinimas
        /// </summary>
        /// <param name="carName"></param>
        /// <returns></returns>
        public Branch FilterCarNames(string carName)
        {
            var _filteredNames = new Branch(Program.MaxNumberOfCars);
            for (int i = 0; i < Count; i++)
                if (GetCar(i).Name == carName)
                    _filteredNames.AddCar(GetCar(i));

            return _filteredNames;
        }

        /// <summary>
        /// Suranda populiariausia markes pav
        /// </summary>
        /// <returns></returns>
        public string FindPopular(out int count)
        {
            string popular = "nerasta";
            string[] carNames;
            count = 0;

            GetCarNames(out carNames);
            for (int i = 0; i < Count; i++)
            {
                var _filteredNames = FilterCarNames(carNames[i]);
                if (_filteredNames.Count > count)
                {
                    popular = carNames[i];
                    count = _filteredNames.Count;
                }
            }

            return popular;
        }

        /// <summary>
        /// Iesko transporto priemones, kuriu technikines galiojimo laikas baigsis per artimuosius 3 men.
        /// </summary>
        /// <param name="expiredCars"></param>
        /// <returns></returns>
        public Branch FindExpiredCars()
        {
            var expiredCars = new Branch(Program.MaxNumberOfCars * Program.MaxNumberOfBranches);  //Kuriamas konteineris, kuriame bus saugoma transporto priemones, 
                                                                                                     //kuriu galiojimo laikas pasibaiges ar bus pasibaiges po 3men.
            var DtNowPlus3Months = DateTime.Now.AddMonths(3);        //Dabartinis laikas + 3men
            var DtNow = DateTime.Now;                                //Dabartinis laikas

            for (int j = 0; j < Count; j++)
                if (GetCar(j).ExpirationDate < DtNowPlus3Months && GetCar(j).ExpirationDate > DtNow)
                {
                    expiredCars.AddCar(GetCar(j));
                    expiredCars.Town = Town;
                }

            return expiredCars;
        }

        public void SortCars()
        {
            for (int i = 0; i < Count - 1; i++)
            {
                int m = i;
                for (int j = i + 1; j < Count; j++)
                    if (_transportoPriemone[j] <= _transportoPriemone[m])
                        m = j;
                TransportoPriemone a = _transportoPriemone[i];
                _transportoPriemone[i] = _transportoPriemone[m];
                _transportoPriemone[m] = a;
            }
        }
    }
}
