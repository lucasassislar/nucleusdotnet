using System.IO;

namespace Nucleus {
    public static class BinaryWriterExtensions {

        public static void WriteNucleusHeader(this BinaryWriter writer) {
            writer.Write('N');
            writer.Write('U');
            writer.Write('K');
            writer.Write('E');
        }
        public static void WriteNucleusVideoHeader(this BinaryWriter writer) {
            writer.Write('V');
            writer.Write('I');
            writer.Write('D');
        }

#if WINDOWS
        public static void Write(this BinaryWriter writer, System.Drawing.Rectangle rect) {
            writer.Write(rect.X);
            writer.Write(rect.Y);
            writer.Write(rect.Width);
            writer.Write(rect.Height);
        }
#endif
    }
}
