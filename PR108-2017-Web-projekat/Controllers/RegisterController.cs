using PR108_2017_Web_projekat.Models.Konstruktori;
using PR108_2017_Web_projekat.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace PR108_2017_Web_projekat.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register
        public ActionResult Index()
        {
            RegistrationModel registration = new RegistrationModel();
            return View(registration);
        }
        [HttpPost]
        public ActionResult Index(RegistrationModel registration)
        {
            Posetilac posetilac = PosetilacService.VisitorInstance.getVisitorByUsrname(registration.KorisnickoIme);
            if (posetilac != null)
            {
                ViewBag.UsernameError = "Vec postoji korisnik sa ovim korisnickim imenom";
                return View(registration);
            }
            Posetilac p = new Posetilac {
                KorisnickoIme = registration.KorisnickoIme,
                Lozinka = registration.Lozinka,
                Ime = registration.Ime,
                Prezime = registration.Prezime,
                Pol = registration.Pol,
                Email = registration.Email,
                DatumRodjenja = TrenutnoVreme.Convert(registration.DatumRodjenja),
                Uloga = KorisnikUloga.POSETILAC
            };
            PosetilacService.VisitorInstance.Add(p);
            return RedirectToAction("Index", "Login");

        }
    }
}