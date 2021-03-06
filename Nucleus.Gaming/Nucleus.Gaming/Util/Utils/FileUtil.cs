﻿using System.IO;

namespace Nucleus {
    public static class FileUtil {
        public static void Write(byte[] data, string place) {
            using (MemoryStream str = new MemoryStream(data)) {
                using (FileStream stream = new FileStream(place, FileMode.Create)) {
                    str.CopyTo(stream);
                    stream.Flush();
                }
            }
        }

        /// <summary>
        /// Copies the files from a directory to another
        /// </summary>
        /// <param name="rootFolder"></param>
        /// <param name="destination"></param>
        /// <param name="exitCode"></param>
        /// <param name="exclusions"></param>
        public static void CopyDirContents(string rootFolder, string destination, out int exitCode, params string[] exclusions) {
            exitCode = 1;

            FileInfo[] files = new DirectoryInfo(rootFolder).GetFiles();

            for (int i = 0; i < files.Length; i++) {
                FileInfo file = files[i];

                string lower = file.Name.ToLower();
                bool cont = false;
                for (int j = 0; j < exclusions.Length; j++) {
                    string exc = exclusions[j];
                    if (lower.Contains(exc)) {
                        cont = true;
                        break;
                    }
                }

                if (cont) {
                    continue;
                }

                string relative = file.FullName.Replace(rootFolder + @"\", "");
                string linkPath = Path.Combine(destination, relative);
                try {
                    File.Copy(file.FullName, linkPath, false);
                } catch { }
            }
        }

        public static void CopyDirectory(string root, DirectoryInfo currentDir, string destination, out int exitCode, string[] dirExclusions, string[] fileExclusions, bool overrideSpecial = false) {
            exitCode = 1;

            Directory.CreateDirectory(destination);
            // copy all files
            CopyDirContents(currentDir.FullName, destination, out exitCode, fileExclusions);

            DirectoryInfo[] children = currentDir.GetDirectories();
            for (int i = 0; i < children.Length; i++) {
                DirectoryInfo child = children[i];
                CopyDirectory(root, child, Path.Combine(destination, child.Name), out exitCode, dirExclusions, fileExclusions);
            }
        }
    }
}
