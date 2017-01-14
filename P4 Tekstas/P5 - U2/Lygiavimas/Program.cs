using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Lygiavimas
{
    class Program
    {
        const string CFd = @"..\..\Knyga.txt";
        const string CFa = @"..\..\ManoKnyga.txt";
        const string CFr = @"..\..\Analize.txt";
        readonly static char[] vowelList = { 'a', 'e', 'i', 'o', 'u' }; //Balses
        private static int maxVowelWords = 10;                          //Daugiausia zodziu su balsem
        private static int maxLongestWords = 10;                        //Daugiausia zodziu, kurie yra ilgiausi

        static void Main(string[] args)
        {
            if (File.Exists(CFd))
                ReadData();
        }

        static void ReadData()
        {
            string line = string.Empty;
            string allText = string.Empty;

            int vwCount = 0;                                 //Zodziai su balsem kiekis
            int longestWordsCount = 0;                       //Ilgiausiu zodziu kiekis

            string[] vowelWords = new string[maxVowelWords];        //Zodziai su balsem
            int[] vowWordsCount = new int[10];
            int x = 0;


            string[] longestWords = new string[maxLongestWords];     //Ilgiausi zodziai
            int[] lonWordsCount = new int[10];
            int p = 0;

            string[] trumpiausi = new string[10];
            int trumpCount = 0;
            int s = 0;
            int[] WordsCount = new int[10];

            using (StreamWriter fr = File.CreateText(CFr))
            {
                fr.WriteLine("Pradiniai duomenys: \r\n");
                using (StreamReader fd = File.OpenText(CFd))
                {
                    while ((line = fd.ReadLine()) != null)
                    {
                        fr.WriteLine(line);   //Raso prad. duom.
                        CleanLine(line);      //Sutvarko eilute
                        allText += line;      //Pridedama eilute prie teksto
                        FindVowelWords(line, ref vwCount, ref vowelWords); //Zodziai kurie prasideda ir pasibaige balse
                    }

                    FindLongestWords(allText, ref longestWordsCount, ref longestWords);                      //Randa ilgiausius zodzius
                    FindWords(allText, ref trumpCount, ref trumpiausi);

                    vowelWords = vowelWords.Distinct().ToArray();     //Sunaikina pasikartojancius zod.
                    longestWords = longestWords.Distinct().ToArray();
                    trumpiausi = trumpiausi.Distinct().ToArray();

                    foreach (var vw in vowelWords)
                        if (!string.IsNullOrWhiteSpace(vw))
                            vowWordsCount[x++] = CountStringOccurrences(allText, vw); //Skaiciuoja pasikartojancius zod.

                    foreach (var lw in longestWords)
                        if (!string.IsNullOrWhiteSpace(lw))
                            lonWordsCount[p++] = CountStringOccurrences(allText, lw);

                    foreach (var tr in trumpiausi)
                        if (!string.IsNullOrWhiteSpace(tr))
                            WordsCount[s++] = CountStringOccurrences(allText, tr);
                }

                fr.WriteLine();
                fr.WriteLine("Zodžiai, kurie prasideda ir pasibaigia balse: \r\n");
                fr.WriteLine("Zodziu kiekis: {0} \r\n", vowelWords.Count());
                for (int i = 0; i < x; i++)
                    fr.WriteLine("{0}. {1, -18} {2, 2}", i + 1, vowelWords[i], vowWordsCount[i]);

                fr.WriteLine();
                fr.WriteLine("Ilgiausiu zodziu sarasas: \r\n");
                fr.WriteLine("Ilgiausiu zodziu kiekis: {0} \r\n", longestWordsCount);
                for (int i = 0; i < p; i++)
                    fr.WriteLine("{0}. {1, -15} {2}", i + 1, longestWords[i], lonWordsCount[i]);

                fr.WriteLine();
                fr.WriteLine("Trumpiausiu zodziu sarasas: \r\n");
                fr.WriteLine("Trumpiausiu zodziu kiekis: {0} \r\n", trumpCount);
                for (int i = 0; i < p; i++)
                    fr.WriteLine("{0}. {1, -15} {2}", i + 1, trumpiausi[i], WordsCount[i]);

            }
        }

        /// <summary>
        /// Panaikina nereikalingus tarpus
        /// </summary>
        /// <param name="line"></param>
        static void CleanLine(string line)
        {
            line = Regex.Replace(line, @"\s+", " ");
            line = Regex.Replace(line, @"(?<=[,.?!)])", " ");
        }

        /// <summary>
        /// Randa zodzius, kurie prasideda ir baigiasi balse
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="vowelWordsCount"></param>
        /// <param name="vowelWords"></param>
        private static void FindVowelWords(string lines, ref int vowelWordsCount, ref string[] vowelWords)
        {
            string[] words = Regex.Split(lines, @"\W");
            foreach (var word in words)
                if (!string.IsNullOrWhiteSpace(word))
                    foreach (var vowel1 in vowelList)
                        foreach (var vowel2 in vowelList)
                            if (Convert.ToChar(word.First().ToString().ToLower()) == vowel1 && Convert.ToChar(word.Last().ToString().ToLower()) == vowel2)
                                vowelWords[vowelWordsCount++] = word;
        }

        /// <summary>
        /// Randa ilgiausius zodzius tekste
        /// </summary>
        /// <param name="allText"></param>
        /// <param name="longestWordsCount"></param>
        /// <param name="longestWords"></param>
        private static void FindLongestWords(string allText, ref int longestWordsCount, ref string[] longestWords)
        {
            string[] words = Regex.Split(allText, @"\W");
            words = words.OrderBy(aux => aux.Length).ToArray();
            Array.Reverse(words);

            foreach (var word in words)
                if (!string.IsNullOrWhiteSpace(word) && longestWordsCount < 10)
                    longestWords[longestWordsCount++] = word;
        }


        private static void FindWords(string allText, ref int WordsCount, ref string[] Words)
        {
            string[] words = Regex.Split(allText, @"\W");
            words = words.OrderBy(aux => aux.Length).ToArray();

            foreach (var word in words)
                if (!string.IsNullOrWhiteSpace(word) && WordsCount < 10 && word.Length > 3)
                    Words[WordsCount++] = word;
        }

        /// <summary>
        /// Skaiciuoja pasikartojancius zod. masyve.
        /// </summary>
        public static int CountStringOccurrences(string text, string pattern)
        {
            int count = 0;
            int i = 0;
            while ((i = text.IndexOf(pattern, i)) != -1)
            {
                i += pattern.Length;
                count++;
            }
            return count;
        }

        private static void FindLongestWordInLine(string line, int[] ilgiausi)
        {
            string[] words = Regex.Split(line, @"\W");
            int z = 0;
            foreach (var word in words)
            {
                if (ilgiausi[z] < word.Length)
                {
                    ilgiausi[z] = word.Length;
                    z += 1;
                }
            }
        }
    }
}
