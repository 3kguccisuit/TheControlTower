using System;
using TheControlTowerBLL.Models;

namespace TheControlTowerBLL.Delegate
{

    public delegate void FlightStatusHandler(Flight sender);
    public delegate int AltitudeChangeHandler(Flight sender, int altitude);
    public class FlightEventArgs : EventArgs
    {
        public Flight Flight { get; set; }
        // public string FlightName { get; set; }
        public string Message { get; set; }

        public FlightEventArgs(Flight flight, string message)
        {
            Flight = flight;
            Message = message;
        }
    }
}
