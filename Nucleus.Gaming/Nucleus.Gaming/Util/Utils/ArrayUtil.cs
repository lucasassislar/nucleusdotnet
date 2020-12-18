using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus {
    public static class ArrayUtil {
        public static T[] Join<T>(T[] a, T[] b) {
            T[] final = new T[a.Length + b.Length];

            for (int i = 0; i < a.Length; i++) {
                final[i] = a[i];
            }

            for (int i = 0; i < b.Length; i++) {
                final[i + a.Length] = b[i];
            }

            return final;
        }
    }
}
