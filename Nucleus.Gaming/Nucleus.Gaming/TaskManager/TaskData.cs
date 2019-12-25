using Nucleus.Gaming.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nucleus.TaskManager {
    public class TaskData {
        public string TypeName { get; set; }
        public string AssemblyName { get; set; }
        public string FunctionName { get; set; }
        public object[] Parameters { get; set; }

        public TaskData(string base64Str) {
            ApplicationUtil.PopulateObjectWithArgument(this, base64Str);
        }

        public string GetAsArguments() {
            return ApplicationUtil.GetObjectAsArgument(this);
        }
    }
}
