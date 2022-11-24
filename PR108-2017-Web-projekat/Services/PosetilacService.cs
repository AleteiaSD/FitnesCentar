using PR108_2017_Web_projekat.Models.Konstruktori;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PR108_2017_Web_projekat.Services
{
    public class PosetilacService :BazaService
    {
        private static PosetilacService ps;

        private List<Posetilac> listPosetilac = new List<Posetilac>();

        public static PosetilacService VisitorInstance
        {
            get
            {
                if (ps == null)
                {
                    ps = new PosetilacService();
                    ps.Initial("posetioci.txt");
                }

                return ps;
            }
        }
        public PosetilacService() : base("posetioci.txt") { }
        protected override void CitajLiniju (string linija)
        {
            Posetilac pos = Posetilac.PosetilacParse(linija);
            listPosetilac.Add(pos);
        }
        public List<Posetilac> GetAll()
        {
            List<Posetilac> tempList = new List<Posetilac>();
            foreach (Posetilac p in listPosetilac)
            {
                tempList.Add(p);
            }

            return tempList;
        }
        public void Add(Posetilac posetilac)
        {
            listPosetilac.Add(posetilac);
            DodajLiniju(new string[] { posetilac.ToString() });
        }
        public Posetilac getVisitorByUsrname(string ime)
        {

            foreach (Posetilac p in listPosetilac)
            {
                if (p.KorisnickoIme == ime && !p.Obrisan)
                {
                    return p;
                }
            }
            return null;
        }
        public void AzurirajPosetioca(Posetilac posetilac)
        {
            for (int i = 0; i < listPosetilac.Count; i++)
            {
                if (listPosetilac[i].KorisnickoIme == posetilac.KorisnickoIme)
                {
                    listPosetilac[i] = posetilac;
                    IspisiSveLinije(listPosetilac.ConvertAll(x => x.ToString()));
                }
            }
        }
    }
}