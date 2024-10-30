using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheControlTowerBLL.Models
{
    public class FlightLog
    {
        public string Message { get; set; }

        public FlightLog(string message)
        {
            Message = message;
        }
    }
}
 