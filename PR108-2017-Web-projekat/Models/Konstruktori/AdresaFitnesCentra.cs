using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace PR108_2017_Web_projekat.Models.Konstruktori
{
    public class AdresaFitnesCentra
    {
        public AdresaFitnesCentra()
        {
            Ulica = "";
            Broj = 0;
            MestoGrad = "";
            PostanskiBroj = 0;
        }
        public AdresaFitnesCentra(string ulica, int broj, string mestoGrad, int postanskiBroj)
        {
            Ulica = ulica;
            Broj = broj;
            MestoGrad = mestoGrad;
            PostanskiBroj = postanskiBroj;
        }
        public string Ulica{ get; set; }
        public int Broj { get; set; }
        public string MestoGrad { get; set; }
        public int PostanskiBroj { get; set; }
        public static AdresaFitnesCentra AdresaFitnesCentraParse(string temp)
        {
            string[] tokeni = temp.Split(';');
            Int32.TryParse(tokeni[1], out int numStreet);
            Int32.TryParse(tokeni[3], out int postalNum);

            AdresaFitnesCentra afcRet = new AdresaFitnesCentra(tokeni[0], numStreet, tokeni[2], postalNum);

            return afcRet;
        }
        public override bool Equals(object obj)
        {
            AdresaFitnesCentra afc = obj as AdresaFitnesCentra;
            if ( afc.Ulica == this.Ulica &&
                 afc.Broj == this.Broj &&
                 afc.MestoGrad == this.MestoGrad
               )           
                return true;            
            else
                return false;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Ulica).Append(';');
            sb.Append(Broj).Append(';');
            sb.Append(MestoGrad).Append(';');
            sb.Append(PostanskiBroj);

            return sb.ToString();
        }        
    }
}