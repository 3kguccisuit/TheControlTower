using TheControlTowerBLL.Models;

namespace TheControlTowerBLL.Managers
{
    public class ControlTower : DictionaryManager<string, Flight>
    {
        // Events for flight statuses
        public event EventHandler<FlightEventArgs> Landed;
        public event EventHandler<FlightEventArgs> TakeOff;

        // Orders a flight to take off using its ID
        public void OrderTakeoff(string id)
        {
            var flight = Get(id);
            if (flight != null && flight.Status == "Ready")
            {
                flight.OnTakeOff(); // Trigger takeoff
               // TakeOff?.Invoke(this, new FlightEventArgs(flight.Name, "Flight has taken off")); // Notify listeners
            }
        }

        // Method to land a flight using its ID
        public void OrderLanding(string id)
        {
            var flight = Get(id);
            if (flight != null)
            {
                flight.OnLanding(); // Trigger landing
                Landed?.Invoke(this, new FlightEventArgs(flight.Name, "Flight has landed")); // Notify listeners
            }
        }

        // Handle flight height changes using the flight's ID
        public void ChangeFlightHeight(string id, string height)
        {
            var flight = Get(id);
            if (flight != null && int.TryParse(height, out int newHeight))
            {
                flight.ChangeFlightHeight(newHeight); // This will trigger the event inside the Flight class
            }
        }
    }
}
