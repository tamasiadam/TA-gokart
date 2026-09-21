using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TA_gokart
{
    internal class FoglalasFeltetelek
    {
        public int minVersenyzo = 8;
        public int maxVersenyzo = 20;

        public DateTime Kezdet { get; }
        public DateTime Veg => Kezdet.AddHours(1);

        

    }
}
