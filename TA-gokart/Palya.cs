using System;
using System.Collections.Generic;
using System.Linq;

namespace TA_gokart
{
    internal class Palya : FoglalasFeltetelek
    {
        public Dictionary<DateTime, List<string>> Foglalasok = new Dictionary<DateTime, List<string>>();
        private int NapiFoglaltOrak(Versenyzo versenyzo, DateTime nap)
        {
            int ossz = 0;
            foreach (var idopont in Foglalasok.Keys)
            {
                if (idopont.Date == nap.Date && Foglalasok[idopont].Contains(versenyzo.Azonosito))
                {
                    ossz++;
                }
            }
            return ossz;
        }

        public bool IdopontBeallitas(Versenyzo versenyzo)
        {
            Console.Write("Add meg a dátumot (éééé.hh.nn): ");
            DateTime datum = DateTime.Parse(Console.ReadLine());

            Console.Write($"Add meg a kezdő órát ({Nyitas}-{Zaras - 1}): ");
            int ora = int.Parse(Console.ReadLine());

            Console.Write($"Hány órára foglal ({minIdotartam} vagy {maxIdotartam})? ");
            int hossz = int.Parse(Console.ReadLine());

            Kezdet = datum.Date.AddHours(ora);
            Idotartam = hossz;

            DateTime zarasIdopontja = Kezdet.Date.AddHours(Zaras);
            if (ora < Nyitas || Veg > zarasIdopontja)
            {
                Console.WriteLine($"A megadott időpont kívül esik a nyitvatartáson ({Nyitas}-{Zaras})!");
                return false;
            }

            if (Idotartam < minIdotartam || Idotartam > maxIdotartam)
            {
                Console.WriteLine($"A foglalás hossza {minIdotartam} és {maxIdotartam} óra között lehet!");
                return false;
            }

            int mostaniOrak = NapiFoglaltOrak(versenyzo, Kezdet.Date);
            if (mostaniOrak + Idotartam > maxIdotartam)
            {
                Console.WriteLine($"{versenyzo.Azonosito} ezen a napon már {mostaniOrak} órát foglalt, ez meghaladná a napi {maxIdotartam} órás limitet!");
                return false;
            }

            for (DateTime idopont = Kezdet; idopont < Veg; idopont = idopont.AddHours(1))
            {
                if (Foglalasok.ContainsKey(idopont))
                {
                    if (Foglalasok[idopont].Contains(versenyzo.Azonosito))
                    {
                        Console.WriteLine($"{versenyzo.Azonosito} már foglalt erre az időpontra: {idopont:HH:00}!");
                        return false;
                    }
                    if (Foglalasok[idopont].Count >= maxVersenyzo)
                    {
                        Console.WriteLine($"Betelt a pálya ekkor: {idopont:HH:00}!");
                        return false;
                    }
                }
            }

            for (DateTime idopont = Kezdet; idopont < Veg; idopont = idopont.AddHours(1))
            {
                if (!Foglalasok.ContainsKey(idopont))
                    Foglalasok[idopont] = new List<string>();

                Foglalasok[idopont].Add(versenyzo.Azonosito);
            }

            Console.WriteLine($"\n{versenyzo.Azonosito} sikeresen lefoglalva: {Kezdet:yyyy.MM.dd.} {Kezdet:HH}:00-tól {Veg:HH}:00-ig.");
            return true;
        }

        public void FoglalasTorles(Versenyzo versenyzo)
        {
            bool voltTorles = false;

            foreach (var idopont in Foglalasok.Keys.ToList())
            {
                if (Foglalasok[idopont].Contains(versenyzo.Azonosito))
                {
                    Foglalasok[idopont].Remove(versenyzo.Azonosito);
                    voltTorles = true;
                }
            }

            if (voltTorles)
                Console.WriteLine($"{versenyzo.Azonosito} foglalásai törölve.");
            else
                Console.WriteLine($"{versenyzo.Azonosito} nem rendelkezik foglalással.");
        }

        public void FoglalasModositas(Versenyzo versenyzo)
        {
            FoglalasTorles(versenyzo);
            Console.WriteLine("Add meg az új időpontot:");
            IdopontBeallitas(versenyzo);
        }

        public void PalyaTabla()
        {
            DateTime ma = DateTime.Today;
            DateTime honapVege = new DateTime(ma.Year, ma.Month, DateTime.DaysInMonth(ma.Year, ma.Month));

            int oszlopSzelesseg = 8;

            Console.Write("".PadRight(12));
            for (int ora = Nyitas; ora < Zaras; ora++)
            {
                string cimke = $"{ora}-{ora + 1}";
                Console.Write(cimke.PadRight(oszlopSzelesseg));
            }
            Console.WriteLine();

            for (DateTime nap = ma; nap <= honapVege; nap = nap.AddDays(1))
            {
                Console.Write(nap.ToString("yyyy.MM.dd.").PadRight(12));

                for (int ora = Nyitas; ora < Zaras; ora++)
                {
                    DateTime idopont = nap.Date.AddHours(ora);
                    bool foglalt = Foglalasok.ContainsKey(idopont) && Foglalasok[idopont].Count > 0;

                    Console.BackgroundColor = foglalt ? ConsoleColor.Red : ConsoleColor.Green;
                    Console.Write("       ");
                    Console.ResetColor();
                    Console.Write(" ");
                }

                Console.WriteLine();
            }

            Console.ResetColor();
        }
    }
}