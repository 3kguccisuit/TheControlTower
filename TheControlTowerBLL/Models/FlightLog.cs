using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheControlTowerBLL.Models
{
    public class FlightLog
    {
        public string FlightName { get; set; }
        public string ID { get; set; }
        public string FlightID { get; set; }
        public string Date { get; set; }
        public string Message { get; set; }
        public string Destination { get; set; } 

        public FlightLog(string id, string flightName, string message, string date, string destination, string flightID)
        {
            ID = id;
            FlightID = flightID;
            Date = date;
            FlightName = flightName;
            Message = message;
            Destination = destination;
        }
    }
}
 