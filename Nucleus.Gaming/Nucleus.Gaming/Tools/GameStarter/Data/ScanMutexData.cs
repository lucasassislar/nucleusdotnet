namespace Nucleus.Tools.GameStarter {
    public class ScanMutexData {
        public string ProcessName { get; set; }
        public string[] Mutexes { get; set; }
        public bool ShouldRename { get; set; }
    }
}
