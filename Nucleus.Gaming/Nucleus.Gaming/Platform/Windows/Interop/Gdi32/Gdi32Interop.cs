using System;
using System.Runtime.InteropServices;

namespace Nucleus.Gaming.Windows.Interop {
    public static class Gdi32Interop {
        [DllImport("gdi32.dll")]
        public static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateRoundRectRgn(int x1, int y1, int x2, int y2, int cx, int cy);
    }
}
