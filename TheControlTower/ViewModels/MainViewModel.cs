using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using TheControlTower.ViewModels;
using TheControlTower.Windows;
using TheControlTowerBLL.Managers;
using TheControlTowerBLL.Models;

public partial class MainViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ControlTower _controlTower;

    [ObservableProperty]
    private Flight selectedFlight;

    public ObservableCollection<Flight> Flights { get; private set; } = new ObservableCollection<Flight>();
    public ObservableCollection<string> FlightLog { get; private set; } = new ObservableCollection<string>();

    public MainViewModel(IServiceProvider serviceProvider, ControlTower controlTower)
    {
        _serviceProvider = serviceProvider;
        _controlTower = controlTower;

        // Subscribe to ControlTower events
        _controlTower.TakeOff += OnFlightTakeOff;
        _controlTower.Landed += OnFlightLanded;

        RefreshFlights();
    }

    [RelayCommand]
    private void DeleteFlight(Flight selected)
    {
        if (selected != null)
        {
            var result = MessageBox.Show($"Are you sure you want to delete the selected flight?\n\n{selected.Name}",
                                         "Confirm Deletion",
                                         MessageBoxButton.YesNo,
                                         MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                Flights.Remove(selected);
                _controlTower.Remove(selected.ID); // Remove from ControlTower
                RefreshFlights();
                SelectedFlight = Flights.FirstOrDefault();
            }
        }
        else
        {
            MessageBox.Show("Please select a flight", "No Flight Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    [RelayCommand]
    private void AddFlight()
    {
        var viewModel = _serviceProvider.GetRequiredService<CreateFlightViewModel>();
        var window = new CreateFlightWindow(viewModel);
        viewModel.Initialize("Flight");
        var isOK = window.ShowDialog();

        if (isOK == true)
        {
            var newFlight = viewModel.Selected;

            // Add the new flight using ControlTower's AddFlight to ensure callbacks are set
            _controlTower.AddFlight(newFlight.ID, newFlight);

            RefreshFlights();
            SelectedFlight = Flights.FirstOrDefault(f => f.Name == newFlight.Name);
        }
    }


    [RelayCommand]
    private void TakeOffFlight(Flight selected)
    {
        if (selected != null)
        {
            _controlTower.OrderTakeoff(selected.ID); // Use ID to reference the flight
        }
        else
        {
            MessageBox.Show("Please select a flight", "No Flight Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }


    // Event handler for when the flight takes off
    private void OnFlightTakeOff(object sender, FlightEventArgs e)
    {
        FlightLog.Add($"Flight {e.FlightName} has taken off.");
    }

    // Event handler for when the flight lands
    private void OnFlightLanded(object sender, FlightEventArgs e)
    {
        FlightLog.Add($"Flight {e.FlightName} has landed.");
        RefreshFlights();
    }

    // Event handler for when the flight height changes
    private void OnFlightHeightChange(object sender, FlightHeightEventArgs e)
    {
        FlightLog.Add($"Flight {e.FlightName} changed altitude to {e.NewHeight} meters.");
    }

    private void RefreshFlights()
    {
        Flights.Clear();
        // Add flights from ControlTower
        foreach (var flight in _controlTower.GetAll())
        {
            Flights.Add(flight);
        }
    }
}
