using System;
using TheControlTowerBLL.Models;

namespace TheControlTowerBLL.Managers
{
    public class ControlTower : DictionaryManager<string, Flight>
    {
        // Events for flight statuses
        public event EventHandler<FlightEventArgs> Landed;
        public event EventHandler<FlightEventArgs> TakeOff;
        public event EventHandler<FlightEventArgs> Altitude;

        // Method to add a flight and set up notifications
        public void AddFlight(string id, Flight flight)
        {
            Add(id, flight);

            // Set up notification callbacks from Flight to ControlTower
            flight.TakeOff += OnFlightTakeOff;
            flight.Landed += OnFlightLanded;
            flight.ChangeAltitude += OnChangeAltitude;
        }

        public void RemoveFlight(Flight flight)
        {
            Remove(flight.ID);
            flight.TakeOff -= OnFlightTakeOff;
            flight.Landed -= OnFlightLanded;
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
        public int ChangeAltitude(Flight flight, int newAltitude)
        {
            if (flight != null && flight.Status == "In-Flight")
            {
                return flight.OnAltitudeChange(flight, newAltitude); // Trigger altitude change in Flight
                // Altitude event will be triggered via OnChangeAltitude callback
            }
            return 0;
        }

        // Callback for when a flight takes off
        private void OnFlightTakeOff(Flight flight)
        {
            TakeOff?.Invoke(this, new FlightEventArgs(flight, "departed"));
        }

        // Callback for when a flight lands
        private void OnFlightLanded(Flight flight)
        {
            Landed?.Invoke(this, new FlightEventArgs(flight, "landed"));
            flight.Destination = "Home";
        }

        private int OnChangeAltitude(Flight flight, int newAltitude)
        {
            Altitude?.Invoke(this, new FlightEventArgs(flight, $"changed altitude to {flight.FlightHeight}"));
            return flight.FlightHeight;
        }
    }
}
