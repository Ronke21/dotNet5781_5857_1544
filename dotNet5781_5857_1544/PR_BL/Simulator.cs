using System;
using BO;

namespace BL
{
    public class Simulator
    {
        #region singelton
        static readonly Simulator instance = new Simulator();
        static Simulator() { }// static ctor to ensure instance init is done just before first usage
        private Simulator() { } // default => private
        public static Simulator Instance { get => instance; }// The public Instance property to use
        #endregion

        public int StatCode { get; set; }

        private LineTiming _lineTiming;
        public LineTiming LineTiming
        {
            get => _lineTiming;
            set
            {
                _lineTiming = value;
                setDigitalPanelObserver?.Invoke(_lineTiming);
            }
        }

        private event Action<LineTiming> setDigitalPanelObserver;

        public event Action<LineTiming> SetDigitalPanelObserver
        {
            add => setDigitalPanelObserver = value;
            remove => setDigitalPanelObserver -= value;
        }
    }
}
