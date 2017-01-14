using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoMusis
{
    /// <summary>
    /// Sukuriama klase "Klausimai", visu klausimu duomenim
    /// </summary>
    
    class Klausimai
    {
        public string Tema { get; set; }
        public double Sudetingumas { get; set; }
        public string Autorius { get; set; }
        public string Tekstas { get; set; }
        public string Atsakymai { get; set; }
        public string Teisingasats { get; set; }
        public double Balai { get; set; }

        public Klausimai()
        {
            Tema = "tema";
            Sudetingumas = 0;
            Autorius = "autorius";
            Tekstas = "tekstas";
            Atsakymai = "atsakymai";
            Teisingasats = "teisingasats";
            Balai = 0;
        }
        public Klausimai(string tema, double sudetingumas, string autorius, string tekstas, string atsakymai, string teisingasats, double balai)
        {
            Tema = tema;
            Sudetingumas = sudetingumas;
            Autorius = autorius;
            Tekstas = tekstas;
            Atsakymai = atsakymai;
            Teisingasats = teisingasats;
            Balai = balai;

        }

        public override string ToString()
        {
            return String.Format("{0, -10} | {1, 4} | {2, -10} | {3, -85} | {4, -30} | {5, 2}", Tema, Sudetingumas, Autorius, Tekstas, Teisingasats, Balai);
        }
    }

    
}
