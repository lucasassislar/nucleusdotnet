﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Nucleus {
    public static class ByteArrayExtensions {
        public static string ToHex(this byte[] bytes, bool upperCase) {
            StringBuilder result = new StringBuilder(bytes.Length * 2);

            for (int i = 0; i < bytes.Length; i++)
                result.Append(bytes[i].ToString(upperCase ? "X2" : "x2"));

            return result.ToString();
        }
    }
}
