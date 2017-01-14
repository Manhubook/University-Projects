using System;
using System.IO;
using System.Text;

namespace Automobiliu_parkas
{
    class Program
    {
        public const int MaxNumberOfBranches = 3;          //Filialu skaicius
        public const int MaxNumberOfCars = 50;             //Daugiausia leistina masinu skaicius

        public const string CDt = @"../../Apziura.txt";
        public const string CDr = @"../../Rezultatai.txt";
        public const string CDf = @"Data";

        static void Main(string[] args)
        {
            Console.WriteLine("Programa pradejo darba.");

            if (File.Exists(CDr))
                File.Delete(CDr);

            int numberOfBranches = 0;                         //Filalu skc
            var branches = new Branch[MaxNumberOfBranches];
            ReadData(CDf, branches, ref numberOfBranches);

            var allCars = new Branch(MaxNumberOfBranches * MaxNumberOfCars); //Visos masinos
            allCars = allCars.GetAllCars(branches, numberOfBranches);

            var oldestCars = new Branch(MaxNumberOfCars * MaxNumberOfBranches);                    //Seniausios masinos
            oldestCars = oldestCars.GetOldestCars(branches, numberOfBranches);

            int popularCount;
            string popular = allCars.FindPopular(out popularCount);                          //Populiariausia marke

            var _expiredCars = new Branch[MaxNumberOfBranches * MaxNumberOfCars]; //Pasibaiges apziuros galiojimo laikas
            for (int i = 0; i < numberOfBranches; i++)
                _expiredCars[i] = branches[i].FindExpiredCars();

            WriteOpeningData(branches, numberOfBranches);
            WriteOldestCarData(oldestCars);
            WritePopularData(popular, popularCount);
            WriteExpiredCars(_expiredCars, numberOfBranches);
            WriteSortedCars(branches, numberOfBranches);

            Console.WriteLine("Programa baige darba.");
            Console.ReadKey();
        }


        /// <summary>
        /// Duomenu skaitymas
        /// </summary>
        /// <param name="file"></param>
        /// <param name="branches"></param>
        static void ReadData(string file, Branch[] branches, ref int number)
        {
            string[] filePaths = Directory.GetFiles(file, "*.txt");
            foreach (var path in filePaths)
                ReadCarData(path, branches, ref number);
        }

        /// <summary>
        /// Duomenu skaitymas
        /// </summary>
        /// <param name="file"></param>
        /// <param name="branches"></param>
        /// <param name="number"></param>
        static void ReadCarData(string file, Branch[] branches, ref int number)
        {
            using (var reader = new StreamReader(@file, Encoding.GetEncoding(1257)))
            {
                string line = reader.ReadLine();
                Branch _branch = GetBranchByTown(branches, ref number, line);

                line = reader.ReadLine();
                _branch.Address = line;   //Nuskaito adresa 

                line = reader.ReadLine();
                _branch.TelNumber = line; //Nuskaito telefono numeri

                while (null != (line = reader.ReadLine()))
                {
                    switch (line[0])
                    {
                        case 'L':
                            _branch.AddCar(new LengvasisAutomobilis(line));
                            break;

                        case 'K':
                            _branch.AddCar(new KrovininisAutomobilis(line));
                            break;

                        case 'A':
                            _branch.AddCar(new Autobusas(line));
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Nuskaito miesto pav. ir gauna filalo skc.
        /// </summary>
        /// <param name="branches"></param>
        /// <param name="number"></param>
        /// <param name="town"></param>
        /// <returns></returns>
        static Branch GetBranchByTown(Branch[] branches, ref int number, string town)
        {
            for (int i = 0; i < number; i++)
                if (branches[i].Town == town)
                    return branches[i];
            branches[number++] = new Branch(town);
            return branches[number - 1];
        }

        /// <summary>
        /// Spausdina prad. duomenys
        /// </summary>
        /// <param name="branches"></param>
        /// <param name="numberOfBranches"></param>
        static void WriteOpeningData(Branch[] branches, int numberOfBranches)
        {
            using (var fr = File.CreateText(CDr))
            {
                fr.WriteLine("Pradiniai duomenys: \r\n");
                var s = new string('-', branches[0].GetCar(0).ToString().Length);
                for (int i = 0; i < numberOfBranches; i++)
                {
                    fr.WriteLine(branches[i].Town + "\r\n");
                    fr.WriteLine(s);
                    for (int j = 0; j < branches[i].Count; j++)
                        fr.WriteLine("{0} {1}", j + 1, branches[i].GetCar(j).ToString());
                    fr.WriteLine();
                }
            }
        }

        /// <summary>
        /// Spausdina filala, kuriame yra senesnes transporto priemones
        /// </summary>
        /// <param name="oldestCars"></param>
        static void WriteOldestCarData(Branch oldestCars)
        {
            using (var fr = File.AppendText(CDr))
            {
                fr.Write("Filalas, kuriame yra senesnes transporto priemones: {0} metai: {1} \r\n", oldestCars.Town, oldestCars.branchYear);
                var s = new string('-', oldestCars.GetCar(0).ToString().Length);
                fr.WriteLine(s);
                for (int i = 0; i < oldestCars.Count; i++)
                    fr.WriteLine("{0} {1}", i + 1, oldestCars.GetCar(i).ToString());
                fr.WriteLine();
            }
        }

        /// <summary>
        /// Spausdina populiariausia marke
        /// </summary>
        /// <param name="popular"></param>
        /// <param name="Count"></param>
        static void WritePopularData(string popular, int Count)
        {
            using (var fr = File.AppendText(CDr))
                fr.Write("Gamintojo transporto priemonių daugiausia {0} {1}", popular, Count);

        }

        /// <summary>
        /// Spausdina transporto priemones, kuriu baigsis galiojimo laikas uz 3men.
        /// </summary>
        /// <param name="_expiredCars"></param>
        /// <param name="numberOfBranches"></param>
        static void WriteExpiredCars(Branch[] _expiredCars, int numberOfBranches)
        {
            using (var fr = File.CreateText(CDt))
            {
                fr.WriteLine("Transporto priemonių, kurių techninės apžiūros galiojimo laikas baigsis per artimiausius 3 mėnesius. \r\n");
                var s = new string('-', _expiredCars[0].GetCar(0).ToString().Length);

                for (int i = 0; i < numberOfBranches; i++)
                {
                    fr.WriteLine(_expiredCars[i].Town);
                    fr.WriteLine(s);
                    for (int j = 0; j < _expiredCars[i].Count; j++)
                        fr.WriteLine("{0} {1}", j + 1, _expiredCars[i].GetCar(j).ToString());
                    fr.WriteLine();
                }
            }
        }

        /// <summary>
        /// Spausdina 
        /// </summary>
        /// <param name="branches"></param>
        /// <param name="numberOfBranches"></param>
        static void WriteSortedCars(Branch[] branches, int numberOfBranches)
        {
            using (var fr = File.AppendText(CDr))
            {
                fr.WriteLine("\r\n\n");
                fr.WriteLine("Isrikiuotas filalas: \r\n");
                var s = new string('-', branches[0].GetCar(0).ToString().Length);

                for (int i = 0; i < numberOfBranches; i++)
                {
                    fr.WriteLine(branches[i].Town + "\r\n");
                    fr.WriteLine(s);
                    branches[i].SortCars();
                    for (int j = 0; j < branches[i].Count; j++)
                        fr.WriteLine("{0} {1}", j + 1, branches[i].GetCar(j).ToString());
                    fr.WriteLine();
                }
            }
        }


    }
}
