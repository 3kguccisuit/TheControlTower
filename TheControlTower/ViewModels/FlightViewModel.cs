using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;
using TheControlTowerBLL.Models;
using TheControlTowerBLL.Managers;
using Microsoft.Extensions.DependencyInjection;
using TheControlTower.Windows;


namespace TheControlTower.ViewModels
{
    public partial class FlightViewModel : ObservableObject
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ControlTower _controlTower;

        private Flight _SelectedFlight2;
        public Flight SelectedFlight2
        {
            get { return _SelectedFlight2; }
            set { SetProperty(ref _SelectedFlight2, value); }
        }

        public ObservableCollection<Flight> Flights2 { get; private set; } = new ObservableCollection<Flight>();

        public FlightViewModel(IServiceProvider serviceProvider, ControlTower controlTower)
        {
            _serviceProvider = serviceProvider;
            _controlTower = controlTower;
            RefreshFlights();
        }

        // Refresh the Flights collection
        private void RefreshFlights()
        {
            Flights2.Clear();
            var data = _controlTower.GetAll(); // Retrieve flights from ControlTower

            foreach (var flight in data)
            {
                Flights2.Add(flight);
            }

            SelectedFlight2 = Flights2.FirstOrDefault();    
        }
    }
}
