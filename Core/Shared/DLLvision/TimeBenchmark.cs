using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;

namespace ekium_test
{
    class TimeBenchmark
    {
        private IDictionary<BenchmarkEvent, List<double>> _eventTimes = new Dictionary<BenchmarkEvent, List<double>>();
        private IDictionary<BenchmarkEvent, double> _eventAverageTimes = new Dictionary<BenchmarkEvent, double>();
        private IDictionary<BenchmarkEvent, Stopwatch> _eventWatches = new Dictionary<BenchmarkEvent, Stopwatch>();

        public TimeBenchmark() { }

        public void registerEvent(BenchmarkEvent eventName)
        {
            _eventTimes.Add(eventName, new List<double>());
            _eventWatches.Add(eventName, new Stopwatch());
        }

        public void startBenchmark(BenchmarkEvent eventName)
        {
            _eventWatches[eventName].Start();
        }

        public void stopBenchmark(BenchmarkEvent eventName)
        {
            _eventWatches[eventName].Stop();
            _eventTimes[eventName].Add(_eventWatches[eventName].Elapsed.TotalMilliseconds*1000000);
            _eventWatches[eventName].Reset();
        }

        public double getAverage(BenchmarkEvent eventName)
        {
            return _eventTimes[eventName].Average();
        }

        private void populateAverage() {
            foreach (var item in _eventTimes)
            {
                _eventAverageTimes[item.Key] = item.Value.Average();
            }
        }

        public void writeEventTimes(string path, BenchmarkEvent bevent)
        {
            File.WriteAllLines(path, _eventTimes[bevent].Select(x => x.ToString()));
        }
    }
}
