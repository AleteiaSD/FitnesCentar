using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
//● Naziv
//● Adresa u formatu: ulica i broj, mesto/grad, poštanski broj
//● Godina otvaranja fitnes centra
//● Vlasnik(veza sa klasom Korisnik)
//● Cena mesečne članarine
//● Cena godišnje članarine
//● Cena jednog treninga
//● Cena jednog grupnog treninga
//● Cena jednog treninga sa personalnim trenerom
namespace PR108_2017_Web_projekat.Models.Konstruktori
{
    public class FitnesCentar
    {
        public enum StatusFC{ AKTIVAN, OBRISAN }
        public FitnesCentar()
        {
            Obrisan = false;
            Trener = new List<string>();
        }
        public FitnesCentar(FitnesCentar fc)
        {
            this.Naziv = fc.Naziv;
            this.Adresa = fc.Adresa;
            this.GodinaOtvaranja = fc.GodinaOtvaranja;
            this.Vlasnik = fc.Vlasnik;
            this.CenaMesecneClanarine = fc.CenaMesecneClanarine;
            this.CenaGodisnjeClanarine = fc.CenaGodisnjeClanarine;
            this.CenaJednogTreninga = fc.CenaJednogTreninga;
            this.CenaJednogGrupnogTreninga = fc.CenaJednogGrupnogTreninga;
            this.CenaJednogTreningaSaPersonalnimTrenerom = fc.CenaJednogTreningaSaPersonalnimTrenerom;

            this.Trener = fc.Trener;
            this.Obrisan = fc.Obrisan;
        }
        public List<string> Trener { get; set; }
        public string Naziv { get; set; }
        public AdresaFitnesCentra Adresa  { get; set; }//u formatu: ulica i broj, mesto/grad, poštanski broj
        public int GodinaOtvaranja{ get; set; }
        public string Vlasnik { get; set; }
        public int CenaMesecneClanarine { get; set; }
        public int CenaGodisnjeClanarine { get; set; }
        public int CenaJednogTreninga { get; set; }
        public int CenaJednogGrupnogTreninga { get; set; }
        public int CenaJednogTreningaSaPersonalnimTrenerom { get; set; }    
        public bool Obrisan { get; set; }
        public static FitnesCentar FitnesCentarParse(string temp)
        {
            string[] tokeniPrviDeo = temp.Split('|');
            AdresaFitnesCentra afc = AdresaFitnesCentra.AdresaFitnesCentraParse(tokeniPrviDeo[0]);
            string[] tokeniDrugiDeo = tokeniPrviDeo[1].Split(';');
            Int32.TryParse(tokeniDrugiDeo[1], out int godinaOtvaranja);
            Int32.TryParse(tokeniDrugiDeo[3], out int cenaMesecneClanarine);
            Int32.TryParse(tokeniDrugiDeo[4], out int cenaGodisnjeClanarine);
            Int32.TryParse(tokeniDrugiDeo[5], out int cenaJednogTreninga);
            Int32.TryParse(tokeniDrugiDeo[6], out int cenaJednogGrupnogTreninga);
            Int32.TryParse(tokeniDrugiDeo[7], out int cenaJednogTreningaSaPersonalnimTrenerom);
            bool.TryParse(tokeniDrugiDeo[8], out bool obrisan);

            FitnesCentar fc = new FitnesCentar()
            {
                Adresa = afc,
                Naziv = tokeniDrugiDeo[0],
                GodinaOtvaranja = godinaOtvaranja,
                Vlasnik = tokeniDrugiDeo[2],
                CenaMesecneClanarine = cenaMesecneClanarine,
                CenaGodisnjeClanarine = cenaGodisnjeClanarine,
                CenaJednogGrupnogTreninga = cenaJednogGrupnogTreninga,
                CenaJednogTreninga = cenaJednogTreninga,
                CenaJednogTreningaSaPersonalnimTrenerom = cenaJednogTreningaSaPersonalnimTrenerom,
                Obrisan = obrisan
            };
            if (tokeniPrviDeo[2] != "" && tokeniPrviDeo[2] != null)
            {
                string[] tempList = tokeniPrviDeo[2].Split(';');
                for (int i = 0; i < tempList.Length; i++)
                {
                    fc.Trener.Add(tempList[i]);
                }
            }
            return fc;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Adresa).Append('|');
            sb.Append(Naziv).Append(';');
            sb.Append(GodinaOtvaranja).Append(';');
            sb.Append(Vlasnik).Append(';');
            sb.Append(CenaMesecneClanarine).Append(';');
            sb.Append(CenaGodisnjeClanarine).Append(';');
            sb.Append(CenaJednogTreninga).Append(';');
            sb.Append(CenaJednogGrupnogTreninga).Append(';');
            sb.Append(CenaJednogTreningaSaPersonalnimTrenerom).Append(';');
            sb.Append(Obrisan).Append('|');
            for (int i = 0; i < Trener.Count; i++)
            {
                if ( Trener.Count - 1 == i){ sb.Append(Trener[i]); break; }
                sb.Append(Trener[i]).Append(';');
            }
            return sb.ToString();
        }
    }
    public class FCPretraga
    {
        public string Naziv { get; set; }
        public string Ulica { get; set; }
        public int Broj { get; set; }
        public string MestoGrad { get; set; }
        public int Od { get; set; }
        public int Do { get; set; }
        public string sortiraj { get; set; }
    }
    public class FCDetalji
    {
        public FitnesCentar FCNaziv { get; set; }
        public List<GrupniTrening> listGrupniTreninzi { get; set; }
        public List<Komentar> Odobren { get; set; }
        public List<Komentar> Odbijen { get; set; }
        public FCDetalji()
        {
            listGrupniTreninzi = new List<GrupniTrening>();
            Odobren = new List<Komentar>();
            Odbijen = new List<Komentar>();
        }
    }
    public class FitnesCentarKreiranje
    {
        public string Naziv { get; set; }
        public int GodinaOtvaranja { get; set; }
        public string Ulica { get; set; }
        public int Broj { get; set; }
        public string MestoGrad { get; set; }
        public int PostanskiBroj { get; set; }
        public int CenaMesecneClanarine { get; set; }
        public int CenaGodisnjeClanarine { get; set; }
        public int CenaJednogTreninga { get; set; }
        public int CenaJednogGrupnogTreninga { get; set; }
        public int CenaJednogTreningaSaPersonalnimTrenerom { get; set; }
    }
    public class FitnesCentarIzmena : FitnesCentarKreiranje
    {
        public string StaroIme { get; set; }
    }
}