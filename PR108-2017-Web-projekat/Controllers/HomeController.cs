using PR108_2017_Web_projekat.Models.Konstruktori;
using PR108_2017_Web_projekat.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PR108_2017_Web_projekat.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<FitnesCentar> fc = FitnesCentarService.FitnessInstance.GetAll();

            if ((Korisnik)Session["korisnik"] != null && (KorisnikUloga)Session["uloga"] == KorisnikUloga.VLASNIK)
            {
                Vlasnik vlasnik = (Vlasnik)Session["korisnik"];
                List<FitnesCentar> listaFC = new List<FitnesCentar>();
                foreach (string temp in vlasnik.MojiFitnesCentri)
                {
                    FitnesCentar fctemp = FitnesCentarService.FitnessInstance.getFCIme(temp);
                    if (fctemp != null)
                        listaFC.Add(fctemp);
                }
                listaFC = listaFC.OrderBy(x => x.Naziv).ToList();
                return View(listaFC);
            }
            fc = fc.OrderBy(x => x.Naziv).ToList();
            return View(fc);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult Pretraga(FCPretraga pretraga)
        {

            List<FitnesCentar> temp = FitnesCentarService.FitnessInstance.GetAll();
            if (temp == null)
                return View();

            temp = FitnesCentarService.FitnessInstance.Pretraga(pretraga, temp);
            temp = FitnesCentarService.FitnessInstance.Sortiranje(pretraga.sortiraj, temp);
            return View("Index", temp);

        }
        public ActionResult Detalji(string ime, string greskaUlaz = null, string greskaKomentar = null, string nemaKomentara = null)
        {
            ViewBag.CommentError = greskaKomentar;
            ViewBag.CantCommError = nemaKomentara;

            ViewBag.ErrorJoin = greskaUlaz;
            FCDetalji detalji = new FCDetalji();
            detalji.FCNaziv = FitnesCentarService.FitnessInstance.getFCIme(ime);
            (detalji.Odobren, detalji.Odbijen) = KomentarService.CommentInstance.GetKomentarZaFC(ime);

            if (detalji.FCNaziv == null)
                return RedirectToAction("Index");


            List<GrupniTrening> temp = GrupniTreningService.GroupTrainingInstance.GetTreningZaFC(ime);
            detalji.listGrupniTreninzi = temp.Where(x => x.DatumIVremeTreninga > TrenutnoVreme.Convert(DateTime.Now)).ToList();
            return View(detalji);
        }
    }
}