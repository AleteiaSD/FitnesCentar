using PR108_2017_Web_projekat.Models.Konstruktori;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PR108_2017_Web_projekat.Services
{
    public class TrenerService : BazaService
    {
        private static TrenerService ts;
        private List<Trener> listTrener = new List<Trener>();
        public static TrenerService TrainerInstance
        {
            get
            {
                if (ts == null)
                {
                    ts = new TrenerService();
                    ts.Initial("treneri.txt");
                }

                return ts;
            }
        }
        public TrenerService() : base("treneri.txt") { }
        protected override void CitajLiniju(string linija)
        {
            Trener t = Trener.TrenerParse(linija);
            listTrener.Add(t);
        }
        public List<Trener> GetAll()
        {
            List<Trener> tempList = new List<Trener>();
            foreach (Trener t in listTrener)
            {
                tempList.Add(t);
            }

            return tempList;
        }
        public void Add(Trener trener)
        {
            listTrener.Add(trener);
            DodajLiniju(new string[] { trener.ToString() });
        }
        public Trener GetTrenerIme(string ime)
        {

            foreach (Trener t in listTrener)
            {
                if (t.KorisnickoIme == ime && !t.Obrisan)
                {
                    return t;
                }
            }
            return null;
        }
        public List<Trener> UnhiredTrainers()
        {
            List<Trener> tempList = new List<Trener>();
            foreach (Trener t in listTrener)
            {
                if (t.FitnesCentar == "" || t.FitnesCentar == null)
                    tempList.Add(t);

            }
            return tempList;
        }
        public bool AzurirajTrener(Trener t)
        {
            for (int i = 0; i < listTrener.Count; i++)
            {
                if (listTrener[i].KorisnickoIme == t.KorisnickoIme)
                {
                    listTrener[i] = t;
                    IspisiSveLinije(listTrener.ConvertAll(x => x.ToString()));
                    return true;
                }
            }
            return false;
        }
        public List<string> FitnesCentriGdeCeSeOdrzavatiTreninzi(List<string> centers, List<GrupniTrening> listGT)
        {
            List<string> tempList = new List<string>();
            foreach (string s in centers)
            {
                foreach (GrupniTrening grupniTrening in listGT)
                {
                    if (grupniTrening.FitnesCentarGdeSeOdrzavaTrening == s && !grupniTrening.Obrisan && grupniTrening.DatumIVremeTreninga > TrenutnoVreme.Convert(DateTime.Now))
                    {
                        tempList.Add(s);
                        break;
                    }
                }
            }
            return tempList;
        }
        public void ObrisiTrenera(string ime)
        {
            Trener t = GetTrenerIme(ime);
            t.Obrisan = true;
            AzurirajTrener(t);
        }
    }
}