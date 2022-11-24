using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace PR108_2017_Web_projekat.Models.Konstruktori
{    
    public class Komentar
    {
        public Komentar()
        {
            Odobren = false;
            Odbijen = false;
        }
        public string PosetilacKojiJeOstavioKomentar { get; set; }
        public string FitnesCentarNaKojiSeKomentarOdnosi { get; set; }
        public string TekstKomentara { get; set; }
        public int Ocena { get; set; }//od 1 do 5
        public bool Odobren { get; set; }
        public bool Odbijen { get; set; }
        public static Komentar KomentarParse(string temp)
        {
            string[] tokeni = temp.Split(';');
            Int32.TryParse(tokeni[3], out int ocena);
            bool.TryParse(tokeni[4], out bool odobren);
            bool.TryParse(tokeni[5], out bool odbijen);
            Komentar komRet = new Komentar() {
                PosetilacKojiJeOstavioKomentar = tokeni[0],
                FitnesCentarNaKojiSeKomentarOdnosi = tokeni[1],
                TekstKomentara = tokeni[2],
                Ocena = ocena,
                Odobren = odobren,
                Odbijen = odbijen };
            return komRet;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(PosetilacKojiJeOstavioKomentar).Append(';');
            sb.Append(FitnesCentarNaKojiSeKomentarOdnosi).Append(';');
            sb.Append(TekstKomentara).Append(';');
            sb.Append(Ocena).Append(';');
            sb.Append(Odobren).Append(';');
            sb.Append(Odbijen);
            return sb.ToString();
        }
    }
    public class AddKomentarModel
    {
        public string Komentar { get; set; }
        public int Ocena  { get; set; }
        public string FitnesCentar { get; set; }
    }

}