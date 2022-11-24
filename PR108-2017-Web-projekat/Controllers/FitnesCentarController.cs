using PR108_2017_Web_projekat.Models.Konstruktori;
using PR108_2017_Web_projekat.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PR108_2017_Web_projekat.Controllers
{
    public class FitnesCentarController : Controller
    {
        // GET: FitnesCentar
        public ActionResult DodajTrenera()
        {
            if ((Korisnik)Session["korisnik"] == null ||
                (KorisnikUloga)Session["uloga"] != KorisnikUloga.VLASNIK ||
                (KorisnikUloga)Session["uloga"] != KorisnikUloga.VLASNIK)
                return RedirectToAction("Index", "Home");
            List<Trener> trenerList = TrenerService.TrainerInstance.UnhiredTrainers();
            return View(trenerList);
        }
        public ActionResult Dodaj(string ime)
        {
            if ((Korisnik)Session["korisnik"] == null ||
                (KorisnikUloga)Session["uloga"] != KorisnikUloga.VLASNIK ||
                (KorisnikUloga)Session["uloga"] != KorisnikUloga.VLASNIK)
                return RedirectToAction("Index", "Home");
            ViewBag.FitnesCentar = ime;
            List<Trener> trenerList = TrenerService.TrainerInstance.UnhiredTrainers();
            return View("DodajTrenera", trenerList);
        }
        public ActionResult ZaposliTrenera(string ime, string fcc)
        {
            if ((Korisnik)Session["korisnik"] == null || 
                (KorisnikUloga)Session["uloga"] != KorisnikUloga.VLASNIK || 
                (KorisnikUloga)Session["uloga"] != KorisnikUloga.VLASNIK)
                RedirectToAction("Index", "Home");
            FitnesCentar fc = FitnesCentarService.FitnessInstance.getFCIme(fcc);
            fc.Trener.Add(ime);
            FitnesCentarService.FitnessInstance.AzuriranjeFC(fc);
            Trener trener = TrenerService.TrainerInstance.GetTrenerIme(ime);
            trener.FitnesCentar = fcc;
            TrenerService.TrainerInstance.AzurirajTrener(trener);
            return RedirectToAction("Index", "Profile");
        }
        public ActionResult MojiFitnesCentri()
        {
            if ((Korisnik)Session["korisnik"] == null ||
                (KorisnikUloga)Session["uloga"] != KorisnikUloga.VLASNIK ||
                (KorisnikUloga)Session["uloga"] != KorisnikUloga.VLASNIK)
                return RedirectToAction("Index", "Home");

            Vlasnik vlasnik = (Vlasnik)Session["korisnik"];
            List<GrupniTrening> gtList = GrupniTreningService.GroupTrainingInstance.GetAll();
            ViewBag.HasGroup = TrenerService.TrainerInstance.FitnesCentriGdeCeSeOdrzavatiTreninzi(vlasnik.MojiFitnesCentri, gtList);
            return View(FitnesCentarService.FitnessInstance.getVlasnikFC(vlasnik.KorisnickoIme));
        }
        public ActionResult ObrisiFitnesCentar(string ime)
        {
            if ((Korisnik)Session["korisnik"] == null ||
                (KorisnikUloga)Session["uloga"] != KorisnikUloga.VLASNIK ||
                (KorisnikUloga)Session["uloga"] != KorisnikUloga.VLASNIK)
                return RedirectToAction("Index", "Home");
            Vlasnik vlasnik = (Vlasnik)Session["korisnik"];
            VlasnikService.VlasnikInstance.ObrisiFC(vlasnik, ime);
            return RedirectToAction("MojiFitnesCentri");
        }
        public ActionResult KreirajFitnesCentar()
        {
            if ((Korisnik)Session["korisnik"] == null ||
                (KorisnikUloga)Session["uloga"] != KorisnikUloga.VLASNIK)
                return RedirectToAction("Index", "Home");
            FitnesCentarKreiranje fc = new FitnesCentarKreiranje() { };
            return View(fc);
        }
        [HttpPost]
        public ActionResult KreirajFitnesCentar(FitnesCentarKreiranje fccm)
        {
            if ((Korisnik)Session["korisnik"] == null ||
                (KorisnikUloga)Session["uloga"] != KorisnikUloga.VLASNIK ||
                (KorisnikUloga)Session["uloga"] != KorisnikUloga.VLASNIK)
                return RedirectToAction("Index", "Home");
            bool temp = true;
            Vlasnik vlasnik = (Vlasnik)Session["korisnik"];
            if (FitnesCentarService.FitnessInstance.DaLiPostoji(fccm.Naziv))
            {
                ViewBag.NameError = "Postoji FC";
                temp = false;
            }
            int i = 0;
            foreach (char c in fccm.PostanskiBroj.ToString())
            {
                i++;
            }
            if (i > 5 || i < 5)
            {
                ViewBag.PostalNumError = "Postanski broj mora imati 5 cifara";
                temp = false;
            }
            if (!temp)
                return View(fccm);

            AdresaFitnesCentra adresa = new AdresaFitnesCentra();
            adresa.Ulica = fccm.Ulica;
            adresa.Broj = fccm.Broj;
            adresa.MestoGrad = fccm.MestoGrad;
            adresa.PostanskiBroj = fccm.PostanskiBroj;
            FitnesCentar fc = new FitnesCentar()
            {
                Naziv = fccm.Naziv,
                Adresa = adresa,
                GodinaOtvaranja = fccm.GodinaOtvaranja,
                Vlasnik = vlasnik.KorisnickoIme,
                CenaMesecneClanarine = fccm.CenaMesecneClanarine,
                CenaGodisnjeClanarine = fccm.CenaGodisnjeClanarine,
                CenaJednogTreninga = fccm.CenaJednogTreninga,
                CenaJednogGrupnogTreninga = fccm.CenaJednogGrupnogTreninga,
                CenaJednogTreningaSaPersonalnimTrenerom = fccm.CenaJednogTreningaSaPersonalnimTrenerom,
                Obrisan = false,

            };
            VlasnikService.VlasnikInstance.KreirajFC(fc, vlasnik);
            return RedirectToAction("MojiFitnesCentri");
        }

        public ActionResult InformacijeOTreneru()
        {
            if ((Korisnik)Session["korisnik"] == null ||
                (KorisnikUloga)Session["uloga"] != KorisnikUloga.VLASNIK ||
                (KorisnikUloga)Session["uloga"] != KorisnikUloga.VLASNIK)
                return RedirectToAction("Index", "Home");
            Vlasnik vlasnik = (Vlasnik)Session["korisnik"];
            (List<Trener>, List<Trener>) trener = VlasnikService.VlasnikInstance.GetVlasniciTrenera(vlasnik);
            InformacijeOTreneru tim = new InformacijeOTreneru() { Obrisan = trener.Item1, NijeObrisan = trener.Item2 };


            return View(tim);

        }
        public ActionResult BlokirajTrenera(string ime)
        {
            VlasnikService.VlasnikInstance.BlokirajTrenera(ime);
            return RedirectToAction("InformacijeOTreneru");
        }
        public ActionResult IzmeniFitnesCentar(string ime)
        {
            if ((Korisnik)Session["korisnik"] == null ||
                 (KorisnikUloga)Session["uloga"] != KorisnikUloga.VLASNIK ||
                 (KorisnikUloga)Session["uloga"] != KorisnikUloga.VLASNIK)
                return RedirectToAction("Index", "Home");
            FitnesCentar fc = FitnesCentarService.FitnessInstance.getFCIme(ime);

            FitnesCentarIzmena fci = new FitnesCentarIzmena()
            {
                Naziv = fc.Naziv,
                GodinaOtvaranja = fc.GodinaOtvaranja,

                Ulica = fc.Adresa.Ulica,
                Broj = fc.Adresa.Broj,
                MestoGrad = fc.Adresa.MestoGrad,
                PostanskiBroj = fc.Adresa.PostanskiBroj,

                CenaMesecneClanarine = fc.CenaMesecneClanarine,
                CenaGodisnjeClanarine = fc.CenaGodisnjeClanarine,  
                CenaJednogTreninga = fc.CenaJednogTreninga,
                CenaJednogGrupnogTreninga = fc.CenaJednogGrupnogTreninga,
                CenaJednogTreningaSaPersonalnimTrenerom = fc.CenaJednogTreningaSaPersonalnimTrenerom,                

                StaroIme = fc.Naziv
            };
            return View(fci);
        }
        [HttpPost]
        public ActionResult IzmeniFitnesCentar(FitnesCentarIzmena fci)
        {
            if ((Korisnik)Session["korisnik"] == null ||
                (KorisnikUloga)Session["uloga"] != KorisnikUloga.VLASNIK ||
                (KorisnikUloga)Session["uloga"] != KorisnikUloga.VLASNIK)
                return RedirectToAction("Index", "Home");
            bool temp = true;
            Vlasnik vlasnik = (Vlasnik)Session["korisnik"];
            FitnesCentar f = FitnesCentarService.FitnessInstance.getFCIme(fci.Naziv);
            if (FitnesCentarService.FitnessInstance.DaLiPostoji(fci.Naziv))
            {
                if (f.Naziv != fci.Naziv)
                {
                    ViewBag.NameError = "Postoji fc sa tim nazivom";
                    temp = false;
                }
            }
            int i = 0;
            foreach (char c in fci.PostanskiBroj.ToString())
            {
                i++;
                if (i > 5)
                    break;
            }
            if (i < 5 || i > 5)
            {
                ViewBag.PostalNumError = "Postanski broj mora imati 5 cifara";
                temp = false;
            }

            if (!temp)
                return View(fci);

            AdresaFitnesCentra adresa = new AdresaFitnesCentra();
            adresa.Ulica = fci.Ulica;
            adresa.Broj = fci.Broj;
            adresa.MestoGrad = fci.MestoGrad;
            adresa.PostanskiBroj = fci.PostanskiBroj;
            FitnesCentar fc = new FitnesCentar()
            {
                Naziv = fci.Naziv,
                Adresa = adresa,
                GodinaOtvaranja = fci.GodinaOtvaranja,
                Vlasnik = vlasnik.KorisnickoIme,

                CenaMesecneClanarine = fci.CenaMesecneClanarine,
                CenaGodisnjeClanarine = fci.CenaGodisnjeClanarine,
                CenaJednogTreninga = fci.CenaJednogTreninga,
                CenaJednogGrupnogTreninga = fci.CenaJednogGrupnogTreninga,
                CenaJednogTreningaSaPersonalnimTrenerom = fci.CenaJednogTreningaSaPersonalnimTrenerom,
                
                Trener = f.Trener,
                Obrisan = false

            };
            FitnesCentarService.FitnessInstance.AzuriranjeFC(fc);
            return RedirectToAction("MojiFitnesCentri");
        }
    }
}