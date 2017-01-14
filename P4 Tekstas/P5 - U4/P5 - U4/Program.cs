using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace P5___U4
{
    class Program
    {
        public const string CDf = @"..\..\Knyga.txt";
        public const string CRf = @"..\..\Analize.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Programa pradeda darba.");

            if (File.Exists(CDf))  //Tikrina ar failas egzistuoja
            {
                Apdoroti();
            }

            Console.WriteLine("Programa baige darba.");
            Console.ReadLine();
        }

        private static void Apdoroti()
        {
            string eilute = string.Empty;

            string[] zodziaiPradRaide = new string[10]; //Masyvas, kuriame bus saugomi zodziai (1 punktas)
            int zodIndex = 0;                           //Surastu zodziu sk.

            string[] fragmentai = new string[100];      //Randa visus fragmentinius zodzius
            int fragIndex = 0;                          //Randa fragmentiniu zdz. sk.
            string ilgFrag;                             //Ilgiausias fragmentinis zodis

            Console.Write("Iveskite nurodyta raide- ");
            char raide = char.Parse(Console.ReadLine());

            using (StreamWriter fr = File.CreateText(CRf))
            {
                fr.WriteLine("Pradinis tekstas:");
                fr.WriteLine();

                using (StreamReader fd = File.OpenText(CDf))
                {
                    while ((eilute = fd.ReadLine()) != null)
                    {
                        fr.WriteLine(eilute); //Rasomas pradinis tekstas i faila
                        IeskoZodziu(eilute, ref zodziaiPradRaide, ref zodIndex, raide);  //Eisko zodziu, kurie sutampa su vartotojo nurodyta raide
                        IeskoFragmentus(eilute, ref fragmentai, ref fragIndex);          //Eisko visus fragmentinius zodius
                    }

                    IeskoIlgiausioFragmento(fragmentai, fragIndex, out ilgFrag);         //Randa ilgiausia fragmentini zodi

                    fr.WriteLine("------------------------------------------------------------------------------");
                    fr.WriteLine("žodžiai, prasidedantys nurodyta raide, sąrašas");
                    fr.WriteLine();

                    for (int i = 0; i < zodIndex; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(zodziaiPradRaide[i]))
                        {
                            fr.WriteLine(zodziaiPradRaide[i]);
                        }
                    }

                    fr.WriteLine();
                    fr.WriteLine("Ilgiausias fragmentas: {0}", ilgFrag);
                }
            }
        }

        /// <summary>
        /// Iesko zodzius, kuriu pirma raide sutampa su vartojo duotajaja
        /// </summary>
        /// <param name="e"></param>
        /// <param name="r"></param>
        /// <param name="index"></param>
        /// <param name="raide"></param>
        private static void IeskoZodziu(string e, ref string[] r, ref int index, char raide)
        {
            string[] zodziai = Regex.Split(e, @"\s+");   //Eilute skirtoma i zodzius

            foreach (var zodis in zodziai)               //Eina per visus zodzius
            {
                if (!string.IsNullOrWhiteSpace(zodis))
                {
                    if (zodis.First().Equals(raide))     //Jei pirma zodzio raide sutampa su vartojo parasyta raide, zodis bus saugomas i masyva
                    {
                        if (index < 10)
                        {
                            r[index++] = zodis;
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Iesko visus fragmentinius zodius
        /// </summary>
        /// <param name="e"></param>
        /// <param name="fragmentai"></param>
        /// <param name="fragIndex"></param>
        private static void IeskoFragmentus(string e, ref string[] fragmentai, ref int fragIndex)
        {
            string[] zodziai = Regex.Split(e, @"\s+");    //Eilute skirtoma i zodzius

            for (int i = 0; i < zodziai.Length - 1; i++)
            {
                if (!string.IsNullOrWhiteSpace(zodziai[i]))
                {
                    if (zodziai[i].Last().ToString().ToLower().Equals(zodziai[i + 1].First().ToString().ToLower()))  //Tikrina ar pirmo zodzio paskutine r. sutampa su
                                                                                                                     //antro zodzio pirma r.
                    {
                        fragmentai[fragIndex++] = zodziai[i] + " " + zodziai[i + 1]; //Saugomi fragmentai i masyva
                    }
                }
            }
        }

        /// <summary>
        /// Iesko ilgiausio fragmento
        /// </summary>
        /// <param name="fragmentai"></param>
        /// <param name="fragIndex"></param>
        /// <param name="ilgFrag"></param>
        private static void IeskoIlgiausioFragmento(string[] fragmentai, int fragIndex, out string ilgFrag)
        {
            int laikIlg = fragmentai[0].Length;  //Laikinas fragmento ilgis
            ilgFrag = string.Empty;              //Ilgiausias fragmentas

            for (int i = 0; i < fragIndex; i++)
            {
                if (fragmentai[i].Length > laikIlg)
                {
                    laikIlg = fragmentai[i].Length;
                    ilgFrag = fragmentai[i];
                }
            }
        }

    }
}
