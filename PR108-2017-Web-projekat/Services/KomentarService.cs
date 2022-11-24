using PR108_2017_Web_projekat.Models.Konstruktori;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PR108_2017_Web_projekat.Services
{
    public class KomentarService : BazaService
    {
        private static KomentarService ks;
        private List<Komentar> komentarList = new List<Komentar>();
        public static KomentarService CommentInstance
        {
            get
            {
                if (ks == null)
                {
                    ks = new KomentarService();
                    ks.Initial("komentari.txt");
                }
                return ks;
            }
        }
        public KomentarService() : base("komentari.txt") { }
        protected override void CitajLiniju(string linija)
        {
            Komentar c = Komentar.KomentarParse(linija);
            komentarList.Add(c);
        }
        public List<Komentar> GetSve()
        {
            List<Komentar> tempList = new List<Komentar>();
            foreach (Komentar t in komentarList)
            {
                tempList.Add(t);
            }
            return tempList;
        }
        public void Add(Komentar komentar)
        {
            komentarList.Add(komentar);
            DodajLiniju(new string[] { komentar.ToString() });
        }
        public (List<Komentar> odobren, List<Komentar> odbijen) GetKomentarZaFC(string fc)
        {
            List<Komentar> OdobrenLista = new List<Komentar>();
            List<Komentar> OdbijenLista = new List<Komentar>();
            foreach (Komentar kom in komentarList)
            {
                if (kom.FitnesCentarNaKojiSeKomentarOdnosi == fc && kom.Odobren)
                {
                    OdobrenLista.Add(kom);
                }
                else if (kom.FitnesCentarNaKojiSeKomentarOdnosi == fc && kom.Odbijen)
                {
                    OdbijenLista.Add(kom);
                }
            }
            return (OdobrenLista, OdbijenLista);
        }
        public Komentar GetKomentar(string fc, string posetilacIme)
        {
            foreach (Komentar kom in komentarList)
            {
                if (kom.FitnesCentarNaKojiSeKomentarOdnosi == fc && kom.PosetilacKojiJeOstavioKomentar == posetilacIme)
                    return kom;
            }
            return null;
        }
        public void AzurirajKomentar(Komentar komentar)
        {
            for (int i = 0; i < komentarList.Count; i++)
            {
                if (komentarList[i].FitnesCentarNaKojiSeKomentarOdnosi == komentar.FitnesCentarNaKojiSeKomentarOdnosi && 
                    komentarList[i].PosetilacKojiJeOstavioKomentar == komentar.PosetilacKojiJeOstavioKomentar)
                {
                    komentarList[i] = komentar;
                    IspisiSveLinije(komentarList.ConvertAll(x => x.ToString()));
                }
            }
        }
        public List<Komentar> KomentarKojiJeOdobrioVlasnik(Vlasnik vlasnik)
        {
            List<Komentar> tempList = new List<Komentar>();
            foreach (string temp in vlasnik.MojiFitnesCentri)
            {
                foreach (Komentar komentar in komentarList)
                {
                    if (komentar.FitnesCentarNaKojiSeKomentarOdnosi == temp &&
                        !komentar.Odobren && !komentar.Odbijen)
                        tempList.Add(komentar);
                }
            }
            return tempList;
        }
        public void OdbijenKomentar(string posetilac, string fc)
        {
            for (int i = 0; i < komentarList.Count; i++)
            {
                if (komentarList[i].FitnesCentarNaKojiSeKomentarOdnosi == fc && komentarList[i].PosetilacKojiJeOstavioKomentar == posetilac)
                {
                    komentarList[i].Odbijen = true;
                    AzurirajKomentar(komentarList[i]);
                    break;
                }
            }
        }
        public void OdobrenKomentar(string posetilac, string fc)
        {
            for (int i = 0; i < komentarList.Count; i++)
            {
                if (komentarList[i].FitnesCentarNaKojiSeKomentarOdnosi == fc && komentarList[i].PosetilacKojiJeOstavioKomentar == posetilac)
                {
                    komentarList[i].Odobren = true;
                    AzurirajKomentar(komentarList[i]);
                    break;
                }
            }
        }
    }
}