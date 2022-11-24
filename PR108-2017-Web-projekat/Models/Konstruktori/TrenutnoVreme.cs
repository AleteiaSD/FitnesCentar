using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace PR108_2017_Web_projekat.Models.Konstruktori
{
    public static class TrenutnoVreme
    {
        public static DateTime Convert(DateTime vreme)
        {
            string datum = vreme.ToString("dd/MM/yyyy HH:mm");
            DateTime.TryParseExact(datum, "dd/MM/yyyy HH:mm", new CultureInfo("en-US", true), System.Globalization.DateTimeStyles.None, out DateTime datumIVreme);
            return datumIVreme;
        }
    }
}
/*
            string formatDatuma = "";
            string[] splitovanDatum = datum.Split(delimeters);
            if (splitovanDatum[1].Length > 1)
                formatDatuma = splitovanDatum[2] + "/" + splitovanDatum[1] + "/" + splitovanDatum[0];
            else
                formatDatuma = splitovanDatum[2] + "/0" + splitovanDatum[1] + "/" + splitovanDatum[0];
                */
