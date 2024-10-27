using System;
using System.Windows;
using System.Windows.Threading;

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

        // Events
        public event EventHandler<FlightEventArgs> TakeOff;
        public event EventHandler<FlightEventArgs> Landed;
        public event EventHandler<FlightHeightEventArgs> FlightHeightChange;

        // Constructor
        public Flight(string id, string name, string destination, double time)
        {
            ID = id;
            Name = name;
            Destination = destination;
            Time = time;
            //DepartureTime = TimeOnly.FromDateTime(DateTime.Now); // Set departure time
            Status = "Ready"; // Initial status
            InFlight = false;
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
            dispatchTimer.Start(); // Start the timer here
            TakeOff?.Invoke(this, new FlightEventArgs(Name, "Flight has taken off"));
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

            // Check how many seconds have passed since departure
            double timeLeft = (LocalTime - DepartureTime).TotalSeconds;

            // If sufficient time has passed, make the airplane land
            if (timeLeft >= Time) // Time is now configurable from the constructor
            {
                OnLanding();
            }
        }

        // Method to trigger the Landed event
        public void OnLanding()
        {
            StopTimer(); // Stop the timer when the flight lands
            Status = "Landed";
            InFlight = false;
            Landed?.Invoke(this, new FlightEventArgs(Name, "Flight has landed"));
        }

        // Method to change flight height
        public void ChangeFlightHeight(int height)
        {
            FlightHeight = height;
            FlightHeightChange?.Invoke(this, new FlightHeightEventArgs(Name, height));
        }
    }

}
