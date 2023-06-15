using System;

namespace BigdataPowerUsageVisualisation
{
    public class Power
    {
        public DateTime Time { get; set; }
        public string? House { get; set; }
        public int Usage { get; set; }
        private int Incrementor = 10;
        private double expectedMax { get; set; }
        private Random random {  get; set; } = new Random();
        public Power() { }
        public Power(int seed)
        {
            random = new Random(seed);
        }
        public void Update()
        {
            Incrementor = random.Next((int)Math.Floor(expectedMax / 2));
            Time.AddHours(1);
            if (Usage < expectedMax)
                Usage += Incrementor;
            else
            {
                if (random.Next() == 1)
                    Usage += Incrementor;
                else
                    Usage -= Incrementor;
            }
            if (Usage < 0)
                Usage = 0;
            if (Time.Hour > 6 && Time.Hour < 16)
                Usage = (int)Math.Floor(Usage * 0.17);
        }
    }
}
