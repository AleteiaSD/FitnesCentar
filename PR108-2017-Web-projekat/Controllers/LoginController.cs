using PR108_2017_Web_projekat.Models.Konstruktori;
using PR108_2017_Web_projekat.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PR108_2017_Web_projekat.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            LoginModel login = new LoginModel();
            return View(login);
        }
        [HttpPost]
        public ActionResult Index(LoginModel login)
        {
            Posetilac posetilac = PosetilacService.VisitorInstance.getVisitorByUsrname(login.KorisnickoIme);
            if (posetilac != null)
            {
                if (posetilac.Lozinka != login.Lozinka)
                {
                    ViewBag.PasswordError = "Pogresna lozinka.";
                    return View("Index", login);
                }
                Session["korisnik"] = posetilac;
                Session["uloga"] = posetilac.Uloga;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Trener trener = TrenerService.TrainerInstance.GetTrenerIme(login.KorisnickoIme);
                if (trener != null)
                {
                    if (trener.Lozinka != login.Lozinka)
                    {
                        ViewBag.PasswordError = "Pogresna lozinka.";
                        return View("Index", login);
                    }
                    Session["korisnik"] = trener;
                    Session["uloga"] = trener.Uloga;
                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    Vlasnik vlasnik = VlasnikService.VlasnikInstance.GetVlasnikIme(login.KorisnickoIme);
                    if (vlasnik != null)
                    {
                        if (vlasnik.Lozinka != login.Lozinka)
                        {
                            ViewBag.PasswordError = "Pogresna lozinka.";
                            return View("Index", login);
                        }
                        Session["korisnik"] = vlasnik;
                        Session["uloga"] = vlasnik.Uloga;
                        return RedirectToAction("Index", "Home");

                    }
                }
            }
            ViewBag.UsernameError = "Korisnik sa ovim korisnickim imenom ne postoji.";
            return View(login);
        }

        public ActionResult IzlogujSe()
        {
            Session["korisnik"] = null;
            Session["uloga"] = null;
            return RedirectToAction("Index", "Home");
        }
    }
}