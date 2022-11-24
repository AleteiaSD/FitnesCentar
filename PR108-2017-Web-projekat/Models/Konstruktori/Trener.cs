using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace PR108_2017_Web_projekat.Models.Konstruktori
{
    public class Trener : Korisnik
    {
        public string FitnesCentar { get; set; }
        public List<string> GrupniTreninzi { get; set; }
        public Trener()
        {
            FitnesCentar = "";
            GrupniTreninzi = new List<string>();
        }
        public Trener(Korisnik kor) : base(kor)
        {
            FitnesCentar = "";
            GrupniTreninzi = new List<string>();
        }        
        public static Trener TrenerParse(string temp)
        {
            string[] tokeni = temp.Split('|');
            Korisnik kor = Korisnik.Parse(tokeni[0]);
            Trener trenRet = new Trener(kor);
            trenRet.FitnesCentar = tokeni[1];
            tokeni = tokeni.Where((item, index) => index != 0 && index != 1).ToArray();
            if (tokeni[0] != "" || tokeni[0].Length > 1)
            {
                string[] token2 = tokeni[0].Split(';');
                foreach (string ss in token2)
                    trenRet.GrupniTreninzi.Add(ss);
            }
            return trenRet;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToString()).Append('|');
            sb.Append(FitnesCentar).Append('|');

            for (int i = 0; i < GrupniTreninzi.Count; i++)
            {
                if (GrupniTreninzi.Count - 1 == i) { sb.Append(GrupniTreninzi[i]); break; }
                sb.Append(GrupniTreninzi[i]).Append(';');
            }
            return sb.ToString();
        }
    }
    public class InformacijeOTreneru
    {
        public List<Trener> Obrisan { get; set; }
        public List<Trener> NijeObrisan { get; set; }
        public InformacijeOTreneru() { }
    }
}