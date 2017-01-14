using System.IO;
using System.Text;
using System;

namespace Sav
{
    class Program
    {
        static void Main(string[] args)
        {
            const string CFd = @"..\..\Duomenys.txt";
            const string CFr = @"..\..\Rezultatai.txt";
            const string CFa = @"..\..\Analize.txt";

            if (File.Exists(CFr))
                File.Delete(CFr);
            if (File.Exists(CFa))
                File.Delete(CFa);

            Tikrinimas(CFd, CFr, CFa);
            Console.ReadKey();
        }

        /// <summary>
        /// failu nuskaitymas, suskirstymas i eilutes, ir tikrinimaas pagal BeKomentaru metoda.
        /// </summary>
        /// <param name="fvs"></param>
        /// <param name="fvr"></param>
        /// <param name="fa"></param>
        static void Tikrinimas(string fvs, string fvr, string fa)
        {
            bool vienaeilute = true;
            string[] lines = File.ReadAllLines(fvs, Encoding.GetEncoding(1257));
            using (var fr = File.CreateText(fvr))
            {
                using (var far = File.CreateText(fa))
                {
                    foreach (string line in lines)
                    {
                        if (line.Length > 0)
                        {
                            string nauja = line;
                            if (BeKomentaru(line, out nauja, ref vienaeilute))
                                far.WriteLine(line);
                            if (nauja.Length > 0)
                                fr.WriteLine(nauja);
                        }
                        else
                            fr.WriteLine(line);
                    }
                }
            }
        }

        /// <summary>
        /// tikrina eilutes, iesko komentaru // istrina teksta uz ju, taip pat istrina visa teksta kuris yra tarp /* ir */
        /// </summary>
        /// <param name="line"></param>
        /// <param name="nauja"></param>
        /// <param name="vienEilute"></param>
        /// <returns></returns>
        static bool BeKomentaru(string line, out string nauja, ref bool vienEilute)
        {
            string komentaras1 = "//";
            string komentaras2 = "*/";
            nauja = line;
            int laikinas = 0;
            for (int i = 0; i < line.Length - 1; i++)
            {
                if (line[i] == '/' && line[i + 1] == '/')
                {
                    nauja = line.Remove(i);
                    vienEilute = true;
                    return true;
                }
                else if (line[i] == '/' && line[i + 1] == '*')
                {
                    vienEilute = false;
                    laikinas = i;
                }
                if (line[i] == '*' && line[i + 1] == '/')
                {
                    nauja = line.Remove(laikinas, i - laikinas + 2);
                    vienEilute = true;
                    return true;
                }
                if (line[i] == '/' && line[i + 1] == '*' && line.Contains(komentaras2) && line.Contains(komentaras1))
                {
                    nauja = line.Remove(laikinas , line.IndexOf(komentaras1) - laikinas + 2);
                    vienEilute = true;
                    return true;
                }
            }
            if (vienEilute == false)
            {
                nauja = line.Remove(0);
            }
            return false;
        }

    }
}
