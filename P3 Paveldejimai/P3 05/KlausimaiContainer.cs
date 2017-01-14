using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoMusis
{
    class KlausimaiContainer
    {
        private Klausimai[] Klausimas { get; set; }
        public int Count { get; private set; }

        public KlausimaiContainer(int size)
        {
            Klausimas = new Klausimai[size];
        }

        public void AddKlausimai(Klausimai klausimai)
        {
            Klausimas[Count] = klausimai;
            Count++;
        }

        public Klausimai GetKlausimai(int index)
        {
            return Klausimas[index];
        }

        public bool Contains(Klausimai klausimai)
        {
            return Klausimas.Contains(klausimai);
        }
    }
}
