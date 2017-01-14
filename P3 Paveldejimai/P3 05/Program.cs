using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoMusis
{
    class Program
    {
        public const int NumberOfBranches = 2;
        public const int MaxNumberOfAutoriai = 10;
        public const string RfTemos = @"..\..\TemuSkacius.csv";
        public const string RfKlausimai = @"..\..\VienodiKlausimai.csv";

        static void Main(string[] args)
        {
            //Duomenu skaitymas
            string[] filePaths;
            
            Branch[] branches = BranchMethod(filemax(out filePaths), filePaths);
           
            foreach (string path in filePaths)
            {
                ReadKlausimaiData(path, branches);
            }

            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            //Spausdinimas i Konsole
            for (int i = 0; i < NumberOfBranches; i++)
            {
                Console.WriteLine(branches[i].Atstovybe);
                SpausdintiEkrane(branches[i].Klausimai);
                Console.WriteLine();
            }

            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            //Ieskomas populiariausias autorius per visas atstovybes ir ieskomi autoriai populiarus savo atstovybeje
            int autoriusCount;
            string[] fakAutor;
            int fakAutorKiek;
            string autorius = DaugAutorius(branches, out autoriusCount, out fakAutor, out fakAutorKiek);
            Console.WriteLine("Daugiausiai klausimu sukures autorius ir ju kiekis: {0} {1}", autorius, autoriusCount);


            for (int i = 0; i < NumberOfBranches; i++)
            {
                Console.WriteLine("Autorius {0}, kuris sukūrė daugiausiai klausimų {1} atstovybėje ir ju kiekis {2}", fakAutor[i], branches[i].Atstovybe, fakAutorKiek);
            }

            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            string[] Temos;
            int TemosCount;
            int TemosSkc;
            GetTemos(branches, out Temos, out TemosCount, out TemosSkc);
            Console.WriteLine();

            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            //Ieskomi pasiskolinti klausimai
            KlausimaiContainer PatikeKlau = PatikeKlausimai(branches[0], branches[1]);
            Console.WriteLine("Paskolinti klausimai kitu autoriu:");
            SpausdintiEkrane(PatikeKlau);

            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            //Rezultatai rasomi i faila

            IsvestiDuomenisLentele(RfTemos, branches);
            KlausimuSarasas(RfKlausimai, PatikeKlau);

            Console.ReadKey();
        }

        private static int filemax(out string[] filePaths) //Randa failu skaciu
        {
            filePaths = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.csv");
            int i = 0;

            foreach (string files in filePaths)
            {
                i++;
            }

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

        private static Branch GetBranchByAtstovybe(Branch[] branches, string atstovybe)
        {
            for (int i = 0; i < NumberOfBranches; i++)
            {
                if (branches[i].Atstovybe == atstovybe)
                {
                    return branches[i];
                }
            }
            return null;
        }

        private static void ReadKlausimaiData(string file, Branch[] branches) //Duomenu skaitymas
        {

            string atstovybe = null;

            using (StreamReader reader = new StreamReader(@file))
            {
                string line = null;
                line = reader.ReadLine();
                if (line != null)
                {
                    atstovybe = line;
                }
                Branch branch = GetBranchByAtstovybe(branches, atstovybe);
                while (null != (line = reader.ReadLine()))
                {
                    string[] values = line.Split(';');

                    string tema = values[0];
                    double sudetingumas = double.Parse(values[1]);
                    string autorius = values[2];
                    string tekstas = values[3];
                    string atsakymai = values[4];
                    string teisingasats = values[5];
                    double balai = double.Parse(values[6]);

                    Klausimai klausimai = new Klausimai(tema, sudetingumas, autorius, tekstas, atsakymai, teisingasats, balai);

                    branch.Klausimai.AddKlausimai(klausimai);
                }
            }
        }

        public static void GetAutoriai(Branch[] branches, out string[] autoriai, out int autoriaiCount) //Randa visus autorius
        {
            autoriai = new string[MaxNumberOfAutoriai];
            autoriaiCount = 0;

            for (int i = 0; i < NumberOfBranches; i++)
            {
                for (int j = 0; j < branches[i].Klausimai.Count; j++)
                {
                    if (!autoriai.Contains(branches[i].Klausimai.GetKlausimai(j).Autorius))
                    {
                        autoriai[autoriaiCount++] = branches[i].Klausimai.GetKlausimai(j).Autorius;
                    }
                }
            }
        }

        static void SpausdintiEkrane(KlausimaiContainer Klausimai) //Spausdina i konsole
        {
            for (int j = 0; j < Klausimai.Count; j++)
            {
                Console.WriteLine("{0}) {1}", (j + 1), Klausimai.GetKlausimai(j).ToString());
            }
        }

        private static KlausimaiContainer FilterByAutorius(Branch[] branches, string autorius)
        {
            KlausimaiContainer filteredKlausimas = new KlausimaiContainer(Branch.MaxNumberOfKlausimai);
            for (int i = 0; i < NumberOfBranches; i++)
            {

                for (int j = 0; j < branches[i].Klausimai.Count; j++)
                {
                    if (branches[i].Klausimai.GetKlausimai(j).Autorius == autorius)
                    {
                        filteredKlausimas.AddKlausimai(branches[i].Klausimai.GetKlausimai(j));
                    }
                }
            }
            return filteredKlausimas;

        }

        private static string DaugAutorius(Branch[] branches, out int count, out string[] vardas, out int tempcount) //Randa populiariausia autoriu tarp atstovybiu, taip pat, randa autoriu, kuris yra populiarus savo atstovybeje
        {
            String popular = "not found";
            count = 0;

            int AutoriusCount;
            string[] autoriai;
            GetAutoriai(branches, out autoriai, out AutoriusCount);

            string laikinasFakulVardas = branches[0].Klausimai.GetKlausimai(0).Autorius;
            tempcount = 0;
            int tempCount = 0;
            vardas = new string[MaxNumberOfAutoriai];

            for (int i = 0; i < NumberOfBranches; i++)
            {
                for (int j = 0; j < branches[i].Klausimai.Count; j++)
                {
                    if (laikinasFakulVardas == branches[i].Klausimai.GetKlausimai(j).Autorius)
                    {
                        tempcount++;
                    }

                    if (tempcount > tempCount)
                    {
                        vardas[i] = branches[i].Klausimai.GetKlausimai(j).Autorius;
                    }

                    KlausimaiContainer filtered = FilterByAutorius(branches, autoriai[j]);
                    if (filtered.Count > count)
                    {
                        popular = autoriai[j];
                        count = filtered.Count;
                    }
                }
            }

            return popular;
        }

        public static void GetTemos(Branch[] branches, out string[] Temos, out int TemosCount, out int TemosSkc)
        {
            Temos = new string[MaxNumberOfAutoriai];
            TemosCount = 0;
            TemosSkc = 0;
            string laikinas = branches[0].Klausimai.GetKlausimai(0).Tema;

            for (int i = 0; i < NumberOfBranches; i++)
            {
                for (int j = 0; j < branches[i].Klausimai.Count; j++)
                {
                    if (!Temos.Contains(branches[i].Klausimai.GetKlausimai(j).Tema))
                    {
                        Temos[TemosCount++] = branches[i].Klausimai.GetKlausimai(j).Tema;
                    }
                }
            }
        }

        static void IsvestiDuomenisLentele(string rezultatuFailas, Branch[] branches)
        {
            string[] Temos;
            int TemosCount;
            int TemosSkc;
            GetTemos(branches, out Temos, out TemosCount, out TemosSkc);
            using (StreamWriter isvedimas = new StreamWriter(rezultatuFailas))
            {
                isvedimas.WriteLine("Klausimu sarasas: ");
                for (int i = 0; i < TemosCount; i++)
                {
                    isvedimas.WriteLine(Temos[i]);
                }
            }
        }

        private static KlausimaiContainer PatikeKlausimai(Branch branch1, Branch branch2)
        {
            KlausimaiContainer patikeKlausimai = new KlausimaiContainer(Branch.MaxNumberOfKlausimai);

            for (int i = 0; i < branch1.Klausimai.Count; i++)
            {
                if (branch2.Klausimai.GetKlausimai(i).Tekstas.Contains(branch1.Klausimai.GetKlausimai(i).Tekstas))
                {
                    patikeKlausimai.AddKlausimai(branch1.Klausimai.GetKlausimai(i));
                }
            }

            return patikeKlausimai;
        }

        static void KlausimuSarasas(string rezultatuFailas, KlausimaiContainer Klausimai)
        {
            using (StreamWriter isvedimas = new StreamWriter(rezultatuFailas))
            {
                for (int j = 0; j < Klausimai.Count; j++)
                {
                    isvedimas.WriteLine("{0}) {1}", (j + 1), Klausimai.GetKlausimai(j).ToString());
                }
            }
        }
    }
}
            
    

    
