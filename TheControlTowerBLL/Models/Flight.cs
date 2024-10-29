using System;
using System.Windows.Threading;

namespace TheControlTowerBLL.Models
{
    public delegate void FlightStatusHandler(Flight sender);
    //public delegate void FlightHeightChangeHandler(Flight sender, int height);
}


namespace TheControlTowerBLL.Models
{
    public class Flight
    {
        // Fields
        private DispatcherTimer dispatchTimer;
        private double time;

        // Properties
        public string Name { get; set; }
        public string ID { get; set; }
        public string Destination { get; set; }
        public int FlightHeight { get; set; }
        public string Status { get; set; } // Ready, In-Flight, Landed
        public TimeOnly DepartureTime { get; set; }
        public bool InFlight { get; set; }
        public TimeOnly LocalTime { get; set; } // Local time for the flight
        public double Time
        {
            get => time;
            set => time = value;
        }

        // Delegates to notify ControlTower of state changes
        public event FlightStatusHandler TakeOff;
        public event FlightStatusHandler Landed;

        // Constructor
        public Flight(string id, string name, string destination, double time)
        {
            ID = id;
            Name = name;
            Destination = destination;
            Time = time;
            Status = "Ready"; // Initial status
            InFlight = false;
            FlightHeight = 0;
            dispatchTimer = new DispatcherTimer(); // Initialize the DispatcherTimer but don't start it
            dispatchTimer.Tick += DispatcherTimer_Tick;
            dispatchTimer.Interval = new TimeSpan(0, 0, 1); // 1 second = 1 hour in simulation time
        }

        // Start the DispatcherTimer only when the flight takes off
        public void OnTakeOff()
        {
            Status = "In-Flight";
            InFlight = true;
            DepartureTime = TimeOnly.FromDateTime(DateTime.Now); // Update departure time at takeoff
            FlightHeight = 10000;
            dispatchTimer.Start(); // Start the timer here
            TakeOff?.Invoke(this);
        }

        // Method to handle landing
        public void OnLanding()
        {
            StopTimer(); // Stop the timer when the flight lands
            Status = "Ready";
            InFlight = false;
            FlightHeight = 0;
           // Destination = "Home";
            Landed?.Invoke(this);
        }


        // Stop the timer when the flight lands
        public void StopTimer()
        {
            dispatchTimer.Stop();
        }

        // DispatcherTimer tick event to simulate flight time
        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            LocalTime = TimeOnly.FromDateTime(DateTime.Now);

            // Check how much time has passed since departure
            double timeLeft = (LocalTime - DepartureTime).TotalSeconds;

            // If sufficient time has passed, make the airplane land
            if (timeLeft >= Time) // Time is now configurable from the constructor
            {
                OnLanding();
            }
        }
    }
}
