using PR108_2017_Web_projekat.Models.Konstruktori;
using PR108_2017_Web_projekat.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PR108_2017_Web_projekat.Controllers
{
    public class GrupniTreningController : Controller
    {
        // GET: GrupniTrening
        public ActionResult PridruziSe(string fcIme, string trIme)
        {
            if ((Korisnik)Session["korisnik"] == null && (KorisnikUloga)Session["uloga]"] != KorisnikUloga.POSETILAC)            
                RedirectToAction("Index", "Home");      
            Posetilac posetilac = (Posetilac)Session["korisnik"];
            if (posetilac.MojiTreninzi.Exists(x => x.NazivFitnesCentra == fcIme && x.NazivTreninga == trIme))            
                return RedirectToAction("Detalji", "Home", new { name = fcIme, ErrorJoin = "Vec ste pridruzeni!" });

            GrupniTreningService.GroupTrainingInstance.PridruziSeTreningu(fcIme, trIme, posetilac.KorisnickoIme);
            posetilac.MojiTreninzi.Add(new FitnesCentarITrening { NazivFitnesCentra = fcIme, NazivTreninga = trIme });
            PosetilacService.VisitorInstance.AzurirajPosetioca(posetilac);
            return RedirectToAction("Index", "Home");
        }
        public ActionResult KreirajGrupniTrening()
        {
            if ((Korisnik)Session["korisnik"] == null || (KorisnikUloga)Session["uloga"] != KorisnikUloga.TRENER)
                return RedirectToAction("Index", "Home");
            AddModelGrupniTrening amgt = new AddModelGrupniTrening();
            return View(amgt);
        }
        [HttpPost]
        public ActionResult KreirajGrupniTrening(AddModelGrupniTrening amgt)
        {
            if ((Korisnik)Session["korisnik"] == null || (KorisnikUloga)Session["uloga"] != KorisnikUloga.TRENER)
                return RedirectToAction("Index", "Home");

            Trener trener = (Trener)Session["korisnik"];
            bool temp = true;
            if (GrupniTreningService.GroupTrainingInstance.PostojiTrening(trener.FitnesCentar, amgt.Ime))
            {
                ViewBag.NameError = "Postoji trening!";
                temp = false;
            }
            if (TrenutnoVreme.Convert(amgt.DatumIVremeTreninga) < TrenutnoVreme.Convert(DateTime.Now.AddDays(3)))
            {
                ViewBag.DateError = "Trening mora biti za 3 dana najmanje!";
                temp = false;
            }
            if (!temp)
            {
                return View(amgt);
            }

            GrupniTrening gt = new GrupniTrening()
            {
                Naziv = amgt.Ime,
                TipTreninga = amgt.TipTreninga,
                FitnesCentarGdeSeOdrzavaTrening = trener.FitnesCentar,
                TrajanjeTreninga = amgt.TrajanjeTreninga,
                DatumIVremeTreninga = TrenutnoVreme.Convert(amgt.DatumIVremeTreninga),
                MaksimalanBrojPosetilaca = amgt.MaksimalniBrojPosetilaca,
                TrentniBrojPosetilaca = 0,                
                Trener = trener.KorisnickoIme,
            };
            GrupniTreningService.GroupTrainingInstance.Add(gt);
            return RedirectToAction("Index", "Profile");
        }
        public ActionResult ObrisiTrening(string fc, string ime)
        {
            if ((Korisnik)Session["korisnik"] == null || (KorisnikUloga)Session["uloga"] != KorisnikUloga.TRENER)
                return RedirectToAction("Index", "Home");

            GrupniTreningService.GroupTrainingInstance.ObrisiTrening(fc, ime);
            return RedirectToAction("Index", "Profile");
        }
        public ActionResult IzmeniTrening(string fc, string ime)
        {
            if ((Korisnik)Session["korisnik"] == null || (KorisnikUloga)Session["uloga"] != KorisnikUloga.TRENER)
                return RedirectToAction("Index", "Home");
            GrupniTrening gt = GrupniTreningService.GroupTrainingInstance.GetTIFCIme(ime, fc);
            GrupniTreningIzmena gti = new GrupniTreningIzmena()
            {
                Ime = gt.Naziv,
                TrajanjeTreninga = gt.TrajanjeTreninga,
                DatumIVremeTreninga = gt.DatumIVremeTreninga,
                MaksimalanBrojPosetilaca = gt.MaksimalanBrojPosetilaca                
            };
            return View(gti);
        }
        [HttpPost]
        public ActionResult IzmeniTrening(GrupniTreningIzmena gti)
        {
            if ((Korisnik)Session["korisnik"] == null || (KorisnikUloga)Session["uloga"] != KorisnikUloga.TRENER)
                return RedirectToAction("Index", "Home");
            Trener trener = (Trener)Session["korisnik"];
            DateTime dt1 = TrenutnoVreme.Convert(gti.DatumIVremeTreninga);
            DateTime dt2 = TrenutnoVreme.Convert(DateTime.Now.AddDays(1));
            if (DateTime.Compare(dt1, dt2) < 0)
            {
                ViewBag.DateError = "Ako je trening kraci od 1 dana, datum se mora pomeriti za 1 dan. ";
                return View(gti);
            }
            GrupniTrening trening = GrupniTreningService.GroupTrainingInstance.GetTIFCIme(gti.Ime, trener.FitnesCentar);
            trening.MaksimalanBrojPosetilaca = gti.MaksimalanBrojPosetilaca;
            trening.DatumIVremeTreninga = gti.DatumIVremeTreninga;
            trening.TrajanjeTreninga = gti.TrajanjeTreninga;
            GrupniTreningService.GroupTrainingInstance.AzurirajGT(trening);
            return RedirectToAction("Index", "Profile");
        }
        public ActionResult Posetioci(string fc, string trening)
        {
            List<string> listPosetilaca = GrupniTreningService.GroupTrainingInstance.GetPosetiociNaTreningu(fc, trening);
            return View(listPosetilaca);
        }
    }
}