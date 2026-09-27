using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TA_gokart
{
    internal class Helyszin
    {
        public string palyanev = "Future Gokart";
        public string palyacim = "6600 Szentes, Csongrádi út 5.";
        public string palyatelefon = "+36 30 123 4567";
        public string palyaweb = "www.futuregokart.hu";

        public void GoKartKiirat()
        {
            Console.WriteLine($"Üdvözlünk a {palyanev} GoKart pályán!\nCím: {palyacim}\nTel.: {palyatelefon}\nWeboldal: {palyaweb}");
        }
    }
}
