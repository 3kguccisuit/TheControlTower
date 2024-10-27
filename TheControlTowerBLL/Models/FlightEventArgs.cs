using System;

namespace TheControlTowerBLL.Models
{
    public class FlightEventArgs : EventArgs
    {
        public string FlightName { get; set; }
        public string Message { get; set; }

        public FlightEventArgs(string flightName, string message)
        {
            FlightName = flightName;
            Message = message;
        }
    }
}
