using PR108_2017_Web_projekat.Models.Konstruktori;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PR108_2017_Web_projekat.Services
{
    public class FitnesCentarService : BazaService
    {
        private static FitnesCentarService fcs;
        private List<FitnesCentar> listFC = new List<FitnesCentar>();
        public static FitnesCentarService FitnessInstance { get{ if (fcs == null){ fcs = new FitnesCentarService(); fcs.Initial("fitnescentri.txt");}  return fcs; }}
        public FitnesCentarService() : base("fitnescentri.txt") { }
        protected override void CitajLiniju(string linija)
        {
            FitnesCentar fc = FitnesCentar.FitnesCentarParse(linija);
            listFC.Add(fc);
        }
        public List<FitnesCentar> GetAll()
        {
            List<FitnesCentar> lsitFCtemp = new List<FitnesCentar>();
            foreach (FitnesCentar fc in listFC)
            {
                if (!fc.Obrisan) lsitFCtemp.Add(fc);
            }
            return lsitFCtemp;
        }
        public void Add(FitnesCentar fc)
        {
            listFC.Add(fc);
            DodajLiniju(new string[] { fc.ToString() });
        }
        public FitnesCentar getFCIme(string ime)
        {
            foreach (FitnesCentar fc in listFC)
            {
                if (fc.Naziv == ime && !fc.Obrisan) return fc;                
            }
            return null;
        }
        public List<FitnesCentar> getVlasnikFC(string ime)
        {
            List<FitnesCentar> listFCtemp = new List<FitnesCentar>();
            foreach (FitnesCentar fc in listFC)
            {
                if (!fc.Obrisan && fc.Vlasnik == ime)
                    listFCtemp.Add(fc);
            }
            return listFCtemp;
        }
        public bool AzuriranjeFC(FitnesCentar fc)
        {
            for (int i = 0; i < listFC.Count; i++)
            {
                if (listFC[i].Naziv == fc.Naziv)
                {
                    listFC[i] = fc;
                    IspisiSveLinije(listFC.ConvertAll(x => x.ToString()));
                    return true;
                }
            }
            return false;
        }
        public void ObrisiFC(string ime)
        {
            FitnesCentar fc = getFCIme(ime);
            fc.Obrisan = true;
            AzuriranjeFC(fc);
        }
        public bool DaLiPostoji(string ime)
        {
            foreach (FitnesCentar fc in listFC)
            {
                if (fc.Naziv.ToLower() == ime.ToLower() && !fc.Obrisan)
                    return true;
            }
            return false;
        }
        public void ObrisiTrenera(string ime, string imeFitnesCentra)
        {
            foreach (FitnesCentar fc in listFC)
            {
                if (fc.Naziv == imeFitnesCentra)
                {
                    fc.Trener.Remove(ime);
                    AzuriranjeFC(fc);
                    break;
                }
            }
        }
        public void IzmeniImeFc(string staroIme, string novoIme)
        {
            for (int i = 0; i < listFC.Count; i++)
            {
                if (listFC[i].Naziv == staroIme)
                {
                    listFC[i].Naziv = novoIme;
                    IspisiSveLinije(listFC.ConvertAll(x => x.ToString()));
                    break;
                }
            }
        }
        public List<FitnesCentar> Pretraga(FCPretraga pretraga, List<FitnesCentar> tempList)
        {
            if (string.IsNullOrWhiteSpace(pretraga.Naziv) && 
                string.IsNullOrWhiteSpace(pretraga.Ulica) && 
                string.IsNullOrWhiteSpace(pretraga.Broj.ToString()) &&
                string.IsNullOrWhiteSpace(pretraga.MestoGrad) &&
                string.IsNullOrWhiteSpace(pretraga.Od.ToString()) && 
                string.IsNullOrWhiteSpace(pretraga.Do.ToString()))
                return tempList;

            if (!string.IsNullOrWhiteSpace(pretraga.Naziv)) //naziv
                tempList = tempList.Where(a => a.Naziv.ToLower().Contains(pretraga.Naziv.ToLower())).ToList();

            if (!string.IsNullOrWhiteSpace(pretraga.Ulica))
            {
                tempList = tempList.Where(a => a.Adresa.Ulica.ToLower().Contains(pretraga.Ulica.ToLower())).ToList();
                if (pretraga.Broj != 0)
                    tempList = tempList.Where(a => a.Adresa.Broj == pretraga.Broj).ToList();
            } //ulica/broj

            if (!string.IsNullOrWhiteSpace(pretraga.MestoGrad))
            {
                tempList = tempList.Where(a => a.Adresa.MestoGrad.ToLower().Contains(pretraga.MestoGrad.ToLower())).ToList();
            }//mestograd

            if (pretraga.Od != 0)//od
                tempList = tempList.Where(a => a.GodinaOtvaranja >= pretraga.Od).ToList();

            if (pretraga.Do != 0)//do
                tempList = tempList.Where(a => a.GodinaOtvaranja <= pretraga.Do).ToList();
            return tempList;
        }
        public List<FitnesCentar> Sortiranje(string sortiranje, List<FitnesCentar> tempList)
        {
            List<FitnesCentar> temp = new List<FitnesCentar>();
            if (sortiranje == "ImeRastuce")
                temp = tempList.OrderBy(s => s.Naziv).ToList();
            else if (sortiranje == "ImeOpadajuce")
                temp = tempList.OrderByDescending(s => s.Naziv).ToList();
            else if (sortiranje == "AdresaRastuce")
                temp = tempList.OrderBy(s => s.Adresa.Ulica).ToList();
            else if (sortiranje == "AdresaOpadajuce")
                temp = tempList.OrderByDescending(s => s.Adresa.Ulica).ToList();
            else if (sortiranje == "GodinaRastuce")
                temp = tempList.OrderBy(s => s.GodinaOtvaranja).ToList();
            else if (sortiranje == "GodinaOpadajuce")
                temp = tempList.OrderByDescending(s => s.GodinaOtvaranja).ToList();
            else
                return temp;
            return temp;
        }
    }
}