using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Gaming.Threading {
    public class ThreadData {
        public int ThreadID { get; private set; }
        public ThreadTask CurrentTask;

        public ThreadData(int threadID) {
            ThreadID = threadID;
        }
    }
}
