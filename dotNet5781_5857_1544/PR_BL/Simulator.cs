using System;
using BO;

namespace BL
{
    /// <summary>
    /// class for the rides in the simulator. 
    /// singelton - created once and used from then.
    /// has an event to update the digital panel of a chosen station- its code also kept in the class, 
    /// an updating action from pl layer is signed to the event and activated in change of line timing - sends a arrival time of 1 line.
    /// </summary>
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
