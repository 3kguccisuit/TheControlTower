using System;
using TheControlTowerBLL.Models;

namespace TheControlTowerBLL.Managers
{
    public class ControlTower : DictionaryManager<string, Flight>
    {
        // Events for flight statuses
        public event EventHandler<FlightEventArgs> Landed;
        public event EventHandler<FlightEventArgs> TakeOff;

        // Method to add a flight and set up notifications
        public void AddFlight(string id, Flight flight)
        {
            Add(id, flight);

            // Set up notification callbacks from Flight to ControlTower
            flight.TakeOff += OnFlightTakeOff;
            flight.Landed += OnFlightLanded;
        }

        // Orders a flight to take off using its ID
        public void OrderTakeoff(string id)
        {
            var flight = Get(id);
            if (flight != null && flight.Status == "Ready")
            {
                flight.OnTakeOff(); // Trigger takeoff in Flight
                // TakeOff event will be triggered via OnFlightTakeOff callback
            }
        }

        // Callback for when a flight takes off
        private void OnFlightTakeOff(Flight flight)
        {
            TakeOff?.Invoke(this, new FlightEventArgs(flight.Name, "Flight has taken off"));
        }

        // Callback for when a flight lands
        private void OnFlightLanded(Flight flight)
        {
            Landed?.Invoke(this, new FlightEventArgs(flight.Name, "Flight has landed"));
        }
    }
}
