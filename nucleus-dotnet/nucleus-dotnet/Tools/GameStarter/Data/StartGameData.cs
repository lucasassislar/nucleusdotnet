namespace Nucleus.Tools.GameStarter {
    public class StartGameData {
        public GameStarterTask Task { get; set; }
        public string[] Parameters { get; set; }

        public string GetAsArguments() {
            return ApplicationUtil.GetObjectAsArgument(this);
        }
    }
}
