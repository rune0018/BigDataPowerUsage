using System;

namespace BigDataReciverPower
{
    public class Power
    {
        public DateTime Time { get; set; } = DateTime.Now;
        public string? House { get; set; }
        public int Usage { get; set; }
        private int Incrementor = 10;
        public double expectedMax { get; set; }
        private Random random {  get; set; } = new Random();
        public int Workat { get; set; }
        public int Homeat { get; set; }
        public int Sleepat { get; set; }
        public int UpAt { get; set; }
        private bool sleeping { get; set; }
        public Power() {}
        public Power(int seed)
        {
            random = new Random(seed);
            Usage = 100;
            if(expectedMax == 0)
                expectedMax = 100;
            random.Next(10000);
        }
        public Power(int seed, int workat, int homeat, int sleepat,int upat) : this(seed)
        {
            Workat = workat; Homeat = homeat; Sleepat = sleepat; UpAt = upat;
        }
        public void Update()
        {
            Incrementor = random.Next((int)Math.Floor(expectedMax/5));
            Time = Time.AddHours(1);
            if (Usage < expectedMax)
                Usage += Incrementor;
            else
                if (random.Next() == 1)
                    Usage = Incrementor/2+Usage;
                else
                    Usage -= Incrementor;
            if (Usage < 0) 
                Usage = 0;
            if (Time.Hour >= Workat && Time.Hour <= Homeat && Time.DayOfWeek != DayOfWeek.Saturday && Time.DayOfWeek != DayOfWeek.Sunday)
                Usage = (int)(expectedMax * 0.17);
            if (Time.Hour == Sleepat)
                sleeping = true;
            if (Time.Hour == UpAt)
                sleeping = false;

            if(sleeping)
                Usage = (int)(expectedMax * 0.30);
        }
        
    }
}
