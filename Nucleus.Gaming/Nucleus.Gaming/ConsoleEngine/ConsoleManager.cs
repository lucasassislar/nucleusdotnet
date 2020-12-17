using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.ConsoleEngine {
    public class ConsoleManager {
        public Dictionary<string, ConsoleCommand> commands;
        private bool silentMode;

        public void SetSilentMode(bool value) {
            silentMode = value;
        }

        public ConsoleManager() {
            commands = new Dictionary<string, ConsoleCommand>();
        }

        public static IEnumerable<Assembly> GetAssemblies() {
            var list = new List<string>();
            var stack = new Stack<Assembly>();

            stack.Push(Assembly.GetEntryAssembly());

            do {
                var asm = stack.Pop();

                yield return asm;

                foreach (var reference in asm.GetReferencedAssemblies())
                    if (!list.Contains(reference.FullName)) {
                        stack.Push(Assembly.Load(reference));
                        list.Add(reference.FullName);
                    }

            }
            while (stack.Count > 0);
        }


        public void SearchFromLoadedAssemblies() {
            Type consoleType = typeof(ConsoleCommand);

            Assembly entryAssembly = Assembly.GetEntryAssembly();
            string folder = Path.GetDirectoryName(entryAssembly.Location);
            string[] arrDlls = Directory.GetFiles(folder, "*.dll");

            //AssemblyName[] assNames = entryAssembly.GetReferencedAssemblies();
            //Assembly[] assemblies = new Assembly[] { entryAssembly };
            //for (int i = 0; i < assemblies.Length; i++) {

            for (int i = 0; i < arrDlls.Length; i++) {
                string strDllPath = arrDlls[i];
                //Assembly assembly = assemblies[i];
                Assembly assembly = Assembly.LoadFrom(strDllPath);
                Type[] types = assembly.GetTypes();
                for (int j = 0; j < types.Length; j++) {
                    Type t = types[j];

                    if (t.IsSubclassOf(consoleType)) {
                        ConsoleCommand cmd = (ConsoleCommand)Activator.CreateInstance(t, this);
                        commands.Add(cmd.Command, cmd);
                    }
                }
            }
        }

        public bool InputYesNo(bool silent = true) {
            ConsoleU.WriteLine("Yes/No", Palette.Question);
            if (silentMode) {
                // still shows up that we were asking the user a question
                ConsoleU.WriteLine(silent ? "Yes" : "No", Palette.Question);
                ConsoleU.WriteLine("yes");
                return silent;
            }

            string yesno = ConsoleU.ReadLine().ToLower();
            return yesno.StartsWith("y");
        }

        public void ExecuteCommand(string line) {
            if (string.IsNullOrEmpty(line)) {
                return;
            }

            string[] sep = line.Split(' ');
            ExecuteCommand(sep);
        }

        public void ExecuteCommand(string[] sep) {
            if (sep.Length == 0) {
                return;
            }

            string first = sep[0];
            ConsoleCommand cmd;
            if (!commands.TryGetValue(first, out cmd)) {
                ConsoleU.WriteLine("Unknown command", Palette.Error);
                return;
            }

            CommandFeedback feedback = cmd.Execute(sep);
            if (feedback != CommandFeedback.Success) {
                ConsoleU.WriteLine(feedback.ToString(), Palette.Error);
            }
        }

        public void Run() {
            for (; ; )
            {
                string line = ConsoleU.ReadLine();
                ExecuteCommand(line);
            }
        }

        public void ProcessCommand(string command) {

        }

        public ConsoleCommand GetCommand(string cmd) {
            ConsoleCommand command;
            commands.TryGetValue(cmd, out command);
            return command;
        }
    }
}
