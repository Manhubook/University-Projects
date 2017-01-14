using System;
using System.Collections.Generic;
using System.IO;

namespace Biblioteka
{
    class Program
    {
        private static readonly string CDf = @"..\..\Duomenys.txt";
        private static readonly int CMaxBooks = 15;
        static void Main(string[] args)
        {
            List<Books> books = new List<Books>();
            ReadData(CDf, ref books);

            string name = "", author = "";
            int longestBook = FindLongestBook(books, ref name, ref author);
            Console.WriteLine("The longest book is {0} with {1} pages and it's author is {2}", name, longestBook, author);

            int indexOf2014 = 0;
            int[] books2014 = Find2014Books(books, out indexOf2014);
            if (books2014 != null)
            {
                for (int i = 0; i < indexOf2014; i++)
                {
                    books[books2014[i]].Print();
                }
            }

            int indexOfTech = 0;
            int[] booksTech = FindTechBooks(books, out indexOfTech);
            if (books2014 != null)
            {
                for (int i = 0; i < indexOfTech; i++)
                {
                    books[booksTech[i]].Print();
                }
            }


            Console.ReadKey();
        }

        private static void ReadData(string fv, ref List<Books> book)
        {
            string line = string.Empty;
            string[] parts;

            using (StreamReader fd = File.OpenText(fv))
            {
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
                    book.Add(newBook);
                }
            }
        }

        private static int FindLongestBook(List<Books> book, ref string name, ref string author)
        {
            int max = 0;

            for (int i = 0; i < book.Count; i++)
            {
                if (max < book[i].pages)
                {
                    max = book[i].pages;
                    name = book[i].name;
                    author = book[i].author;
                }
            }

            return max;
        }

        private static int[] Find2014Books(List<Books> book, out int index)
        {
            int[] bookIndex = new int[5];
            index = 0;
            DateTime year = new DateTime(2014, 01, 01);

            for (int i = 0; i < book.Count; i++)
                if (book[i].realeseDate.Year == year.Year)
                    bookIndex[index++] = i;
            if (index == 0)
            {
                bookIndex = null;
            }

            return bookIndex;
        }

        private static int[] FindTechBooks(List<Books> book, out int index)
        {
            int[] bookIndex = new int[CMaxBooks];
            index = 0;

            for (int i = 0; i < book.Count; i++)
                if (book[i].type.ToLower() == "technologija")
                    bookIndex[index++] = i;
            if (index == 0)
            {
                bookIndex = null;
            }

            return bookIndex;
        }

    }
}

