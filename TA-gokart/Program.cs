using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Tracing;
using System.Xml.Serialization;

namespace TA_gokart
{

    internal class Program
    {
        public static Random random = new Random();
        
        static DateTime RandomSzuletesiDatum(int minAge = 14, int maxAge = 65)
        {

            DateTime today = DateTime.Today;
            DateTime legkesobbiDatum = today.AddYears(-minAge);
            DateTime legkorabbiDatum = today.AddYears(-(maxAge + 1)).AddDays(1);
            int napokSzama = (legkesobbiDatum - legkorabbiDatum).Days;

            return legkorabbiDatum.AddDays(random.Next(napokSzama + 1));
        }

        static List<string> Keresztnev(string keresztnevek, int felhasznaloSzam)
        {
            string[] keresztnevlist = keresztnevek.Split(',');
            for (int i = 0; i < keresztnevlist.Length; i++)
            {
                keresztnevlist[i] = keresztnevlist[i].Trim(' ', '\'', ';', '\r', '\n');
            }
            int keresztnevCount = keresztnevlist.Length;

            List<string> generalt_keresztnevek = new List<string>();
            for (int i = 0; i < felhasznaloSzam; i++)
            {
                int keresztnevIndex = random.Next(0, keresztnevCount);
                string keresztnev = keresztnevlist[keresztnevIndex];
                generalt_keresztnevek.Add(keresztnev);
            }

            return generalt_keresztnevek;

        }
        static List<string> Vezeteknev(string vezeteknevek, int felhasznaloSzam)
        {
            string[] vezeteknevlist = vezeteknevek.Split(',');
            for (int i = 0; i < vezeteknevlist.Length; i++)
            {
                vezeteknevlist[i] = vezeteknevlist[i].Trim(' ', '\'', ';', '\r', '\n');
            }
            int vezeteknevCount = vezeteknevlist.Length;

            List<string> generalt_vezeteknevek = new List<string>();
            for (int i = 0; i < felhasznaloSzam; i++)
            {
                int vezeteknevIndex = random.Next(0, vezeteknevCount);
                string vezeteknev = vezeteknevlist[vezeteknevIndex];
                generalt_vezeteknevek.Add(vezeteknev);
            }

            return generalt_vezeteknevek;

        }

        static List<Versenyzo> VersenyzokGen()
        {
            int generaltfelhasznalok = random.Next(1, 151);

            string vezeteknevek = File.ReadAllText("vezeteknevek.txt");
            string keresztnevek = File.ReadAllText("keresztnevek.txt");

            List<Versenyzo> versenyzok = new List<Versenyzo>();

            var generaltvezeteknevek = Vezeteknev(vezeteknevek, generaltfelhasznalok);
            var generaltkeresztnevek = Keresztnev(keresztnevek, generaltfelhasznalok);

            for (int i = 0; i  < generaltfelhasznalok; i++)
            {
                versenyzok.Add(new Versenyzo(generaltvezeteknevek[i], generaltkeresztnevek[i], RandomSzuletesiDatum()));
            }



            return versenyzok;
        }
        
        static void Main(string[] args)
        {
            /*
             TA-Gokart
             2026.09.07.
             Gokart időpontfoglaló - Egyéni kisprojekt
            */
            Helyszin helyszin = new Helyszin();
            Palya tabla = new Palya();

            var versenyzok = VersenyzokGen();
            helyszin.GoKartKiirat();

            Console.Write("\nKérlek válassz az alábbi pontok közül!\n1.) Foglalások megjelenítése\n2.) Foglalás módosítása\n3.) Foglalás törlése\n4.) Új foglalás\n5.) Kilépés\nVálaszod: ");
            int menupont = int.Parse(Console.ReadLine());

            switch ( menupont ) {
                case 1: tabla.PalyaTabla(); break;
                case 2: Console.WriteLine(""); break;
                case 3: Console.WriteLine(""); break;
                case 4: Console.WriteLine(""); break;
                case 5: Environment.Exit(0); break;
            }

            /*foreach (var versenyzo in versenyzok)
            {
                Console.WriteLine($"\nNév: {versenyzo.Vezeteknev} {versenyzo.Keresztnev}\n" +
                    $"Születési idő: {versenyzo.Szulido.ToString("yyyy. MM. dd.")}\n" +
                    $"18 elmúlt-e? {versenyzo.Nagykoru}\n" +
                    $"Azonosító: {versenyzo.Azonosito}\n" +
                    $"Email: {versenyzo.Email}");
            }
            */




        }
    }
}