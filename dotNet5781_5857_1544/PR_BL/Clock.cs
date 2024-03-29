﻿using System;
using System.Diagnostics;

namespace BL
{
    /// <summary>
    /// class for the clock in the simulator. 
    /// singelton - created once and used from then.
    /// contains a stop watch to calculate the time according to rate.
    /// has an event to update the clock, 
    /// an updating action from pl layer is signed to the event and activated in change of clock.
    /// </summary>
    public class Clock
    {
        #region singelton
        static readonly Clock instance = new Clock();
        static Clock() { }// static ctor to ensure instance init is done just before first usage
        private Clock() { } // default => private
        public static Clock Instance { get => instance; }// The public Instance property to use
        #endregion

        private TimeSpan _time;
        public TimeSpan Time
        {
            get => _time;
            set
            {
                _time = value;
                clockObserver?.Invoke(_time);
            }
        }

        public int Rate { get; set; }

        public Stopwatch Timer = new Stopwatch();

        internal volatile bool IsRunning; // can be accessed by all threads

        private event Action<TimeSpan> clockObserver;

        public event Action<TimeSpan> ClockObserver
        {
            add => clockObserver = value;
            remove => clockObserver -= value;
        }

    }
}
