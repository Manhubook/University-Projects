using System;

namespace Biblioteka
{
    class Books
    {
        public string ISBN { get; private set; }
        public string name { get; private set; }
        public string author { get; private set; }
        public string type { get; private set; }
        public string press { get; private set; }
        public DateTime realeseDate { get; private set; }
        public int pages { get; private set; }

        public Books() { }

        public Books(string ISBN, string name, string author, string type, string press, DateTime realeseDate, int pages)
        {
            this.ISBN = ISBN;
            this.name = name;
            this.author = author;
            this.type = type;
            this.press = press;
            this.realeseDate = realeseDate;
            this.pages = pages;
        }

        public override string ToString()
        {
            return String.Format("{0} {1} {2} {3} {4} {5} {6}", ISBN, name, author, type, press, realeseDate.ToShortDateString(), pages);
        }

        public static bool operator >(Books b1 , Books b2)
        {
            return b1.pages > b2.pages;
        }

        public static bool operator <(Books b1, Books b2)
        {
            return b1.pages < b2.pages;
        }

        public static bool operator ==(Books b1, Books b2)
        {
            b2.type = "technologija";
            return b1.type == b2.type;
        }

        public static bool operator !=(Books b1, Books b2)
        {
            b2.type = "technologija";
            return b1.type != b2.type;
        }
    }
}
