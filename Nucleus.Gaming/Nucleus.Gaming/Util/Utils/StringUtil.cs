using System;
using System.Collections;
using System.Drawing;
using System.Linq;

namespace Nucleus {
    public static class StringUtil {
        public static string ClearTextString(string strValue) {
            while (strValue.Contains("\t")) {
                strValue = strValue.Replace("\t", "");
            }
            return strValue;
        }

        public static string ClearStartSpaces(string strValue) {
            while (strValue[0] == ' ') {
                strValue = strValue.Remove(0, 1);
            }
            return strValue;
        }

        public static string GetCollisionStringStart(string aStr, string bStr) {
            string col = "";

            for (int i = 0; i < aStr.Length; i++) {
                if (bStr.Length <= i) {
                    break;
                }

                char a = aStr[i];
                char b = bStr[i];
                if (a == b) {
                    col += a;
                } else {
                    break;
                }
            }

            return col;
        }

        public static readonly char[] Numbers = new char[]
        {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
        };

        public static bool IsNumber(char c) {
            // We COULD use Int.TryParse, but this looks way cleaner
            return Numbers.Contains(c);
        }

        public static string RepeatCharacter(char c, int times) {
            string s = "";
            for (int i = 0; i < times; i++) {
                s += c;
            }
            return s;
        }

        public static void MakeSameSize(ref string a, ref string b) {
            if (a.Length < b.Length) {
                int dif = b.Length - a.Length;
                for (int k = 0; k < dif; k++) {
                    a = " " + a;
                }
            } else if (a.Length > b.Length) {
                int dif = a.Length - b.Length;
                for (int k = 0; k < dif; k++) {
                    b = " " + b;
                }
            }
        }

        public static string ReplaceCaseInsensitive(string str, string toFind, string toReplace) {
            string lowerOriginal = str.ToLower();
            string lowerFind = toFind.ToLower();
            string lowerRep = toReplace.ToLower();

            int start = lowerOriginal.IndexOf(lowerFind);
            if (start == -1) {
                return str;
            }

            string end = str.Remove(start, toFind.Length);
            end = end.Insert(start, toReplace);

            return end;
        }

        /// <summary>
        /// Compute the distance between two strings.
        /// </summary>
        public static int ComputeLevenshteinDistance(string s, string t) {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // Step 1
            if (n == 0) {
                return m;
            }

            if (m == 0) {
                return n;
            }

            // Step 2
            for (int i = 0; i <= n; d[i, 0] = i++) {
            }

            for (int j = 0; j <= m; d[0, j] = j++) {
            }

            // Step 3
            for (int i = 1; i <= n; i++) {
                //Step 4
                for (int j = 1; j <= m; j++) {
                    // Step 5
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            // Step 7
            return d[n, m];
        }

        public static string ArrayToString(Array array) {
            string str = "";
            for (int i = 0; i < array.Length; i++) {
                object ob = ((IList)array)[i];
                str += ob;
                if (i != array.Length - 1) {
                    ob += ", ";
                }
            }
            return str;
        }

#if WINFORMS

        /// <summary>
        /// This method can be made better
        /// </summary>
        /// <param name="maxWidth"></param>
        /// <param name="str"></param>
        /// <param name="graphics"></param>
        /// <param name="font"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string WrapString(float maxWidth, string str, Graphics graphics, Font font, out SizeF size) {
            size = graphics.MeasureString(str, font);

            string[] sep = str.Split(' ');
            if (sep.Length == 0) {
                return str;
            }

            float spaceSize = graphics.MeasureString(" ", font).Width;

            string result = sep[0];

            float currentWidth = graphics.MeasureString(result, font).Width;
            float maxUsedWidth = 0;
            int lines = 1;
            string currentLine = result;

            for (int i = 1; i < sep.Length; i++) {
                string word = sep[i];
                string spaceWord = " " + word;
                SizeF wordSize = graphics.MeasureString(spaceWord, font);
                currentWidth += wordSize.Width;

                if (currentWidth > maxWidth) {
                    currentWidth = wordSize.Width;
                    maxUsedWidth = Math.Max(maxUsedWidth, graphics.MeasureString(currentLine, font).Width);
                    result += "\n" + word;
                    currentLine = "";
                    lines++;
                } else {
                    result += spaceWord;
                    currentLine += spaceWord;
                }
            }

            if (maxUsedWidth == 0) {
                maxUsedWidth = Math.Max(maxUsedWidth, graphics.MeasureString(currentLine, font).Width);
            }
            size = new SizeF(maxUsedWidth, lines * font.GetHeight(graphics));

            return result;
        }
#endif
    }
}
