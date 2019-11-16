using System.IO;

namespace Nucleus.Gaming.Diagnostics {
    public interface ILogNode {
        void OnFailureLog(StreamWriter writer);
    }
}
