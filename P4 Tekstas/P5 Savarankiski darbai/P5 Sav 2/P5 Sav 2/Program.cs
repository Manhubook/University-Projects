using System;
using System.IO;
using System.Text;

namespace P5_Sav_2
{
    class Program
    {
        public const string CDf = @"..\..\Knyga.txt";
        public const string CRf = @"..\..\Analize.txt";

        static void Main(string[] args)
        {
            if (File.Exists(CDf))
            {
                Apdoroti();
            }
        }

        static void Apdoroti()
        {
            var eilute = string.Empty;
            var tekstas = new StringBuilder();

            Console.Write("Iveskite zodi: ");
            string zodis = Console.ReadLine();

            using (var fr = File.CreateText(CRf))
            {
                fr.WriteLine("Pradinis tekstas");
                fr.WriteLine();

                using (StreamReader fd = File.OpenText(CDf))
                {
                    while ((eilute = fd.ReadLine()) != null)
                    {
                        if (eilute.Length > 0)
                        {
                            fr.WriteLine(eilute);
                            NaikintiZodi(ref eilute, zodis);
                            tekstas.AppendLine(eilute);
                        }
                    }

                    fr.WriteLine("Baigtas tekstas: ");
                    fr.WriteLine();
                    fr.WriteLine(tekstas);
                }
            }
        }

        static void NaikintiZodi(ref string e, string zodis)
        {
            char[] skyrikliai = { '.', '?', '!', ',' };
            bool panaikino = false;

            if (e.IndexOf(zodis) != -1) //Ar zodis egzistuoja
            {
                foreach (var sk in skyrikliai)
                {
                    if (e[e.IndexOf(zodis) + zodis.Length] == sk) //Ar po zodzio yra skyriklis
                    {
                        e = e.Remove(e.IndexOf(zodis), zodis.Length + 1);
                        panaikino = true;
                    }
                }

                if (panaikino == false) //Ar zodis buvo panaikintas
                {
                    e = e.Remove(e.IndexOf(zodis), zodis.Length);
                    panaikino = true;
                }
            }

        }

    }
}
