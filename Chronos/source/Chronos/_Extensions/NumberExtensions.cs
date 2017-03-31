using System;

namespace Chronos
{
    public static class NumberExtensions
    {
        public static bool IsZero(this double d)
        {
            return Math.Abs(d) <= double.Epsilon;
        }

        public static bool IsNaNOrZero(this double d)
        {
            return double.IsNaN(d) || Math.Abs(d) <= double.Epsilon;
        }
    }
}
