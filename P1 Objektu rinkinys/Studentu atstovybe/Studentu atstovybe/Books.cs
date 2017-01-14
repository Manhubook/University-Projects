using System;

namespace Studentu_atstovybe
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
    }
}
