using System;
using System.IO;

namespace Biblioteka
{
    class Program
    {
        private static readonly string CDf = @"Duomenys";


        static void Main(string[] args)
        {
            string[] Files = GetFiles(CDf);
            var _libary = new Libary[Files.Length];
            int Count = 0;
            for (int i = 0; i < Files.Length; i++)
                ReadData(Files[i], ref _libary[i], ref Count);

            Print(_libary);

            Books[] maxPages = FindLongestBook(_libary, Files.Length);
            for (int i = 0; i < maxPages.Length; i++)
            {
                Console.WriteLine(maxPages[i]);
            }

            Books[] TechBooks = FindTechBooks(_libary, Files.Length);
            for (int i = 0; i < TechBooks.Length; i++)
            {
                if (TechBooks[i] != null)
                {
                    Console.WriteLine(TechBooks[i].type);

                }
            }

            int[] countSpecial;
            int[] onlyOne = FindSpecialBook(_libary, Files.Length, out countSpecial);

            Console.ReadKey();
        }

        private static int[] FindSpecialBook(Libary[] _libary, int lengt, out int[] countSpecial)
        {
            int[] index = new int[100];
            countSpecial = new int[lengt];
            for (int i = 0; i < lengt; i++)
            {
                for (int j = 0; j < _libary[i].container.Count; j++)
                {

                }
            }

            return index;
        }

        private static void Print(Libary[] libary)
        {
            for (int i = 0; i < libary.Length; i++)
            {
                for (int j = 0; j < libary[i].container.Count; j++)
                    Console.WriteLine(libary[i].container.Take(j).ToString());

                Console.WriteLine();
            }
        }

        private static string[] GetFiles(string fv)
        {
            string[] Files = Directory.GetFiles(fv);
            return Files;
        }

        private static void ReadData(string fv, ref Libary libary, ref int Count)
        {
            string line = string.Empty;
            string[] parts;

            using (StreamReader fd = File.OpenText(fv))
            {
                line = fd.ReadLine();
                string libaryName = line;

                line = fd.ReadLine();
                string adress = line;

                line = fd.ReadLine();
                string number = line;
                Libary _libary = new Libary(libaryName, adress, number);
                libary = _libary;
                while ((line = fd.ReadLine()) != null)
                {
                    line.Trim();
                    parts = line.Split(',');
                    string ISBN = parts[0].Trim();
                    string name = parts[1].Trim();
                    string author = parts[2].Trim();
                    string type = parts[3].Trim();
                    string press = parts[4].Trim();
                    DateTime realeseDate = DateTime.Parse(parts[5].Trim());
                    int pages = int.Parse(parts[6].Trim());
                    Books newBook = new Books(ISBN, name, author, type, press, realeseDate, pages);
                    libary.container.Add(newBook);
                    Count++;
                }
            }
        }

        private static Books[] FindLongestBook(Libary[] book, int number)
        {
            Books[] longestBooks = new Books[number];
            Books max = new Books();
            for (int i = 0; i < number; i++)
            {
                for (int j = 0; j < book[i].container.Count; j++)
                {
                    if (max < book[i].container.books[j])
                    {
                        max = book[i].container.books[j];
                        longestBooks[i] = max;
                    }
                }
            }
            return longestBooks;
        }

        private static Books[] FindTechBooks(Libary[] book, int number)
        {
            Books techBook = new Books();
            Books[] techBooks = new Books[100];
            int ind = 0;

            for (int i = 0; i < number; i++)
            {
                for (int j = 0; j < book[i].container.Count; j++)
                {
                    if (book[i].container.books[j] == techBook)
                    {
                        techBooks[ind++] = techBook;
                    }
                }
            }
           
            return techBooks;
        }

    }
}

