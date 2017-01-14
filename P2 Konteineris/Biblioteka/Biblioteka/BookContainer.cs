using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteka
{
    class BookContainer
    {
        public Books[] books;
        public int Count { get; private set; }
        private int CMaxBooks = 15;

        public BookContainer()
        {
            books = new Books[CMaxBooks];
        }

        public void Add(Books book)
        {
            books[Count++] = book;
        }

        public Books Take(int i)
        {
            return books[i];
        }

    }
}
