using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace PR108_2017_Web_projekat.Models.Konstruktori
{
    public class FitnesCentarITrening
    {
        public string NazivTreninga { get; set; }
        public string NazivFitnesCentra { get; set; }
    }
    public class Posetilac:Korisnik
    {
        public List<FitnesCentarITrening> MojiTreninzi { get; set; }
        public Posetilac()
        {
            MojiTreninzi = new List<FitnesCentarITrening>();
        }
        public Posetilac(Korisnik kor) : base(kor)
        {
            MojiTreninzi = new List<FitnesCentarITrening>();
        }
        public static Posetilac PosetilacParse(string temp)
        {
            string[] tokeni = temp.Split('|');
            Korisnik kor = Korisnik.Parse(tokeni[0]);
            Posetilac posRet = new Posetilac(kor);
            tokeni = tokeni.Where((item, index) => index != 0).ToArray();
            if (tokeni[0] != "")
            {
                string[] parts = tokeni[0].Split(';');
                foreach (string ss in parts)
                {
                    string[] tokeni2 = ss.Split('^');
                    FitnesCentarITrening tc = new FitnesCentarITrening { NazivTreninga = tokeni2[0], NazivFitnesCentra = tokeni2[1] };
                    posRet.MojiTreninzi.Add(tc);
                }
            }
            return posRet;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToString()).Append('|');
            string temp = "";
            for (int i = 0; i < MojiTreninzi.Count; i++)
            {
                temp = MojiTreninzi[i].NazivTreninga + '^' + MojiTreninzi[i].NazivFitnesCentra;
                if (MojiTreninzi.Count - 1==i){sb.Append(temp);break;}
                sb.Append(temp).Append(';');
            }
            return sb.ToString();
        }
    }
}