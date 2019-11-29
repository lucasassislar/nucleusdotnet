using System;

namespace Nucleus.Gaming {
    public static class MathUtil {
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

        public static double RecursiveSqrt(double value, int count) {
            RecursiveSqrt(ref value, count);
            return value;
        }

        public static void RecursiveSqrt(ref double value, int count) {
            for (int i = 0; i < count; i++) {
                value = Math.Sqrt(value);
            }
        }

        public static double RecursivePercentage(double pc, int count) {
            double remaining = 1;
            double value = 0;
            for (int i = 0; i < count; i++) {
                double removed = remaining * pc;
                value = removed;
                remaining -= removed;
            }
            return value;
        }
    }
}
