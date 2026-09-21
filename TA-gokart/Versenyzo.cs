using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TA_gokart
{
    public class Versenyzo
    {
        public string Vezeteknev { get; set; }
        public string Keresztnev { get; set; }
        public DateTime Szulido { get; set; }
        public bool Nagykoru => Szulido <= DateTime.Now.AddYears(-18);
        public string Azonosito => AzonositoGen();
        public string Email => EmailGen();

        public Versenyzo(string vezeteknev, string keresztnev, DateTime szulido)
        {
            Vezeteknev = vezeteknev;
            Keresztnev = keresztnev;
            Szulido = szulido;
        }

        public string AzonositoGen()
        {
            string azonositodatum = Szulido.ToString("yyyyMMdd");
            string vezetek = Vezeteknev
                .Replace("á", "a")
                .Replace("é", "e")
                .Replace("í", "i")
                .Replace("ó", "o")
                .Replace("ö", "o")
                .Replace("ő", "o")
                .Replace("ú", "u")
                .Replace("ü", "u")
                .Replace("ű", "u");

            string kereszt = Keresztnev
                .Replace("á", "a")
                .Replace("é", "e")
                .Replace("í", "i")
                .Replace("ó", "o")
                .Replace("ö", "o")
                .Replace("ő", "o")
                .Replace("ú", "u")
                .Replace("ü", "u")
                .Replace("ű", "u");
            return $"GO-{vezetek}{kereszt}-{azonositodatum}";
        }

        public string EmailGen()
        {
            string vezetek = Vezeteknev.ToLower()
                .Replace("á", "a")
                .Replace("é", "e")
                .Replace("í", "i")
                .Replace("ó", "o")
                .Replace("ö", "o")
                .Replace("ő", "o")
                .Replace("ú", "u")
                .Replace("ü", "u")
                .Replace("ű", "u");

            string kereszt = Keresztnev.ToLower()
                .Replace("á", "a")
                .Replace("é", "e")
                .Replace("í", "i")
                .Replace("ó", "o")
                .Replace("ö", "o")
                .Replace("ő", "o")
                .Replace("ú", "u")
                .Replace("ü", "u")
                .Replace("ű", "u");

            return $"{vezetek}.{kereszt}@gmail.com";
        }

    }
}
