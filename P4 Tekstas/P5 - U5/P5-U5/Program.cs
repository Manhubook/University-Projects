using System;
using System.IO;
using System.Text.RegularExpressions;

namespace P5_U5
{
    class Program
    {
        public const string CDf1 = @"..\..\Knyga1.txt";
        public const string CDf2 = @"..\..\Knyga2.txt";
        public const string CDr = @"..\..\Analize.txt";
        public const string CDn = @"..\..\ManoKnyga.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Programa pradeda darba.");

            if (File.Exists(CDf1) && File.Exists(CDf2))
            {
                Apdorti();
            }

            Console.WriteLine("Programa baigia darba.");
            Console.ReadLine();
        }

        private static void Apdorti()
        {
            char[] skyrikliai = { '.', ',', '?', '!' };
            string[] eilutes1 = File.ReadAllLines(CDf1);
            string[] eilutes2 = File.ReadAllLines(CDf2);

            string[] nepasikartojantyszod = new string[10];
            int nepZodIndex = 0;


            int eilutesNr1 = 1;
            int simboliuSk1 = 0;
            int jungimai1 = 0;
            string jungtinis1 = "";
            int eil1 = 1;
            string ilgSakinys1 = string.Empty;

            int eilutesNr2 = 1;
            int simboliuSk2 = 0;
            int jungimai2 = 0;
            string jungtinis2 = "";
            int eil2 = 1;
            string ilgSakinys2 = string.Empty;

            for (int i = 0; i < eilutes1.Length; i++)
            {
                RastiIlgiausiaEil(eilutes1[i], skyrikliai, eil1, ref ilgSakinys1, ref jungtinis1, ref eilutesNr1, ref simboliuSk1, ref jungimai1);
                if (eilutes2[i] != "" && !string.IsNullOrEmpty(eilutes1[i]))
                {
                    Rastizodzius(eilutes1[i], eilutes2[i], ref nepasikartojantyszod, ref nepZodIndex);
                }
            }


            for (int j = 0; j < eilutes2.Length; j++)
            {
                RastiIlgiausiaEil(eilutes2[j], skyrikliai, eil2, ref ilgSakinys2, ref jungtinis2, ref eilutesNr2, ref simboliuSk2, ref jungimai2);
            }

            using (StreamWriter fr1 = File.CreateText(CDr))
            {
                fr1.WriteLine("žodžiai, kurie yra tik faile Knyga1.txt, bet nėra faile Knyga2.txt, skaičius {0} ir sąrašąs:", nepZodIndex);
                fr1.WriteLine();

                for (int i = 0; i < nepZodIndex; i++)
                {
                    fr1.WriteLine(nepasikartojantyszod[i]);
                }

                fr1.WriteLine("Ilgiausias sakinys pirmajame faile:  {0}", ilgSakinys1);
                fr1.WriteLine("Jos ilgis: {0}", simboliuSk1);
                fr1.WriteLine("Vieta {0}", eilutesNr1);
                fr1.WriteLine();

                fr1.WriteLine("Ilgiausias sakinys antrajame faile:  {0}", ilgSakinys2);
                fr1.WriteLine("Jos ilgis: {0}", simboliuSk2);
                fr1.WriteLine("Vieta {0}", eilutesNr2);

                using (StreamWriter fr2 = File.CreateText(CDn))
                {
                    fr2.WriteLine("Kopijuojamas tekstas");
                    fr2.WriteLine();
                }
            }

        }


        /// <summary>
        /// Randa zodzius kurie yra tekstas 1 bet nera tekstas 2 
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <param name="nepasikartojantysZod"></param>
        /// <param name="nepZodIndex"></param>
        private static void Rastizodzius(string e1, string e2, ref string[] nepasikartojantysZod, ref int nepZodIndex)
        {
            string[] zodziai1 = Regex.Split(e1, @"\s+"); //Eilute yra skaidoma i zodzius
            string[] zodziai2 = Regex.Split(e2, @"\s+");

            if (zodziai1 != null && zodziai2 != null)
            {
                for (int i = 0; i < zodziai1.Length; i++)
                {
                    int laikinas = 0;
                    for (int j = 0; j < zodziai2.Length; j++)
                    {
                        if (!string.IsNullOrWhiteSpace(zodziai1[i]) || !string.IsNullOrWhiteSpace(zodziai2[j]))
                        {
                            if (zodziai1[i].ToLower() != (zodziai2[j].ToLower())) // jei zodziai nesutampa
                            {
                                if (nepZodIndex < 10)
                                {
                                    while (laikinas < 1)
                                    {
                                        nepasikartojantysZod[nepZodIndex++] = zodziai1[i];
                                        laikinas++;
                                    }
                                }
                            }
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Randa ilgiausia eilute, jos vieta ir ilgi
        /// </summary>
        /// <param name="e"></param>
        /// <param name="sk"></param>
        /// <param name="eil"></param>
        /// <param name="ilgiausiasSakinys"></param>
        /// <param name="jungtinis"></param>
        /// <param name="eilutesNr"></param>
        /// <param name="simboliuSk"></param>
        /// <param name="jungimuSk"></param>
        private static void RastiIlgiausiaEil(string e, char[] sk, int eil, ref string ilgiausiasSakinys,
                                                    ref string jungtinis, ref int eilutesNr, ref int simboliuSk, ref int jungimuSk)
        {
            bool arSakinysBaigesi = false;
            var duomenys = Regex.Split(e, @"(?<=[.?!])");

            foreach (var dalis in duomenys)
            {
                if (dalis != "")
                {
                    string sakinys = jungtinis + dalis;
                    foreach (var skyriklis in sk)
                    {
                        if (dalis[dalis.Length - 1] == skyriklis) //Ar sakinys baigiasi
                        {
                            if (sakinys.Length > simboliuSk)
                            {
                                ilgiausiasSakinys = sakinys; //Ilgiausias sakinys
                                simboliuSk = sakinys.Length; //Ilgiausio sakinio dydis
                                eilutesNr = eil - jungimuSk; //Eilutes nr
                                jungtinis = "";
                                jungimuSk = 0;
                                arSakinysBaigesi = true;
                            }

                            else if (sakinys.Length < simboliuSk)
                            {
                                jungtinis = "";
                                jungimuSk = 0;
                                arSakinysBaigesi = true;
                            }
                        }
                    }

                    if (arSakinysBaigesi == false)
                    {
                        jungtinis += dalis;
                        jungimuSk++;
                    }
                }
            }


        }
    }
}
