using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace PR108_2017_Web_projekat.Models.Konstruktori
{
    public class Vlasnik:Korisnik
    {
        public List<string> MojiFitnesCentri { get; set; }
        public Vlasnik()
        {
            MojiFitnesCentri = new List<string>();
        }
        public Vlasnik(Korisnik kor) : base(kor)
        {
            MojiFitnesCentri = new List<string>();
        }
        public static Vlasnik VlasnikParse(string temp)
        {
            string[] tokeni = temp.Split('|');
            Korisnik kor = Korisnik.Parse(tokeni[0]);
            Vlasnik vlasRet = new Vlasnik(kor);
            tokeni = tokeni.Where((item, index) => index != 0).ToArray(); //???
            if (tokeni[0] != "" || tokeni[0].Length > 0)
            {
                string[] token2 = tokeni[0].Split(';');
                foreach (string ss in token2)
                    vlasRet.MojiFitnesCentri.Add(ss);
            }
            return vlasRet;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToString()).Append('|');

            for (int i = 0; i < MojiFitnesCentri.Count; i++)
            {
                if (MojiFitnesCentri.Count - 1 == i){ sb.Append(MojiFitnesCentri[i]); break; }
                sb.Append(MojiFitnesCentri[i]).Append(';');
            }
            return sb.ToString();
        }
    }
}