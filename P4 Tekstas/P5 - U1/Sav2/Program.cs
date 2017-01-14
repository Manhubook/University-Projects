using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Sav2
{
    class Program
    {
        static void Main(string[] args)
        {
            const string Failas = "..\\..\\duomenys.txt";
            const string Rezultatas = "..\\..\\rezultatas.txt";
            const string Analize = "..\\..\\analize.txt";

            Nuskaitymas(Failas,Rezultatas,Analize);
            Console.ReadKey();
        }

        //Pagr programos skaiciavimai
        static void Nuskaitymas(string failas , string rezultatas , string analize)
        {
            string[] linijos = File.ReadAllLines(failas, Encoding.GetEncoding(1257));
            using (var kurti = File.CreateText(rezultatas))
            {
                using (var kurtianalize = File.CreateText(analize))
                {
                    for (int i = 0; i < linijos.Length; i++)
                    {
                        if (!ArSkliaustai(linijos[i]))
                        {
                            if (linijos[i].Length > 0)
                            {
                                string nauja = linijos[i];
                                if (Komentarai(linijos[i], out nauja))
                                    kurtianalize.WriteLine(linijos[i]);
                                if (nauja.Length > 0)
                                    kurti.WriteLine(nauja);
                            }
                            else
                                kurti.WriteLine(linijos[i]);
                        }
                        else
                        {
                            //string nauja;
                            //kurtianalize.WriteLine(linijos[i]);
                            //int tikrinamas = SuSkliaustais(linijos[i], linijos);
                            //for (int l = 0; l < tikrinamas; l++)
                            //{
                            //    i++;
                            //    nauja = linijos[i];
                            //    kurtianalize.WriteLine(linijos[i]);
                            //}

                            int pradzia = DuomensEile(linijos[i], linijos);
                            int pabaiga = ArSkliaustuGalas(linijos[i], linijos);
                            string nauja;

                            if (pradzia == pabaiga)
                            {
                                kurtianalize.WriteLine(linijos[i]);
                                nauja = Trinimas(linijos[i]);
                                if (nauja != null)
                                {
                                    kurti.WriteLine(nauja);
                                }
                            }
                            else
                            {
                                kurtianalize.WriteLine(linijos[i]);
                                nauja = Trinimas(linijos[i]);
                                if (nauja != null)
                                {
                                    kurti.WriteLine(nauja);
                                }
                                for (int l = pradzia; l < pabaiga; l++)
                                {
                                    i++;
                                    kurtianalize.WriteLine(linijos[i]);
                                    nauja = Trinimas(linijos[i]);
                                    if (nauja != null)
                                    {
                                        kurti.WriteLine(nauja);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        //patikrinti ar skliaustai toje eiluteje prasideda
        static bool ArSkliaustai(string linija)
        {
            for (int i = 0; i < linija.Length - 1; i++)
            {
                if ((linija[i] == '/' && linija[i + 1] == '*'))
                {
                    return true;
                }
            }
            return false;
        }

        //nutrina su // esancias eilutes (very easy)
        static bool Komentarai(string linija, out string nauja)
        {
            nauja = linija;
            for (int i = 0; i < linija.Length - 1; i++)
            {
                if ((linija[i] == '/' && linija[i + 1] == '/'))
                {
                    nauja = linija.Remove(i);
                    return true;
                }
            }

            return false;
        }

        //nuskaito duomens eile (paciam eiluciu masyvui)
        static int DuomensEile(string linija, string[] linijos)
        {
            int count = 0;
            foreach (string lin in linijos)
            {
                if (lin == linija)
                {
                    break;
                }
                else
                {
                    count++;
                }
            }
            return count;
        }

        //static int SuSkliaustais(string linija,string[] linijos)
        //{
        //    int x = 0;
        //    for (int i = DuomensEile(linija,linijos); i < linijos.Length; i++)
        //    {
        //        string lin = linijos[i];
        //        for (int l = 0; l < lin.Length - 1; l++)
        //        {
        //            if (lin[l] == '*' && lin[l + 1] == '/')
        //            {
        //                return x = DuomensEile(linijos[i], linijos) - DuomensEile(linija, linijos);
        //            }
        //        }
        //    }
        //    return x;
        //}

        //patikrina ar skliaustu galas egzistuoja toje eiluteje (duome eile + sitas algoritmas = pradzia ir pabaiga tarp /* */ skliaustu)
        static int ArSkliaustuGalas(string linija,string[] linijos)
        {
            for (int i = DuomensEile(linija,linijos); i < linijos.Length; i++)
            {
                string lin = linijos[i];
                for (int l = 0; l < lin.Length - 1; l++)
                {
                    if(lin[l] == '*' && lin[l + 1] == '/' )
                    {
                        return DuomensEile(linijos[i], linijos);
                    }
                }
            }
            return -1;
        }

        //Apdorojimo algoritmas
        static string Trinimas(string linija)
        {
            for (int i = 0; i < linija.Length - 1; i++)
            {
                if (linija[i] == '/' && linija[i + 1] == '*')
                {
                    for (int l = i + 2; l < (linija.Length - 1); l++)
                    {
                        if (linija[l] == '*' && linija[l + 1] == '/')
                        {
                            string naujaLinija = linija.Remove(i, ((l + 2) - i));
                            return naujaLinija;
                        }
                        else if (l == linija.Length - 1)
                        {
                            string naujaLinija2 = linija.Remove(i - 1);
                            return naujaLinija2;
                        }
                    }
                }
                else if (linija[i] == '*' && linija[i + 1] == '/')
                {
                    string naujaLinija3 = linija.Remove(0 , i+2);
                    return naujaLinija3;
                }
            }
            return null;
        }
    }
}
