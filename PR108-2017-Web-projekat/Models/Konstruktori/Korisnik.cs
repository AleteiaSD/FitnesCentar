using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace PR108_2017_Web_projekat.Models.Konstruktori
{
    public enum KorisnikPol { MUSKO, ZENSKO }
    public enum KorisnikUloga { POSETILAC, TRENER, VLASNIK}
    public class Korisnik
    {
        public Korisnik()
        {
            this.Obrisan = false;
        }

        public Korisnik(Korisnik korisnik)
        {
            this.KorisnickoIme = korisnik.KorisnickoIme;
            this.Lozinka = korisnik.Lozinka;
            this.Ime = korisnik.Ime;
            this.Prezime = korisnik.Prezime;
            this.Pol = korisnik.Pol;
            this.Email = korisnik.Email;
            this.DatumRodjenja = korisnik.DatumRodjenja;
            this.Uloga = korisnik.Uloga;

            this.Obrisan = false;//nije obrisan
        }
        
        public string KorisnickoIme { get; set; }
        public string Lozinka { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public KorisnikPol Pol { get; set; }
        public string Email { get; set; } //PROVERI DA LI IDE OVAKO
        public DateTime DatumRodjenja { get; set; } //(čuvati u formatu dd/MM/yyyy)
        public KorisnikUloga Uloga { get; set; }

        public bool Obrisan { get; set; }



        public static Korisnik Parse(string korisnik)
        {/*
            this.KorisnickoIme = korisnik.KorisnickoIme;
            this.Lozinka = korisnik.Lozinka;
            this.Ime = korisnik.Ime;
            this.Prezime = korisnik.Prezime;
            this.Pol = korisnik.Pol;
            this.Email = korisnik.Email;
            this.DatumRodjenja = korisnik.DatumRodjenja;
            this.Uloga = korisnik.Uloga;

            this.Obrisan = false;//nije obrisan
             */
            string[] tokeni = korisnik.Split(';');
            Enum.TryParse(tokeni[4], out KorisnikPol pol);
            DateTime.TryParseExact(tokeni[6], "dd/MM/yyyy", new CultureInfo("en-US", true), System.Globalization.DateTimeStyles.None, out DateTime datumRodjenja);
            Enum.TryParse(tokeni[7], out KorisnikUloga uloga);
            bool.TryParse(tokeni[8], out bool obrisan);
            Korisnik korisnikRet = new Korisnik()
            {
                KorisnickoIme = tokeni[0],
                Lozinka = tokeni[1],
                Ime = tokeni[2],
                Prezime = tokeni[3],
                Pol = pol,
                Email = tokeni[5],
                DatumRodjenja = datumRodjenja,
                Uloga = uloga,
                Obrisan = obrisan
            };
            return korisnikRet;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(KorisnickoIme).Append(";");
            sb.Append(Lozinka).Append(";");
            sb.Append(Ime).Append(";");
            sb.Append(Prezime).Append(";");
            sb.Append(Pol).Append(";");
            sb.Append(Email).Append(";");
            sb.Append(DatumRodjenja.ToString("dd/MM/yyyy")).Append(";");
            sb.Append(Uloga).Append(";");
            sb.Append(Obrisan);
            return sb.ToString(); 
        }

    }
    public class RegistrationModel
    {
        public string KorisnickoIme { get; set; }
        public string Lozinka { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public KorisnikPol Pol { get; set; }
        public string Email { get; set; } //PROVERI DA LI IDE OVAKO
        public DateTime DatumRodjenja { get; set; }
        public KorisnikUloga Uloga { get; set; }
    }
    public class LoginModel
    {
        public string KorisnickoIme { get; set; }
        public string Lozinka { get; set; }
    }
    public class ProfileModel
    {       
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Lozinka { get; set; }
    }
}