using System;

namespace TheControlTowerBLL.Delegate
{
    public class FlightHeightEventArgs : EventArgs
    {
        public string FlightName { get; set; }
        public int NewHeight { get; set; }

        public FlightHeightEventArgs(string flightName, int newHeight)
        {
            FlightName = flightName;
            NewHeight = newHeight;
        }
    }
}
