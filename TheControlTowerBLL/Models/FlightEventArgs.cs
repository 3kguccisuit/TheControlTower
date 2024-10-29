using System;

namespace TheControlTowerBLL.Models
{
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
