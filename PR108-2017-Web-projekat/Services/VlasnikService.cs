using PR108_2017_Web_projekat.Models.Konstruktori;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PR108_2017_Web_projekat.Services
{
    public class VlasnikService :BazaService
    {
        private static VlasnikService vlasnikInstance;
        private List<Vlasnik> vlasnikList = new List<Vlasnik>();
        public static VlasnikService VlasnikInstance
        {
            get
            {
                if (vlasnikInstance == null)
                {
                    vlasnikInstance = new VlasnikService();
                    vlasnikInstance.Initial("vlasnici.txt");
                }

                return vlasnikInstance;
            }
        }
        public VlasnikService() : base("vlasnici.txt") { }
        protected override void CitajLiniju(string linija)
        {
            Vlasnik v = Vlasnik.VlasnikParse(linija);
            vlasnikList.Add(v);
        }
        public List<Vlasnik> GetAll()
        {
            List<Vlasnik> tempList = new List<Vlasnik>();
            foreach (Vlasnik vl in vlasnikList)
            {
                tempList.Add(vl);
            }

            return tempList;
        }
        public Vlasnik GetVlasnikIme(string ime)
        {

            foreach (Vlasnik vl in vlasnikList)
            {
                if (vl.KorisnickoIme == ime && !vl.Obrisan)
                {
                    return vl;
                }
            }
            return null;
        }
        public void AzurirajVlasnika(Vlasnik vlasnik)
        {
            for (int i = 0; i < vlasnikList.Count; i++)
            {
                if (vlasnikList[i].KorisnickoIme == vlasnik.KorisnickoIme)
                {
                    vlasnikList[i] = vlasnik;
                    IspisiSveLinije(vlasnikList.ConvertAll(x => x.ToString()));
                }
            }
        }
        public List<FitnesCentar> GetVlasnikFc(List<FitnesCentar> fitnesCentri, string ime)
        {
            List<FitnesCentar> tempList = new List<FitnesCentar>();
            foreach (FitnesCentar fc in fitnesCentri)
            {
                if (!fc.Obrisan && fc.Vlasnik == ime)
                    tempList.Add(fc);
            }
            return tempList;
        }
        public void ObrisiFC(Vlasnik vlasnik, string imeFC)
        {
            foreach (string vl in vlasnik.MojiFitnesCentri)
            {
                if (vl == imeFC)
                {
                    vlasnik.MojiFitnesCentri.Remove(vl);
                    AzurirajVlasnika(vlasnik);
                    break;
                }
            }
            FitnesCentar fc = FitnesCentarService.FitnessInstance.getFCIme(imeFC);
            foreach (string trainer in fc.Trener)
                TrenerService.TrainerInstance.ObrisiTrenera(trainer);

            FitnesCentarService.FitnessInstance.ObrisiFC(imeFC);
        }
        public void KreirajFC(FitnesCentar fc, Vlasnik vlasnik)
        {
            FitnesCentarService.FitnessInstance.Add(fc);
            foreach (Vlasnik vl in vlasnikList)
            {
                if (vl.Ime == vlasnik.Ime)
                {
                    vl.MojiFitnesCentri.Add(fc.Naziv);
                    AzurirajVlasnika(vl);
                    break;
                }
            }
        }
        public (List<Trener> mozeSeObrisati, List<Trener> neMozeSeObrisati) GetVlasniciTrenera(Vlasnik vlasnik)
        {
            List<Trener> listaTreneraSaBuducimTreninzima = new List<Trener>();
            List<Trener> listaTrenera = new List<Trener>();
            foreach (string vl in vlasnik.MojiFitnesCentri)
            {
                FitnesCentar fc = FitnesCentarService.FitnessInstance.getFCIme(vl);
                foreach (string u in fc.Trener)
                {
                    Trener t = TrenerService.TrainerInstance.GetTrenerIme(u);
                    if (t != null)
                    {
                        GrupniTrening gttemp = null;
                        foreach (string g in t.GrupniTreninzi)
                        {
                            GrupniTrening gt = GrupniTreningService.GroupTrainingInstance.GetTIFCIme(g, t.FitnesCentar);
                            if (gt != null)
                            {
                                if (gt.DatumIVremeTreninga > TrenutnoVreme.Convert(DateTime.Now))
                                {
                                    gttemp = gt;
                                    break;
                                }
                            }
                        }
                        if (gttemp == null)
                            listaTrenera.Add(t);
                        else
                            listaTreneraSaBuducimTreninzima.Add(t);
                    }
                }
            }
            return (listaTrenera, listaTreneraSaBuducimTreninzima);
        }
        public void BlokirajTrenera(string ime)
        {
            Trener t = TrenerService.TrainerInstance.GetTrenerIme(ime);
            TrenerService.TrainerInstance.ObrisiTrenera(ime);
            FitnesCentarService.FitnessInstance.ObrisiTrenera(ime, t.FitnesCentar);
        }

    }
}