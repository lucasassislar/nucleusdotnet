using Nucleus.Gaming.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nucleus.Gaming {
    public static class ConsoleU {
        private static object locker = new object();
        public static OutputLevel currentLevel = OutputLevel.High;

        private static string CropLine(string line) {
            int maxSize = Console.WindowWidth;

            if (line.Length >= maxSize) {
                line = line.Remove(maxSize, line.Length - maxSize);//cut start
                //line = line.Remove(0, line.Length - maxSize);// cut end
            }
            return line;
        }

        public static void WriteLine(string line, ConsoleColor foreGroundColor) {
            lock (locker) {
                line = CropLine(line);
                ConsoleColor current = Console.ForegroundColor;
                Console.ForegroundColor = foreGroundColor;
                Console.WriteLine(line);
                Console.ForegroundColor = current;
            }
        }

        public static void Write(string line, ConsoleColor foreGroundColor) {
            lock (locker) {
                line = CropLine(line);
                ConsoleColor current = Console.ForegroundColor;
                Console.ForegroundColor = foreGroundColor;
                Console.Write(line);
                Console.ForegroundColor = current;
            }
        }

        public static void WriteLine(string line, ConsoleColor foreGroundColor, OutputLevel logLevel) {
            if (logLevel < currentLevel) {
                return;
            }

            lock (locker) {
                line = CropLine(line);
                ConsoleColor current = Console.ForegroundColor;
                Console.ForegroundColor = foreGroundColor;
                Console.WriteLine(line);
                Console.ForegroundColor = current;
            }
        }

        public static void WriteLine(string line, OutputLevel logLevel) {
            if (logLevel < currentLevel) {
                return;
            }

            lock (locker) {
                line = CropLine(line);
                Console.WriteLine(line);
            }
        }
    }
}
