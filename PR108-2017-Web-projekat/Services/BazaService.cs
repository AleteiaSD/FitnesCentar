using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace PR108_2017_Web_projekat.Services
{
    public abstract class BazaService
    {
        private readonly string ime;
        public BazaService(string ime)
        {
            this.ime = ime;
        }
        protected void IspisiSveLinije(IEnumerable<string> linije, string imetemp = null)
        {
            using (FileStream stream = new FileStream(GetPutanja(imetemp ?? ime), FileMode.Create))
            using (StreamWriter sr = new StreamWriter(stream))
            {
                foreach (var linija in linije)
                {
                    sr.WriteLine(linija);
                }
            }
        }
        protected void DodajLiniju(IEnumerable<string> linije)
        {
            using (FileStream stream = new FileStream(GetPutanja(ime), FileMode.Append))
            using (StreamWriter sw = new StreamWriter(stream))
            {
                foreach (var linija in linije)
                {
                    sw.WriteLine(linija);
                }
            }
        }
        protected abstract void CitajLiniju(string linija);
        private string GetPutanja(string ime)
        {
            string putanja = Path.Combine(HttpRuntime.AppDomainAppPath, "App_Data", ime);
            return putanja;
        }        
        protected void Initial(string drugoime = null)
        {
            using (FileStream stream = new FileStream(GetPutanja(drugoime ?? ime), FileMode.Open))
            using (StreamReader sr = new StreamReader(stream)){
                string linija;
                while ((linija = sr.ReadLine()) != null){
                    CitajLiniju(linija);
                }
            }
        }
    }
}