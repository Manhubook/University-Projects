using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
namespace L5
{
    class Program
    {

        static void Main(string[] args)
        {
            const string CFd = "..\\..\\Knyga.txt";
            const string CFa = "..\\..\\Analize.txt";
            const string CFr = "..\\..\\ManoKnyga.txt";
            char[] skyrikliai = new char[4] {'.','!','?',';'};
            System.IO.File.WriteAllText(CFr , string.Empty); //ištrinamas rezultatų failas, kadangi naudojama append
            Apdoroti(CFd, CFr, CFa, skyrikliai);



            Console.ReadLine(); 

        }



        static void Apdoroti(string Fd, string Fr, string Fa, char[] skyrikliai)
        {
            
            int skaiciuKiekis = 0;
            int skaiciuSuma = 0;


            int eil = 0;
            string ilgiausiasSakinys ="";                      // ilgiausias sakinys
            string jungtinis = "";                             // sujungtas sakinys iš kelių eilučių
            int eilutesNr = 1;                                 // eilutės, kurioje ilgiausias sakinys, numeris
            int simboliuSk = 0;                                // simbolių skaičius
            int jungimai = 0;                                  // kiek kartų buvo jungtas sakinys 
            int zodziuSk = 0;


            using (StreamReader reader = new StreamReader(Fd))
            {
                string line;
                while (null != (line = reader.ReadLine()))
                {
                    eil++;
                    Lygiuoti(line, Fr);
                    AnalizuotiSkaicius(line, ref skaiciuKiekis, ref skaiciuSuma);
                    RastiIlgiausiaSakini(skyrikliai, line, eil, ref ilgiausiasSakinys, ref jungtinis, ref eilutesNr, ref simboliuSk, ref jungimai);
                }
                Console.WriteLine(ilgiausiasSakinys);
            }
            zodziuSk = SkaiciuotiZodzius(ilgiausiasSakinys);
            SpausdintiAnalize(Fa, skaiciuKiekis, skaiciuSuma, simboliuSk, ilgiausiasSakinys, eilutesNr, zodziuSk);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"> eilutė </param>
        /// <param name="skaiciuKiekis"> skaičių tekste kiekis </param>
        /// <param name="skaiciuSuma"> skaičių tekse suma </param>
        static void AnalizuotiSkaicius(string line, ref int skaiciuKiekis, ref int skaiciuSuma)
        {
            int skaicius;
            int suma = 0;
            string[] skaiciai = Regex.Matches(line, @"(?<!\S)\d+(?!\S)").
                Cast<Match>()
                .Select(m => m.Value)
                .ToArray();
            skaiciuKiekis += skaiciai.Length;
            foreach(string skaiciusString in skaiciai)
            {
                skaicius = int.Parse(skaiciusString);
                suma+= skaicius;
            }
            skaiciuSuma += suma;
           

 
        }


        /// <summary>
        /// metodas rasti ilgiausiam sakiniui
        /// </summary>
        /// <param name="skyrikliai"> skyriklių masyvas </param>
        /// <param name="line"> eilutė </param>
        /// <param name="eil"> esamosios eilutės nr </param>
        /// <param name="ilgiausiasSakinys"> ilgiausias sakinys </param>
        /// <param name="jungtinis"> jungtinis sakinys iš kelių eilučių </param>
        /// <param name="eilutesNr"> ilgiausio sakinio eilutės numeris </param>
        /// <param name="simboliuSk"> ilgiausio sakinio simbolių skaičius </param>
        /// <param name="jungimuSk"> kiek kartų buvo jungtas sakinys </param>
        static void RastiIlgiausiaSakini(char[] skyrikliai, string line, int eil, ref string ilgiausiasSakinys,
            ref string jungtinis, ref int eilutesNr, ref int simboliuSk, ref int jungimuSk)
        {
            bool isSentenceComplete = false;
            var Duomenys = Regex.Split(line, @"(?<=[.?!])");
            foreach (string dalis in Duomenys)
            {
                if (dalis != "")                                   // išvengiama pilno sakinio eilutėje klaidos, kai atskiriamas sakinys ir tuščia vieta
                {
                    string sakinys = jungtinis + dalis;
                    foreach (char skyriklis in skyrikliai)         // einama per skyriklius
                    {
                        if (dalis[dalis.Length - 1] == skyriklis)  //tikrinama ar dalis eilutės užsibaigia 
                        {
                            if (sakinys.Length > simboliuSk) 
                            {
                                ilgiausiasSakinys = sakinys;
                                simboliuSk = sakinys.Length;
                                eilutesNr = eil - jungimuSk;
                                jungtinis = "";
                                jungimuSk = 0;
                                isSentenceComplete = true;
                            }
                            else if (sakinys.Length < simboliuSk)
                            {
                                jungtinis = "";
                                jungimuSk = 0;
                                isSentenceComplete = true;
                            }
                        }
                            
                    }



                    if (isSentenceComplete == false) /* jeigu sakinys neužsibaigia, jis yra įrašomas į kintamąjį
                         kuris vėliau su jungiamas su kitos eilutės dalimi */ 
                    {
                        jungtinis += dalis;
                        jungimuSk++;
                    }
                }
            }
        }

        public static int SkaiciuotiZodzius(string ilgiausiasSakinys)
        {
            int zodziuSk = 0;
            string[] zodziai = Regex.Split(ilgiausiasSakinys, @"\s+");

            foreach(string zodis in zodziai)
            {
                if (zodis != "-")
                zodziuSk++;
            }

            return zodziuSk;
        }

        /// <summary>
        /// spausdinamas analizės failas
        /// </summary>
        /// <param name="fa"> analizės failas </param>
        /// <param name="skaiciuKiekis"> skaičių kiekis </param>
        /// <param name="skaiciuSuma"> skaičių suma </param>
        /// <param name="simboliuSk"> simbolių skaičius ilgiausiame sakinyje </param>
        /// <param name="ilgiausiasSakinys"> ilgiausias sakinys </param>
        /// <param name="eilutesNr"> ilgiausio sakinio eilutės numeris </param>
        public static void SpausdintiAnalize(string fa, int skaiciuKiekis, int skaiciuSuma, int simboliuSk,
            string ilgiausiasSakinys, int eilutesNr, int zodziuSk)
        {
            using (StreamWriter writer = new StreamWriter(fa))
            {
                writer.WriteLine("Ilgiausias sakinys tekste : {0}", ilgiausiasSakinys);
                writer.WriteLine("Ilgiausia sakinį sudaro : {0} vietos", simboliuSk);
                writer.WriteLine("Ilgiausia sakinį sudaro : {0} žodžiai", zodziuSk);
                writer.WriteLine("Ilgiausias sakinys prasideda : {0} eilutėje", eilutesNr);
                writer.WriteLine("Skaičių kiekis tekste : {0} ", skaiciuKiekis);
                writer.WriteLine("Skaičių suma tekste : {0} ", skaiciuSuma);
            }
        }

        /// <summary>
        /// lygiuojami žodžiai kas nustatytą simbolių kiekį
        /// </summary>
        /// <param name="line"> eilutė </param>
        /// <param name="Fr"> rezultatų failas </param>
        static void Lygiuoti(string line, string Fr)
        {
            string nauja = "";
            string[] zodziai = Regex.Split(line, @"\s+");
            foreach (string zodis in zodziai)
            {
                if (zodis != "-")
                {


                    nauja += String.Format("{0, -15}", zodis);

                }
            }
            Spausdinti(Fr, nauja);
        }

        /// <summary>
        /// spausdinamas rezultatų failas
        /// </summary>
        /// <param name="Fr"> rezultatų failas </param>
        /// <param name="lygiuota"> lygiuotos eilutės </param>
        static void Spausdinti(string Fr, string lygiuota)
        {

            using (StreamWriter writer = new StreamWriter(Fr, true))
            {

                writer.WriteLine(lygiuota);

            }

        }
            
    }
}

