using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
//● Naziv
//● Tip treninga(yoga, les mills tone, body pump itd. )
//● Fitnes centar gde se održava trening(veza sa klasom Fitnes centar)
//● Trajanje treninga(izraženo u minutima)
//● Datum i vreme treninga(čuvati u formatu dd/MM/yyyy HH:mm)
//● Maksimalan broj posetilaca
//● Spisak posetilaca(lista Korisnika sa ulogom Posetilac koji su se prijavili da pohađaju
//trening)
namespace PR108_2017_Web_projekat.Models.Konstruktori
{
    public enum TipTreninga { YOGA, LESSMILLSTONE, BODYPUMP }
    public class GrupniTrening
    {
        public GrupniTrening()
        {
            SpisakPosetilaca = new List<string>();
            Obrisan = false;
        }
        public GrupniTrening(GrupniTrening grupniTrening)
        {
            this.Naziv = grupniTrening.Naziv;
            this.TipTreninga = grupniTrening.TipTreninga;
            this.FitnesCentarGdeSeOdrzavaTrening = grupniTrening.FitnesCentarGdeSeOdrzavaTrening;
            this.TrajanjeTreninga = grupniTrening.TrajanjeTreninga;
            this.DatumIVremeTreninga = grupniTrening.DatumIVremeTreninga;
            this.MaksimalanBrojPosetilaca = grupniTrening.MaksimalanBrojPosetilaca;
            this.TrentniBrojPosetilaca = grupniTrening.TrentniBrojPosetilaca;
            this.SpisakPosetilaca = grupniTrening.SpisakPosetilaca;

            this.Trener = grupniTrening.Trener;
            this.Obrisan = false;
        }
        public string Naziv { get; set; }
        public TipTreninga TipTreninga{ get; set; }//(yoga, les mills tone, body pump itd. )
        public string FitnesCentarGdeSeOdrzavaTrening{get;set;}    //(veza sa klasom Fitnes centar)
        public int TrajanjeTreninga{get;set;}//(izraženo u minutima) MOZDA TREBA INT
        public DateTime DatumIVremeTreninga {get;set;}//(čuvati u formatu dd/MM/yyyy HH:mm)
        public int MaksimalanBrojPosetilaca{get;set;}
        public int TrentniBrojPosetilaca { get; set; }
        public string Trener { get; set; }
        public bool Obrisan { get; set; }
        public List<string> SpisakPosetilaca{get;set;}//(lista Korisnika sa ulogom Posetilac koji su se prijavili da pohađaju trening)
        public static GrupniTrening GrupniTreningParse(string temp)
        {
            string[] tokeni = temp.Split('|');
            string[] tokenList = tokeni[1].Split(';');
            tokeni = tokeni[0].Split(';');
            Enum.TryParse(tokeni[1], out TipTreninga tip);
            Int32.TryParse(tokeni[2], out int trajanje);
            DateTime.TryParseExact(tokeni[3], "dd/MM/yyyy HH:mm", new CultureInfo("en-US", true), System.Globalization.DateTimeStyles.None, out DateTime datumivremetreninga);
            Int32.TryParse(tokeni[4], out int trenutnoPosetilaca);
            bool.TryParse(tokeni[7], out bool obrisan);
            Int32.TryParse(tokeni[8], out int maksimalanbrojposetilaca);

            GrupniTrening gtRet = new GrupniTrening {
                Trener = tokeni[6],
                Naziv = tokeni[0],
                TipTreninga = tip,
                DatumIVremeTreninga = datumivremetreninga,
                TrajanjeTreninga = trajanje,
                MaksimalanBrojPosetilaca = maksimalanbrojposetilaca,
                TrentniBrojPosetilaca = trenutnoPosetilaca,
                Obrisan = obrisan,
                FitnesCentarGdeSeOdrzavaTrening = tokeni[5] };

            if (tokenList != null && tokenList.Count() > 0 && tokenList[0] != "")
            {
                foreach (string pom in tokenList)
                {
                    gtRet.SpisakPosetilaca.Add(pom);
                }
            }
            gtRet.Naziv = tokeni[0];
            return gtRet;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Naziv).Append(';');
            sb.Append(TipTreninga).Append(';');
            sb.Append(TrajanjeTreninga).Append(';');
            sb.Append(DatumIVremeTreninga.ToString("dd/MM/yyyy HH:mm")).Append(';');
            sb.Append(TrentniBrojPosetilaca).Append(';');
            sb.Append(FitnesCentarGdeSeOdrzavaTrening).Append(';');
            sb.Append(Trener).Append(';');
            sb.Append(Obrisan).Append(';');
            sb.Append(MaksimalanBrojPosetilaca).Append('|');

            for (int i = 0; i < SpisakPosetilaca.Count; i++)
            {
                if (SpisakPosetilaca.Count - 1 == i){ sb.Append(SpisakPosetilaca[i]); break;}
                sb.Append(SpisakPosetilaca[i]).Append(';');
            }
            return sb.ToString();
        }
    }
    public class PretragaProsliTreninzi
    {
        public string Ime { get; set; }
        public string TipTreninga { get; set; }
        public string FitnesCentar { get; set; }
    }
    public class PretragaProsliTreninziTrener
    {
        public string Ime { get; set; }
        public string TipTreninga { get; set; }
        public string FitnesCentar { get; set; }
        public DateTime OdDatuma { get; set; }
        public DateTime DoDatuma { get; set; }

    }
    public class AddModelGrupniTrening
    {
        public string Ime { get; set; }
        public TipTreninga TipTreninga { get; set; }
        public int TrajanjeTreninga { get; set; }
        public DateTime DatumIVremeTreninga { get; set; }
        public int MaksimalniBrojPosetilaca { get; set; }
    }
    public class GrupniTreningIzmena
    {
        public string Ime { get; set; }
        public int TrajanjeTreninga { get; set; }
        public DateTime DatumIVremeTreninga { get; set; }
        public int MaksimalanBrojPosetilaca { get; set; }
    }
}