using System;
using TheControlTowerBLL.Models;

namespace TheControlTowerBLL.Managers
{
    public class FlightLogManager : DictionaryManager<string, FlightLog>
    {

        private void AddFlightLog(object sender, FlightEventArgs e)
        {
            var flight = e.Flight;
            var uniqueID = Guid.NewGuid().ToString();
            var logEntry = new FlightLog(
                id: uniqueID,
                flightID: e.Flight.ID,
                flightName: e.Flight.Name,
                message: e.Message,
                date: DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                destination: e.Flight.Destination
            );

            Add(logEntry.ID, logEntry);
        }
    }
}
