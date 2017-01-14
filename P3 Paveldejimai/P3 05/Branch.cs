using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoMusis
{
    class Branch
    {
        public const int MaxNumberOfKlausimai = 50;

        public string Atstovybe { get; set; }
        public KlausimaiContainer Klausimai { get; private set; }

        public Branch(string atstovybe)
        {
            Atstovybe = atstovybe;
            Klausimai = new KlausimaiContainer(MaxNumberOfKlausimai);
        }
    }
}
