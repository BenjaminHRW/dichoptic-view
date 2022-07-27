using System;
using System.Diagnostics;

namespace RejTech
{
    public class Counter
    {
        private Stopwatch time  = new Stopwatch();
        public int Count { get; internal set; }

        /// <summary>Constructor</summary>
        public Counter()
        {
            Clear();
        }

        /// <summary>Clears the counter</summary>
        public void Clear()
        {
            time.Reset();
            Count = 0;
        }

        /// <summary>Begins the counting</summary>
        public void Start()
        {
            Clear();
            time.Start();
        }

        /// <summary>Counter behaves as an integer during incrementing</summary>
        public static Counter operator ++(Counter counter)
        {
            counter.Count++;
            return counter;
        }

        /// <summary>Counter behaves as an integer in expressions</summary>
        public static implicit operator int(Counter counter)
        {
            return counter.Count;
        }

        /// <summary>Returns true if counter is running</summary>
        public bool IsRunning
        {
            get
            {
                return time.IsRunning;
            }
        }

        /// <summary>Rate of increments per second</summary>
        public double Frequency
        {
            get
            {
                return (double)Count / ((double)time.ElapsedTicks / Stopwatch.Frequency);
            }
        }

        /// <summary>Milliseconds elapsed since last reset/start (decimal, sub-millisecond accurate)</summary>
        public double Timestamp
        {
            get
            {
                return (double)time.ElapsedTicks / Stopwatch.Frequency * 1000.0d;
            }
        }

        /// <summary>Ticks elapsed</summary>
        public long TicksElapsed
        {
            get
            {
                return time.ElapsedTicks;
            }
        }
    }
}
