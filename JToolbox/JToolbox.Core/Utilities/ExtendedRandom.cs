using System;

namespace JToolbox.Core.Utilities
{
    public class ExtendedRandom : Random
    {
        private readonly GaussianValuesSource gaussianSource;

        public ExtendedRandom() : this((int)(new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds()))
        {
        }

        public ExtendedRandom(int seed) : base(seed)
        {
            gaussianSource = new GaussianValuesSource(this);
        }

        public bool NextBool()
        {
            return Next(2) == 0;
        }

        public byte NextByte()
        {
            return (byte)Next(0, 256);
        }

        public double NextDouble(double min, double max)
        {
            return min + NextDouble() * (max - min);
        }

        public double NextGaussian(double stdDev)
        {
            return gaussianSource.Next(0d, stdDev);
        }

        public double NextGaussian(double mean, double stdDev)
        {
            return gaussianSource.Next(mean, stdDev);
        }

        public class GaussianValuesSource
        {
            private double? nextValue;
            private readonly Random random;

            public GaussianValuesSource(Random random)
            {
                this.random = random;
            }

            public double Next(double mean, double stdDev)
            {
                double result;
                if (!nextValue.HasValue)
                {
                    double u1 = 1d - random.NextDouble();
                    double u2 = 1d - random.NextDouble();
                    double R = Math.Sqrt(-2d * Math.Log(u1));
                    double theta = 2d * Math.PI * u2;
                    double z0 = R * Math.Cos(theta);
                    double z1 = R * Math.Sin(theta);

                    nextValue = z1;
                    result = mean + z0 * stdDev;
                }
                else
                {
                    result = mean + nextValue.Value * stdDev;
                    nextValue = null;
                }
                return result;
            }
        }
    }
}