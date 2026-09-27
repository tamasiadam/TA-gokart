using System;
using System.Globalization;
using System.Text;


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

        public static string EkezetNelkul(string szoveg)
        {
            string normalizalt = szoveg.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            foreach (char c in normalizalt)
            {
                var kategoria = CharUnicodeInfo.GetUnicodeCategory(c);
                if (kategoria != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }
        public string AzonositoGen()
        {
            string azonositodatum = Szulido.ToString("yyyyMMdd");
            string vezetek = EkezetNelkul(Vezeteknev);
            string kereszt = EkezetNelkul(Keresztnev);
            return $"GO-{vezetek}{kereszt}-{azonositodatum}";
        }

        public string EmailGen()
        {
            string vezetek = EkezetNelkul(Vezeteknev).ToLower();
            string kereszt = EkezetNelkul(Keresztnev).ToLower();
            return $"{vezetek}.{kereszt}@gmail.com";
        }

    }
}