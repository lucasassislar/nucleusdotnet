namespace Nucleus.Tools.GameStarter {
    public class SymlinkGameData {
        public string SourcePath { get; set; }
        public string DestinationPath { get; set; }

        public string[] DirExclusions { get; set; }
        public string[] FileExclusions { get; set; }
        public string[] FileCopies { get; set; }
    }
}
