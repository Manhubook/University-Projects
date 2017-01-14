using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Lab4
{
    class Program
    {
        static void Main(string[] args)
        {
            const string Knyga1 = "..\\..\\Knyga1.txt";
            const string Knyga2 = "..\\..\\Knyga2.txt";
            const string Analize = "..\\..\\Analize.txt";
            const string ManoKnyga = "..\\..\\ManoKnyga.txt";
            char[] skyrikliai = {' ', '.', ',', '!', '?', ':', ';', '(', ')', '\t'};


            Apdoroti(Knyga1,Knyga2,Analize,ManoKnyga,skyrikliai);

            Console.ReadKey();
        }

        static int ZodziuSkaicius(string[] duomenys, char[] skyrikliai)
        {
            int skaicius = 0;
            foreach (string eilute in duomenys)
            {
                string[] zodziai = eilute.Split(skyrikliai);
                skaicius += zodziai.Length;
            }
            return skaicius;
        }
        static string[] PavertimasIZodzius(string[] duomenys, char[] skyrikliai)
        {
            string[] IsskaidytaZodziais = new string[ZodziuSkaicius(duomenys, skyrikliai)];
            int count = 0;

            foreach (string eilute in duomenys)
            {
                string[] zodziai = eilute.Split(skyrikliai);
                foreach (string zodis in zodziai)
                {
                    if (zodis != null && !IsskaidytaZodziais.Contains(zodis) && zodis != " " && zodis != "-" && zodis != "")
                    {
                        IsskaidytaZodziais[count++] = zodis.ToLower();
                    }
                }
            }
            return IsskaidytaZodziais;
        }


        static void Apdoroti(string knyga1,string knyga2,string analize, string manoknyga , char[] skyrikliai)
        {
            string[] Knyga1 = File.ReadAllLines(knyga1, Encoding.GetEncoding(1257));
            string[] Knyga2 = File.ReadAllLines(knyga2, Encoding.GetEncoding(1257));
            int count, count2;
            string[] Kartojasi;
            Unikalus(Knyga1,Knyga2,skyrikliai,out count,out count2,out Kartojasi);

            using (var Analize = File.CreateText(analize))
            {
                using (var ManoKnyga = File.CreateText(manoknyga))
                {

                    string[] atrinkta = AtrinktiDesimt(Knyga1, Knyga2, skyrikliai);
                    Analize.WriteLine("Dazniausiai pasikartojantys unikalos zodziai pirmai knygai. (U.Z.K - is viso unikaliu zodziu)");
                    Analize.WriteLine("| Zodis U.Z.K {0}   | Kart  |", count - 1);
                    Analize.WriteLine("+------------------+-------+");
                    foreach (string zodis in atrinkta)                                                                                                   
                    {
                        Analize.WriteLine("| {0,-16} | {1,-5} |",zodis, KiekKartuKartojasiTekste(zodis,Knyga1,skyrikliai));
                    }
                    Analize.WriteLine("+------------------+-------+");

                    string[] KartojasiAbejuose = UnikalusAbiemFailam(Knyga1, Knyga2, skyrikliai);

                    Analize.WriteLine("Dazniausiai pasikartojantys zodziai tarp abieju knygu. (U.Z.K - is viso unikaliu zodziu)");
                    Analize.WriteLine("| Zodis U.Z.K {0}   | Kart  |", count2 - 1);
                    Analize.WriteLine("+------------------+-------+");
                    foreach (string zodis in KartojasiAbejuose)                                                                                                   
                    {
                        Analize.WriteLine("| {0,-16} | {1,-5} |", zodis, (KiekKartuKartojasiTekste(zodis, Knyga1, skyrikliai) + KiekKartuKartojasiTekste(zodis, Knyga2, skyrikliai)));
                    }
                    Analize.WriteLine("+------------------+-------+");


                    string[] Knyga1Zodziai = PavertimasIZodzius(Knyga1, skyrikliai);
                    string[] Knyga2Zodziai = PavertimasIZodzius(Knyga2, skyrikliai);
                    bool knyg1 = false;
                    bool knyg2 = false;
                    int i = 0;
                    int l = 0;

                    while (knyg1 != true || knyg2 != true)
                    {
                        for (;i < Knyga1Zodziai.Length - 1; i++)
                        {
                            if ((Knyga1Zodziai[i] != Knyga2Zodziai[l]) && (Knyga1Zodziai[i] != null))
                            {
                                ManoKnyga.Write(Knyga1Zodziai[i] + " ");
                            }
                            else
                            {
                                if (Knyga1Zodziai[i] == null)
                                    knyg1 = true;

                                i++;
                                ManoKnyga.WriteLine();
                                break;   
                            }
                        }

                        for (;l < Knyga2Zodziai.Length - 1; l++)
                        {
                            if ((Knyga2Zodziai[l] != Knyga1Zodziai[i]) && (Knyga2Zodziai[l] != null))
                            {
                                ManoKnyga.Write(Knyga2Zodziai[l] + " ");
                            }
                            else
                            {
                                if (Knyga2Zodziai[l] == null)
                                    knyg2 = true;

                                l++;
                                ManoKnyga.WriteLine();
                                break;
                            }
                        }
                    }

                }
            }

        }

        //unikalių žodžių, kurie yra tik faile Knyga1.txt ir ne knyga2 Skaiciu , 10zodziu isrikiuotu pagal pasikartojima atspausdinti | failas analize
        static string[] Unikalus(string[] Knyga1, string[] Knyga2,char[] skyrikliai,out int count,out int count2,out string[] Kartojasi)
        {
            string[] Knyga1Zodziai = PavertimasIZodzius(Knyga1,skyrikliai);
            string[] Knyga2Zodziai = PavertimasIZodzius(Knyga2, skyrikliai);
            string[] UnikalusZodziai = new string[Knyga1Zodziai.Length + Knyga2Zodziai.Length];
            Kartojasi = new string[Knyga1Zodziai.Length + Knyga2Zodziai.Length];
            count = 0;
            count2 = 0;

            foreach (string zodis in Knyga1Zodziai)
            {
                if (zodis != null && !Knyga2Zodziai.Contains(zodis))
                {
                    UnikalusZodziai[count++] = zodis;
                }
                else
                {
                    if (zodis != null && Knyga2Zodziai.Contains(zodis))
                    {
                        Kartojasi[count2++] = zodis;
                    }
                }
            }
            return UnikalusZodziai;
        }
        static int KiekKartuKartojasiTekste(string zodis, string[] knyga, char[] skyrikliai)
        {
            int Kartai = 0;
            foreach (string eilute in knyga)
            {
                string[] zodziai = eilute.Split(skyrikliai);
                foreach (string zod in zodziai)
                {
                    if ((zodis != null && zod != null) && (zodis == zod || zodis.ToLower() == zod.ToLower()))
                    {
                        Kartai++;
                    }
                }
            }
            return Kartai;
        }
        static string[] Surusiuoti(string[] masyvas, string objektas, int indeksas)
        {
            string[] rusiuoti = masyvas;
            string laikinas = null;
            string laikinas2 = null;

            laikinas = rusiuoti[indeksas];
            rusiuoti[indeksas] = objektas;

            for (int i = indeksas + 1; i < 10; i++)
            {
                laikinas2 = rusiuoti[i];
                rusiuoti[i] = laikinas;

                laikinas = laikinas2;
                Console.WriteLine(laikinas);

            }

            return rusiuoti;
        }
        static string[] AtrinktiDesimt(string[] Knyga1, string[] Knyga2, char[] skyrikliai)
        {
            int count,count2;
            string[] kartojasi;
            string[] Zodziai = new string[10];
            string[] UnikalusZodziai = Unikalus(Knyga1,Knyga2,skyrikliai,out count,out count2,out kartojasi);

            foreach (string zodis in UnikalusZodziai)
            {
                for (int i = 0; i < 10; i++)
                {
                    if (!Zodziai.Contains(zodis) && KiekKartuKartojasiTekste(zodis, Knyga1, skyrikliai) > KiekKartuKartojasiTekste(Zodziai[i], Knyga1, skyrikliai))
                    {
                        Zodziai = Surusiuoti(Zodziai, zodis, i);
                    }
                }
            }
            return Zodziai;
        }
        // unikalių  žodžių,  kurie  yra  abejuose  failuose,  skaičių  ir  tokių  žodžių  sąrašą  (ne  daugiau  nei  10 žodžių), surikiuotą pagal pasikartojimo skaičių | failas analize
        static string[] UnikalusAbiemFailam(string[] Knyga1, string[] Knyga2, char[] skyrikliai)
        {
            string[] Zodziai = new string[10];
            int count, count2;
            string[] kartojasi;
            Unikalus(Knyga1, Knyga2, skyrikliai, out count, out count2, out kartojasi);

            foreach (string zodis in kartojasi)
            {
                for (int i = 0; i < 10; i++)
                {
                    if (!Zodziai.Contains(zodis) && (KiekKartuKartojasiTekste(zodis, Knyga1, skyrikliai) +  KiekKartuKartojasiTekste(zodis, Knyga2, skyrikliai)) > (KiekKartuKartojasiTekste(Zodziai[i], Knyga1, skyrikliai) +  KiekKartuKartojasiTekste(Zodziai[i], Knyga2, skyrikliai)))
                    {
                        Zodziai = Surusiuoti(Zodziai, zodis, i);
                    }
                }
            }
            return Zodziai;
        }

        //kopijuojamas pirmojo failo tekstas tol, kol sutinkamas pirmasis antrojo failo žodis arba pasiekiama failo pabaiga;
        //kopijuojamas antrojo failo tekstas tol, kol sutinkamas pirmasis nenukopijuotas pirmojo failo žodis arba pasiekiama failo pabaiga
        //kartojama tol, kol pasiekiama abiejų failų pabaiga.
        //failas ManoKnyga.txt


    }
}
