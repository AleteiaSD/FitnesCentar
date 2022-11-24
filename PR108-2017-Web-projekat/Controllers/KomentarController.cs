using PR108_2017_Web_projekat.Models.Konstruktori;
using PR108_2017_Web_projekat.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PR108_2017_Web_projekat.Controllers
{
    public class KomentarController : Controller
    {
        // GET: Komentar
        public ActionResult DodajKomentar(string fitnescentar)
        {
            Posetilac posetilac = (Posetilac)Session["korisnik"];
            if (posetilac == null || posetilac.Uloga != KorisnikUloga.POSETILAC)
                return RedirectToAction("Index", "Home");
            if (KomentarService.CommentInstance.GetKomentar(fitnescentar, posetilac.KorisnickoIme) != null)
                return RedirectToAction("Detalji", "Home", new { name = fitnescentar, CommentError = "Komentarisano vec" });

            List<GrupniTrening> listaGT = GrupniTreningService.GroupTrainingInstance.GetTreningZaPosetioce(posetilac.KorisnickoIme);
            bool temp = false;
            foreach (GrupniTrening gt in listaGT)
            {
                if (gt.FitnesCentarGdeSeOdrzavaTrening == fitnescentar && 
                    gt.DatumIVremeTreninga < TrenutnoVreme.Convert(DateTime.Now))
                    temp = true;
            }
            if (!temp)
                return RedirectToAction("Detalji", "Home", new { name = fitnescentar, cantComm = "Niste pokusali" });

            ViewBag.FitnesCentar = fitnescentar;
            return View();
        }
        [HttpPost]
        public ActionResult DodajKomentar(AddKomentarModel akm)
        {
            Posetilac v = (Posetilac)Session["korisnik"];
            if (v == null || v.Uloga != KorisnikUloga.POSETILAC)
                return RedirectToAction("Index", "Home");
            Komentar c = new Komentar()
            {
                FitnesCentarNaKojiSeKomentarOdnosi = akm.FitnesCentar,
                Ocena = akm.Ocena,
                TekstKomentara = akm.Komentar,
                PosetilacKojiJeOstavioKomentar = v.KorisnickoIme
            };
            KomentarService.CommentInstance.Add(c);
            return RedirectToAction("Detalji", "Home", new { name = akm.FitnesCentar });

        }
        public ActionResult KomentarUpit()
        {
            Vlasnik vlasnik = (Vlasnik)Session["korisnik"];
            if (vlasnik == null || vlasnik.Uloga != KorisnikUloga.VLASNIK)
                return RedirectToAction("Index", "Home");
            List<Komentar> listKom = KomentarService.CommentInstance.KomentarKojiJeOdobrioVlasnik(vlasnik);
            return View(listKom);
        }
        public ActionResult OdbijenKomentar(string posetilac, string fitnescentar)
        {
            KomentarService.CommentInstance.OdbijenKomentar(posetilac, fitnescentar);
            return RedirectToAction("KomentarUpit");
        }
        public ActionResult OdobrenKomentar(string posetilac, string fitnescentar)
        {
            KomentarService.CommentInstance.OdobrenKomentar(posetilac, fitnescentar);
            return RedirectToAction("KomentarUpit");
        }
    }
}