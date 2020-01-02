using Newtonsoft.Json;
using System.IO;

namespace Nucleus.IO {
    public class JsonPropertiesFile {
        protected string pathToFile;
        private readonly object locker = new object();

        public JsonPropertiesFile(string _pathToFile) {
            this.pathToFile = _pathToFile;
        }

        public void Load() {
            lock (locker) {
                if (File.Exists(pathToFile)) {
                    string jsonText = File.ReadAllText(pathToFile);
                    JsonConvert.PopulateObject(jsonText, this);
                }
            }
        }

        public void Save(string location = "") {
            if (!string.IsNullOrEmpty(location)) {
                pathToFile = location;
            }

            lock (locker) {
                string serialized = JsonConvert.SerializeObject(this);
                if (File.Exists(pathToFile)) {
                    File.Delete(pathToFile);
                }
                File.WriteAllText(pathToFile, serialized);
            }
        }
    }
}
