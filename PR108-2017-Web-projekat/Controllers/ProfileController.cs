using PR108_2017_Web_projekat.Models.Konstruktori;
using PR108_2017_Web_projekat.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PR108_2017_Web_projekat.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        public ActionResult Index()
        {
            if ((Korisnik)Session["korisnik"] == null)
                return RedirectToAction("Index", "Home");

            Korisnik korisnik = (Korisnik)Session["korisnik"];
            if (korisnik.Uloga == KorisnikUloga.POSETILAC)
            {
                Posetilac posetilac = (Posetilac)Session["korisnik"];
                ViewBag.Model = posetilac;
                List<GrupniTrening> gtList = GrupniTreningService.GroupTrainingInstance.GetTreningZaPosetioce(korisnik.KorisnickoIme);
                List<GrupniTrening> temp = gtList.Where(x => x.DatumIVremeTreninga > DateTime.Now).ToList();
                return View("Posetilac", temp);
            }
            else if (korisnik.Uloga == KorisnikUloga.VLASNIK)
            {
                Vlasnik vlasnik = (Vlasnik)Session["korisnik"];
                ViewBag.Model = vlasnik;
                List<FitnesCentar> fcList = FitnesCentarService.FitnessInstance.getVlasnikFC(vlasnik.KorisnickoIme);
                return View("Vlasnik", fcList);
            }
            else
            {
                Trener trener = (Trener)Session["korisnik"];
                ViewBag.Model = trener;
                List<GrupniTrening> gtList = GrupniTreningService.GroupTrainingInstance.GetNaredniTreninziTrenera(trener.KorisnickoIme);
                return View("Trener", gtList);
            }
        }
        public ActionResult ProsliTreninzi()
        {
            if ((Korisnik)Session["korisnik"] == null)            
                return RedirectToAction("Index", "Home");            
            Posetilac posetilac = (Posetilac)Session["korisnik"];
            ViewBag.Model = posetilac;
            List<GrupniTrening> gtList = GrupniTreningService.GroupTrainingInstance.GetTreningZaPosetioce(posetilac.KorisnickoIme);
            List<GrupniTrening> tempList = gtList.Where(x => x.DatumIVremeTreninga < DateTime.Now).ToList();
            return View("ProsliTreninzi", tempList);
        }
        [HttpPost]
        public ActionResult PretragaProslihTreninga(PretragaProsliTreninzi ppt)
        {
            if ((Korisnik)Session["korisnik"] == null)            
                return RedirectToAction("Index", "Home");            
            Posetilac posetilac = (Posetilac)Session["korisnik"];
            ViewBag.Model = posetilac;
            List<GrupniTrening> gtList = GrupniTreningService.GroupTrainingInstance.GetTreningZaPosetioce(posetilac.KorisnickoIme);
            gtList = gtList.Where(x => x.DatumIVremeTreninga < TrenutnoVreme.Convert(DateTime.Now)).ToList();
            gtList = GrupniTreningService.GroupTrainingInstance.PretragaGT(ppt, gtList);
            return View("ProsliTreninzi", gtList);
        }
        [HttpPost]
        public ActionResult PretragaProslihTreningaTrenera(PretragaProsliTreninziTrener pptt)
        {
            if ((Korisnik)Session["korisnik"] == null)            
                return RedirectToAction("Index", "Home");            
            Trener trener = (Trener)Session["korisnik"];
            List<GrupniTrening> gtList = GrupniTreningService.GroupTrainingInstance.GetProsliTreninziTrenera(trener.KorisnickoIme);
            gtList = GrupniTreningService.GroupTrainingInstance.PretragaGTTrener(pptt, gtList);
            return View("ProsliTreninziTrenera", gtList);
        }
        public ActionResult SortiranjeProslihTreninga(string sortiranje)
        {
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            if (korisnik == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<GrupniTrening> gtList = GrupniTreningService.GroupTrainingInstance.GetSviPrethodniTreninziZaPosetioce(korisnik.KorisnickoIme);
            gtList = GrupniTreningService.GroupTrainingInstance.SortiranjeGT(sortiranje, gtList);
            return View("ProsliTreninzi", gtList);
        }
        public ActionResult SortiranjeProslihTreningaTrenera(string sortiranje)
        {
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            if (korisnik == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<GrupniTrening> gtList = GrupniTreningService.GroupTrainingInstance.GetProsliTreninziTrenera(korisnik.KorisnickoIme);
            gtList = GrupniTreningService.GroupTrainingInstance.SortiranjeGT(sortiranje, gtList);
            return View("ProsliTreninziTrenera", gtList);
        }
        public ActionResult Izmeni()
        {
            Korisnik korisnik = (Korisnik)Session["korisnik"];
            if (korisnik == null)
                return RedirectToAction("Index", "Home");
            ProfileModel profileinfo = new ProfileModel() { Ime = korisnik.Ime, Prezime = korisnik.Prezime, Lozinka = korisnik.Lozinka };
            return View(profileinfo);
        }
        [HttpPost]
        public ActionResult Izmeni(ProfileModel pm)
        {
            if ((Korisnik)Session["korisnik"] == null)
                return RedirectToAction("Index", "Home");


            if ((KorisnikUloga)Session["uloga"] == KorisnikUloga.POSETILAC)
            {
                Posetilac posetilac = (Posetilac)Session["korisnik"];
                posetilac.Ime = pm.Ime;
                posetilac.Prezime = pm.Prezime;
                posetilac.Lozinka = pm.Lozinka;
                PosetilacService.VisitorInstance.AzurirajPosetioca(posetilac);
            }
            else if ((KorisnikUloga)Session["uloga"] == KorisnikUloga.VLASNIK)
            {
                Vlasnik vlasnik = (Vlasnik)Session["korisnik"];
                vlasnik.Ime = pm.Ime;
                vlasnik.Prezime = pm.Prezime;
                vlasnik.Lozinka = pm.Lozinka;
                VlasnikService.VlasnikInstance.AzurirajVlasnika(vlasnik);
            }
            else
            {
                Trener t = (Trener)Session["korisnik"];
                t.Ime = pm.Ime;                
                t.Prezime = pm.Prezime;
                t.Lozinka = pm.Lozinka;
                TrenerService.TrainerInstance.AzurirajTrener(t);
            }
            return RedirectToAction("Index");


        }
        public ActionResult ProsliTreninziTrenera()
        {
            if ((Korisnik)Session["korisnik"] == null && (KorisnikUloga)Session["uloga"] != KorisnikUloga.TRENER)
            {
                return RedirectToAction("Index", "Home");
            }
            Trener trener = (Trener)Session["korisnik"];
            List<GrupniTrening> gtList = GrupniTreningService.GroupTrainingInstance.GetProsliTreninziTrenera(trener.KorisnickoIme);
            return View("ProsliTreninziTrenera", gtList);
        }
    }
}