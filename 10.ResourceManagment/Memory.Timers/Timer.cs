using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Memory.Timers
{
    public class Timer : IDisposable
    {
        private bool _disposed;
        private long _rest;
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
            var child = new ChildTimer(childName);
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
                {
                    _writer.Write(FormatReportLine(item.Name, item.Lvl, item.Stopwatch.ElapsedMilliseconds));
                    _rest += item.Stopwatch.ElapsedMilliseconds;
                    if (item.Childs.Count == 0)
                        continue;

                    foreach (var child in item.Childs)
                    { 
                        _writer.Write(FormatReportLine(child.Name, child.Lvl, child.Stopwatch.ElapsedMilliseconds));
                    }
                    _writer.Write(FormatReportLine("Rest", item.Lvl + 1, item.RestTime));
                }
                
                if (_queue.Count > 0)
                    _writer.Write(FormatReportLine("Rest", 1, _stopwatch.ElapsedMilliseconds - _rest));
            }
            
            _disposed = true;
        }

        private static string FormatReportLine(string timerName, int level, long value)
        {
            var intro = new string(' ', level * 4) + timerName;
            return $"{intro,-20}: {value}\n";
        }
    }

    public class ChildTimer : IDisposable
    {
        public string Name { get; }
        public Stopwatch Stopwatch { get; }
        public Queue<ChildTimer> Childs { get; }
        public long RestTime { get; private set; }
        public int Lvl { get; }

        public ChildTimer(string name, int lvl = 1)
        {
            Name = name;
            Childs = new Queue<ChildTimer>();
            Lvl = lvl;
            Stopwatch = new Stopwatch();
            Stopwatch.Start();
        }

        public void Dispose()
        {
            Stopwatch.Stop();
            RestTime = Stopwatch.ElapsedMilliseconds - Childs.Sum(child => child.Stopwatch.ElapsedMilliseconds);
        }

        public ChildTimer StartChildTimer(string name)
        {
            var child = new ChildTimer(name, Lvl + 1);
            Childs.Enqueue(child);
            return child;
        }
    }
}