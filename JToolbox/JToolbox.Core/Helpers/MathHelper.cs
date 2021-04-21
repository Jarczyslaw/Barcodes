using System;

namespace JToolbox.Core.Helpers
{
    public static class MathHelper
    {
        public static byte RoundToByte(double value)
        {
            return ByteClamp(Math.Round(value));
        }

        public static int RoundToInt(double value)
        {
            return (int)Math.Round(value);
        }

        public static byte CeilToByte(double value)
        {
            return ByteClamp(Math.Ceiling(value));
        }

        public static int CeilToInt(double value)
        {
            return (int)Math.Ceiling(value);
        }

        public static byte FloorToByte(double value)
        {
            return ByteClamp(Math.Floor(value));
        }

        public static int FloorToInt(double value)
        {
            return (int)Math.Floor(value);
        }

        public static byte Max(byte val1, byte val2, byte val3)
        {
            return Math.Max(val1, Math.Max(val2, val3));
        }

        public static double Max(double val1, double val2, double val3)
        {
            return Math.Max(val1, Math.Max(val2, val3));
        }

        public static byte Min(byte val1, byte val2, byte val3)
        {
            return Math.Min(val1, Math.Min(val2, val3));
        }

        public static double Min(double val1, double val2, double val3)
        {
            return Math.Min(val1, Math.Min(val2, val3));
        }

        public static int Clamp(int value, int min, int max)
        {
            if (value > max)
                return max;
            if (value < min)
                return min;
            return value;
        }

        public static double Clamp(double value, double min, double max)
        {
            if (value > max)
                return max;
            if (value < min)
                return min;
            return value;
        }

        public static byte ByteClamp(int value)
        {
            return (byte)Clamp(value, byte.MinValue, byte.MaxValue);
        }

        public static byte ByteClamp(double value)
        {
            return (byte)Clamp(value, byte.MinValue, byte.MaxValue);
        }

        public static double DegToRad(double deg)
        {
            return deg * Math.PI / 180d;
        }

        public static double RadToDeg(double rad)
        {
            return rad * 180d / Math.PI;
        }

        public static void Swap<T>(ref T left, ref T right)
        {
            T temp = left;
            left = right;
            right = temp;
        }

        public static void Orientate(ref byte val1, ref byte val2)
        {
            if (val1 > val2)
                Swap(ref val1, ref val2);
        }

        public static void Orientate(ref int val1, ref int val2)
        {
            if (val1 > val2)
                Swap(ref val1, ref val2);
        }

        public static void Orientate(ref double val1, ref double val2)
        {
            if (val1 > val2)
                Swap(ref val1, ref val2);
        }

        public static bool AreEqual(double val1, double val2, double tolerance = 0.001d)
        {
            return Math.Abs(val1 - val2) < tolerance;
        }

        public static double Rescale(double x, double a1, double a2, double b1, double b2)
        {
            return (b2 - b1) / (a2 - a1) * (x - a1) + b1;
        }

        public static double Normalize(double value, double max)
        {
            return byte.MaxValue * value / max;
        }

        public static double NormalizeLog10(double value, double max)
        {
            return byte.MaxValue * Math.Log10(1d + value) / Math.Log10(1d + max);
        }

        public static byte Negative(byte value)
        {
            return (byte)(byte.MaxValue - value);
        }
    }
}