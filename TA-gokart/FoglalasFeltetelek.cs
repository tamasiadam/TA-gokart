using System;

namespace TA_gokart
{
    internal class FoglalasFeltetelek
    {
        public int minIdotartam = 1;
        public int maxIdotartam = 2;
        public int minVersenyzo = 8;
        public int maxVersenyzo = 20;
        public int Nyitas = 8;
        public int Zaras = 19;
        public DateTime Kezdet { get; set; }
        public int Idotartam { get; set; } = 1;
        public DateTime Veg => Kezdet.AddHours(Idotartam);
    }
}