using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Nucleus.Gaming.Diagnostics {
    public static class Profiler {
        private static Dictionary<MeasureKey, MeasureData> measuring = new Dictionary<MeasureKey, MeasureData>();

        public static MeasureData? GetCurrentlyMeasuringByThread(int threadId) {
            lock (measuring) {
                MeasureData data = measuring.Values.FirstOrDefault(c => c.ThreadID == threadId);
                if (data.Equals(default(MeasureData))) {
                    return null;
                }
                return data;
            }
        }

        public static List<MeasureData> AllData = new List<MeasureData>();

        public static void StartMeasuring(object parent, string uniqueKey, int thread) {
            MeasureKey reference = new MeasureKey(parent, uniqueKey);
            lock (measuring) {
                if (!measuring.ContainsKey(reference)) {
                    measuring.Add(reference, new MeasureData(reference, Stopwatch.GetTimestamp(), thread));
                }
            }
        }

        public static double EndMeasuring(object parent, string uniqueKey) {
            MeasureKey reference = new MeasureKey(parent, uniqueKey);
            MeasureData timestamp = measuring[reference];
            lock (measuring) {
                measuring.Remove(reference);
            }

            long now = Stopwatch.GetTimestamp();
            double result = (now - timestamp.RecordedValue) / (double)Stopwatch.Frequency;
            AllData.Add(new MeasureData(reference, result, timestamp.ThreadID));

            //ConsoleU.WriteLine($"Finished job {uniqueKey} in {(result * 1000).ToString("F0")}ms (Thread ${timestamp.ThreadID})", ConsoleColor.Yellow, OutputLevel.Low);

            return result;
        }
    }
}
