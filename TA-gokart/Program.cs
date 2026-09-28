using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace TA_gokart
{
    /*
    TA-Gokart 
    Kezdeti dátum: 2026.09.07.
    Projekt neve: Gokart időpontfoglaló - Egyéni kisprojekt
    */
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
                generalt_keresztnevek.Add(keresztnevlist[keresztnevIndex]);
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
                generalt_vezeteknevek.Add(vezeteknevlist[vezeteknevIndex]);
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

            for (int i = 0; i < generaltfelhasznalok; i++)
            {
                versenyzok.Add(new Versenyzo(generaltvezeteknevek[i], generaltkeresztnevek[i], RandomSzuletesiDatum()));
            }

            return versenyzok;
        }

        static void VersenyzokListazasa(List<Versenyzo> versenyzok)
        {
            Console.WriteLine("\n--- Versenyzők listája ---");
            foreach (var versenyzo in versenyzok)
            {
                Console.WriteLine($"\nNév: {versenyzo.Vezeteknev} {versenyzo.Keresztnev}\n" +
                                  $"Születési idő: {versenyzo.Szulido:yyyy. MM. dd.}\n" +
                                  $"18 elmúlt-e? {(versenyzo.Nagykoru ? "Igen" : "Nem")}\n" +
                                  $"Azonosító: {versenyzo.Azonosito}\n" +
                                  $"Email: {versenyzo.Email}");
            }
        }

        static Versenyzo VersenyzoKivalasztasa(List<Versenyzo> versenyzok)
        {
            Versenyzo talalt = null;

            while (talalt == null)
            {
                Console.Write("\nAdd meg a versenyző azonosítóját: ");
                string beirtAzonosito = Console.ReadLine();

                talalt = versenyzok.FirstOrDefault(v => v.Azonosito == beirtAzonosito);

                if (talalt == null)
                {
                    Console.WriteLine("Nincs ilyen azonosítójú versenyző, próbáld újra!");
                }
            }

            return talalt;
        }

        static void KezdetiFoglalasokGeneralasa(List<Versenyzo> versenyzok, Palya tabla)
        {
            DateTime ma = DateTime.Today;
            DateTime honapVege = new DateTime(ma.Year, ma.Month, DateTime.DaysInMonth(ma.Year, ma.Month));

            for (DateTime nap = ma; nap <= honapVege; nap = nap.AddDays(1))
            {
                HashSet<string> napiFoglaltak = new HashSet<string>();

                for (int ora = tabla.Nyitas; ora < tabla.Zaras; ora++)
                {
                    DateTime idopont = nap.Date.AddHours(ora);

                    int meglevoLetszam = tabla.Foglalasok.ContainsKey(idopont) ? tabla.Foglalasok[idopont].Count : 0;

                    if (meglevoLetszam == 0 && random.Next(2) == 0)
                        continue;

                    int celLetszam = Math.Max(meglevoLetszam, random.Next(tabla.minVersenyzo, tabla.maxVersenyzo + 1));

                    List<string> ujAzonositok = new List<string>();
                    HashSet<DateTime> erintettIdopontok = new HashSet<DateTime>();

                    int jelenlegiLetszam = meglevoLetszam;
                    int probalkozas = 0;

                    while (jelenlegiLetszam < celLetszam && probalkozas < 200)
                    {
                        probalkozas++;

                        Versenyzo jelolt = versenyzok[random.Next(versenyzok.Count)];

                        if (napiFoglaltak.Contains(jelolt.Azonosito))
                            continue;

                        int hossz = random.Next(tabla.minIdotartam, tabla.maxIdotartam + 1);
                        if (ora + hossz > tabla.Zaras)
                            hossz = tabla.minIdotartam;

                        bool elfer = true;
                        for (int i = 0; i < hossz; i++)
                        {
                            DateTime ellenorzendo = nap.Date.AddHours(ora + i);
                            int letszamOtt = tabla.Foglalasok.ContainsKey(ellenorzendo) ? tabla.Foglalasok[ellenorzendo].Count : 0;
                            if (letszamOtt >= tabla.maxVersenyzo)
                            {
                                elfer = false;
                                break;
                            }
                        }

                        if (!elfer)
                            continue;

                        for (int i = 0; i < hossz; i++)
                        {
                            DateTime foglalando = nap.Date.AddHours(ora + i);
                            if (!tabla.Foglalasok.ContainsKey(foglalando))
                                tabla.Foglalasok[foglalando] = new List<string>();
                            tabla.Foglalasok[foglalando].Add(jelolt.Azonosito);
                            erintettIdopontok.Add(foglalando);
                        }

                        napiFoglaltak.Add(jelolt.Azonosito);
                        ujAzonositok.Add(jelolt.Azonosito);
                        jelenlegiLetszam = tabla.Foglalasok[idopont].Count;
                    }

                    if (jelenlegiLetszam < tabla.minVersenyzo)
                    {
                        foreach (DateTime erintett in erintettIdopontok)
                        {
                            foreach (string azonosito in ujAzonositok)
                                tabla.Foglalasok[erintett].Remove(azonosito);

                            if (tabla.Foglalasok[erintett].Count == 0)
                                tabla.Foglalasok.Remove(erintett);
                        }

                        foreach (string azonosito in ujAzonositok)
                            napiFoglaltak.Remove(azonosito);
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("TA-Gokart\r\nKezdeti dátum: 2026.09.07.\r\nProjekt neve: Gokart időpontfoglaló - Egyéni kisprojekt");

            Helyszin helyszin = new Helyszin();
            Palya tabla = new Palya();

            var versenyzok = VersenyzokGen();
            KezdetiFoglalasokGeneralasa(versenyzok, tabla);

            helyszin.GoKartKiirat();


            bool kilepes = false;
            while (!kilepes)
            {
                Console.Write("\nKérlek válassz az alábbi pontok közül!\n1.) Foglalások megjelenítése\n2.) Foglalás módosítása\n3.) Foglalás törlése\n4.) Új foglalás\n5.) Kilépés\nVálaszod: ");
                string bemenet = Console.ReadLine();

                if (!int.TryParse(bemenet, out int menupont))
                {
                    Console.WriteLine("Érvénytelen bevitel! Kérlek egy számot adj meg (1-5).");
                    continue;
                }

                switch (menupont)
                {
                    case 1:
                        VersenyzokListazasa(versenyzok);
                        tabla.PalyaTabla();
                        break;
                    case 2:
                        Versenyzo modositando = VersenyzoKivalasztasa(versenyzok);
                        tabla.FoglalasModositas(modositando);
                        break;
                    case 3:
                        Versenyzo torlendo = VersenyzoKivalasztasa(versenyzok);
                        tabla.FoglalasTorles(torlendo);
                        break;
                    case 4:
                        Versenyzo kivalasztott = VersenyzoKivalasztasa(versenyzok);
                        tabla.IdopontBeallitas(kivalasztott);
                        break;
                    case 5:
                        kilepes = true;
                        break;
                    default:
                        Console.WriteLine("Érvénytelen választás!");
                        break;
                }
            }
        }
    }
}
