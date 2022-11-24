using PR108_2017_Web_projekat.Models.Konstruktori;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PR108_2017_Web_projekat.Services
{
    public class GrupniTreningService : BazaService
    {
        private static GrupniTreningService gts;

        private List<GrupniTrening> gtList = new List<GrupniTrening>();
        public static GrupniTreningService GroupTrainingInstance
        {
            get
            {
                if (gts == null)
                {
                    gts = new GrupniTreningService();
                    gts.Initial("grupniTreninzi.txt");
                }

                return gts;
            }
        }
        public GrupniTreningService() : base("grupniTreninzi.txt") { }
        protected override void CitajLiniju(string linija)
        {
            GrupniTrening gt = GrupniTrening.GrupniTreningParse(linija);
            gtList.Add(gt);
        }
        public List<GrupniTrening> GetAll()
        {
            List<GrupniTrening> temp = new List<GrupniTrening>();
            foreach (GrupniTrening gt in gtList)
            {
                if (!gt.Obrisan)
                    temp.Add(gt);
            }
            return temp;
        }
        public void Add(GrupniTrening gt)
        {
            gtList.Add(gt);
            DodajLiniju(new string[] { gt.ToString() });
        }
        public GrupniTrening GetTIFCIme(string trening, string fc)
        {
            foreach (GrupniTrening gt in gtList)
            {
                if (gt.Naziv == trening && gt.FitnesCentarGdeSeOdrzavaTrening == fc && !gt.Obrisan) return gt;                
            }
            return null;
        }
        public bool PostojiTrening(string fc, string imeTreninga)
        {
            foreach (GrupniTrening gt in gtList)
            {
                if (!gt.Obrisan && gt.FitnesCentarGdeSeOdrzavaTrening == fc && gt.Naziv == imeTreninga)
                {
                    return true;
                }
            }
            return false;
        }
        public List<GrupniTrening> GetTreningZaFC(string fc)
        {
            List<GrupniTrening> temp = new List<GrupniTrening>();
            foreach (GrupniTrening gt in gtList)
            {
                if (gt.FitnesCentarGdeSeOdrzavaTrening == fc && !gt.Obrisan)
                    temp.Add(gt);
            }
            return temp;
        }
        public List<GrupniTrening> GetTreningZaPosetioce(string ime)
        {
            List<GrupniTrening> gt = new List<GrupniTrening>();
            foreach (GrupniTrening gttemp in gtList)
            {
                if (gttemp.SpisakPosetilaca.Contains(ime) && !gttemp.Obrisan)
                    gt.Add(gttemp);
            }
            return gt;

        }
        public bool AzurirajGT(GrupniTrening gt)
        {
            for (int i = 0; i < gtList.Count; i++)
            {
                if (gtList[i].Naziv == gt.Naziv && gtList[i].FitnesCentarGdeSeOdrzavaTrening == gt.FitnesCentarGdeSeOdrzavaTrening)
                {
                    gtList[i] = gt;
                    IspisiSveLinije(gtList.ConvertAll(x => x.ToString()));
                    return true;
                }
            }
            return false;
        }
        public bool PridruziSeTreningu(string fc, string imeTreninga, string posetilac)
        {
            GrupniTrening gt = gtList.Find(x => x.FitnesCentarGdeSeOdrzavaTrening == fc && x.Naziv == imeTreninga);
            gt.TrentniBrojPosetilaca++;
            gt.SpisakPosetilaca.Add(posetilac);
            return AzurirajGT(gt);
        }
        public void ObrisiTrening(string fc, string ime)
        {
            foreach (GrupniTrening gt in gtList)
            {
                if (gt.FitnesCentarGdeSeOdrzavaTrening == fc && gt.Naziv == ime)
                {
                    gt.Obrisan = true;
                    AzurirajGT(gt);
                    break;
                }
            }
        }
        public List<string> GetPosetiociNaTreningu(string fc, string trening)
        {
            List<string> listPosetilac = new List<string>();
            foreach (GrupniTrening gt in gtList)
            {
                if (gt.FitnesCentarGdeSeOdrzavaTrening == fc && gt.Naziv == trening)
                {
                    foreach (string s in gt.SpisakPosetilaca)
                        listPosetilac.Add(s);
                    break;
                }
            }
            return listPosetilac;
        }
        public List<GrupniTrening> PretragaGT(PretragaProsliTreninzi pretraga, List<GrupniTrening> listTrening)
        {
            if (string.IsNullOrWhiteSpace(pretraga.Ime) && 
                string.IsNullOrWhiteSpace(pretraga.TipTreninga.ToString()) &&
                string.IsNullOrWhiteSpace(pretraga.FitnesCentar))
                return listTrening;

            if (!string.IsNullOrWhiteSpace(pretraga.Ime))
            {
                listTrening = listTrening.Where(a => a.Naziv.ToLower().Contains(pretraga.Ime.ToLower())).ToList();
            }
            if (!string.IsNullOrWhiteSpace(pretraga.FitnesCentar))
            {
                listTrening = listTrening.Where(a => a.FitnesCentarGdeSeOdrzavaTrening.ToLower().Contains(pretraga.FitnesCentar.ToLower())).ToList();
            }
            if (!string.IsNullOrWhiteSpace(pretraga.TipTreninga.ToString()))
                listTrening = listTrening.Where(a => a.TipTreninga.ToString().ToLower().Contains(pretraga.TipTreninga.ToString().ToLower())).ToList();

            return listTrening;
        }
        public List<GrupniTrening> PretragaGTTrener(PretragaProsliTreninziTrener pretraga, List<GrupniTrening> listTrening)
        {
            if (string.IsNullOrWhiteSpace(pretraga.Ime) &&
                string.IsNullOrWhiteSpace(pretraga.TipTreninga.ToString()) && 
                pretraga.OdDatuma.ToShortDateString() == new DateTime(1, 1, 0001).ToShortDateString() &&
                pretraga.DoDatuma.ToShortDateString() == new DateTime(1, 1, 0001).ToShortDateString())
                return listTrening;

            if (!string.IsNullOrWhiteSpace(pretraga.Ime))
            {
                listTrening = listTrening.Where(a => a.Naziv.ToLower().Contains(pretraga.Ime.ToLower())).ToList();
            }
            if (!string.IsNullOrWhiteSpace(pretraga.TipTreninga.ToString()))
                listTrening = listTrening.Where(a => a.TipTreninga.ToString().ToLower().Contains(pretraga.TipTreninga.ToString().ToLower())).ToList();
            if (pretraga.OdDatuma.ToShortDateString() != new DateTime(1, 1, 0001).ToShortDateString())
            {
                listTrening = listTrening.Where(a => a.DatumIVremeTreninga > pretraga.OdDatuma).ToList();
            }
            if (pretraga.DoDatuma.ToShortDateString() != new DateTime(1, 1, 0001).ToShortDateString())
            {
                listTrening = listTrening.Where(a => a.DatumIVremeTreninga < pretraga.DoDatuma).ToList();
            }
            return listTrening;
        }
        public List<GrupniTrening> GetSviPrethodniTreninziZaPosetioce(string ime)
        {
            List<GrupniTrening> gt = new List<GrupniTrening>();
            foreach (GrupniTrening gttemp in gtList)
            {
                if (gttemp.SpisakPosetilaca.Contains(ime) && 
                    gttemp.DatumIVremeTreninga < TrenutnoVreme.Convert(DateTime.Now) && 
                    !gttemp.Obrisan)
                    gt.Add(gttemp);
            }
            return gt;
        }
        public List<GrupniTrening> SortiranjeGT(string sortiranje, List<GrupniTrening> listGT)
        {
            if (sortiranje == "ImeRastuce")
                listGT = listGT.OrderBy(x => x.Naziv).ToList();
            else if (sortiranje == "ImeOpadajuce")
                listGT = listGT.OrderByDescending(x => x.Naziv).ToList();
            else if (sortiranje == "TipTreningaRastuce")
                listGT = listGT.OrderBy(x => x.TipTreninga).ToList();
            else if (sortiranje == "TipTreningaOpadajuce")
                listGT = listGT.OrderByDescending(x => x.TipTreninga).ToList();
            else if (sortiranje == "DatumRastuce")
                listGT = listGT.OrderBy(x => x.DatumIVremeTreninga).ToList();
            else if (sortiranje == "DatumOpadajuce")
                listGT = listGT.OrderByDescending(x => x.DatumIVremeTreninga).ToList();
            return listGT;
        }
        public List<GrupniTrening> GetNaredniTreninziTrenera(string ime)
        {
            List<GrupniTrening> tempList = new List<GrupniTrening>();
            foreach (GrupniTrening gt in gtList)
            {
                if (gt.Trener == ime &&
                    gt.DatumIVremeTreninga > TrenutnoVreme.Convert(DateTime.Now) &&
                    !gt.Obrisan)
                    tempList.Add(gt);
            }
            return tempList;
        }
        public List<GrupniTrening> GetProsliTreninziTrenera(string ime)
        {
            List<GrupniTrening> tempList = new List<GrupniTrening>();
            foreach (GrupniTrening gt in gtList)
            {
                if (gt.Trener == ime &&
                    gt.DatumIVremeTreninga < TrenutnoVreme.Convert(DateTime.Now) &&
                    !gt.Obrisan)
                    tempList.Add(gt);
            }
            return tempList;
        }
    }
}