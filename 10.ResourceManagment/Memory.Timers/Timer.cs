using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Memory.Timers
{
    public class Timer : IDisposable
    {
        private bool _disposed;
        private readonly string _name;
        private readonly StringWriter _writer;
        private readonly Stopwatch _stopwatch;
        private readonly Queue<ChildTimer> _queue;

        private Timer(StringWriter writer, string name = null)
        {
            _writer = writer;
            _name = name ?? "*";
            _queue = new Queue<ChildTimer>();
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
        }
        
        public static Timer Start(StringWriter writer, string name = null) => new Timer(writer, name);

        public ChildTimer StartChildTimer(string childName)
        {
            var child = ChildTimer.StartChildTimerStatic(childName);
            _queue.Enqueue(child);
            return child;
        } 

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Timer()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposeFromMethod)
        {
            if (_disposed)
                return;

            if (disposeFromMethod)
            {
                _stopwatch.Stop();
                _writer.Write(FormatReportLine(_name, 0, _stopwatch.ElapsedMilliseconds));
                foreach (var item in _queue)
                    _writer.Write(item.Sb.ToString());

                if (_queue.Count > 0)
                    _writer.Write(FormatReportLine("Rest", 1,
                        _stopwatch.ElapsedMilliseconds - _queue.Sum(child => child.Stopwatch.ElapsedMilliseconds)));
            }
            
            _disposed = true;
        }

        public static string FormatReportLine(string timerName, int level, long value)
        {
            var intro = new string(' ', level * 4) + timerName;
            return $"{intro,-20}: {value}\n";
        }
    }

    public class ChildTimer : IDisposable
    {
        public readonly StringBuilder Sb = new StringBuilder();
        public Stopwatch Stopwatch { get; }
        private Queue<ChildTimer> Queue { get; }
        private string Name { get; }
        private long RestTime { get; set; }
        private int Lvl { get; }

        private ChildTimer(string name, int lvl = 1)
        {
            Name = name;
            Queue = new Queue<ChildTimer>();
            Lvl = lvl;
            Stopwatch = new Stopwatch();
            Stopwatch.Start();
        }

        public void Dispose()
        {
            Stopwatch.Stop();
            RestTime = Stopwatch.ElapsedMilliseconds - Queue.Sum(child => child.Stopwatch.ElapsedMilliseconds);

            Sb.Append(Timer.FormatReportLine(Name, Lvl, Stopwatch.ElapsedMilliseconds));
            foreach (var child in Queue)
                Sb.Append(Timer.FormatReportLine(child.Name, child.Lvl, child.Stopwatch.ElapsedMilliseconds));

            if (Queue.Count > 0)
                Sb.Append(Timer.FormatReportLine("Rest", Lvl + 1, RestTime));
        }

        public ChildTimer StartChildTimer(string name)
        {
            var child = new ChildTimer(name, Lvl + 1);
            Queue.Enqueue(child);
            return child;
        }

        public static ChildTimer StartChildTimerStatic(string name) => new ChildTimer(name);
    }
}