using System;

namespace Nucleus.Gaming {
    public static class MathUtil {

        public static int Clamp(int value, int min, int max) {
            return Math.Max(min, Math.Min(value, max));
        }

        /// <summary>
        /// Greatest Common Divisor
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int GCD(int a, int b) {
            int Remainder;

            while (b != 0) {
                Remainder = a % b;
                a = b;
                b = Remainder;
            }

            return a;
        }
    }
}
