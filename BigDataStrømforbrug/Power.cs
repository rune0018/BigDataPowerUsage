using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BigDataStrømforbrug
{
    internal class Power
    {
        // energital pr person i GJ 125 på år (ca. 340000 kw( 95 kw pr dag)) konklusion pr statestikker en alm. dansker bruger 300000 til 500000kr pr år sauce: https://ens.dk/service/statistik-data-noegletal-og-kort/noegletal-og-internationale-indberetninger
        // 4.3 kwh pr dag og op til 133KWh pr måned underhold(21%) vask(20%) køle(17%) TV(15%) mad(11%) lys(10%) div.(5%) varme(2.5%) sauce:  https://findenergi.dk/guides/mit-elforbrug/
        // 4.3 kwh data er tilsvarende 12.19 kwh for familie på 4 sauce: https://www.energifyn.dk/privat/energiradgivning/energisparetips/stromslugere-i-din-bolig/
        public DateTime Time { get; set; }
        public string? House { get; set; }
        public int Usage { get; set; }
        private int Incrementor = 10;
        private double expectedMax { get; set; }
        private static readonly Random random = new();
        
        public Power()
        {
            double.TryParse( Environment.GetEnvironmentVariable("Amount"),out double a);
            expectedMax = a*4.3;
            if (expectedMax <= 0)
                expectedMax = 1000 * 4.3;
            Incrementor = random.Next((int)Math.Floor(expectedMax/2));
            Usage = Incrementor;
            Time = DateTime.Now;
        }
        public void Update()
        {
            Incrementor = random.Next((int)Math.Floor(expectedMax / 10));
            Time.AddHours(1);
            if (Usage < expectedMax)
                Usage += Incrementor;
            else
            {
                if (random.Next(8) == 2)
                    Usage += Incrementor;
                else
                    Usage -= Incrementor;
            }
            if (Usage < 0)
                Usage = 0;
            if (Time.Hour > 6 && Time.Hour < 16)
                Usage = (int)Math.Floor(Usage * 0.17);
        }
        public override string ToString()
        {
            return $"{Town} uses {Usage} kw of power";
        }
    }
}
