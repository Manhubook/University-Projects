using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteka
{
    class Libary
    {
        public BookContainer container { get; private set; }
        public string LibaryName, Address, number;

        public Libary(string LibaryName, string Address, string number)
        {
            this.LibaryName = LibaryName;
            this.Address = Address;
            this.number = number;
            container = new BookContainer();
        }
    }
}
