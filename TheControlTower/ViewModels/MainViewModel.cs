using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls.Primitives;
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
        RefreshFlights();
    }

    // Command to Delete Flight
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
                UnsubscribeFromFlightEvents(selected); // Unsubscribe from events
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

    // Command to Open Flight Creation Form
    [RelayCommand]
    private void AddFlight()
    {
        var viewModel = _serviceProvider.GetRequiredService<CreateFlightViewModel>();
        var window = new CreateFlightWindow(viewModel);
        viewModel.Initialize("Flight");
        var isOK = window.ShowDialog();

        if (isOK == true)
        {
            _controlTower.Add(viewModel.Selected.ID, viewModel.Selected); // Add to ControlTower
           // SubscribeToFlightEvents(viewModel.Selected); // Subscribe to the events
            RefreshFlights();
            SelectedFlight = Flights.FirstOrDefault(f => f.Name == viewModel.Selected.Name);
           // FlightLog.Add($"Flight {viewModel.Selected.Name} added to the Control Tower.");
        }
    }

    // Command to take off a flight
    [RelayCommand]
    private void TakeOffFlight(Flight selected)
    {
        if (selected != null)
        {
            _controlTower.OrderTakeoff(selected.ID); // Use ID to reference the flight
            //MessageBox.Show($"Flight {selected.Name} has taken off!", "Flight Status", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        else
        {
            MessageBox.Show("Please select a flight", "No Flight Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    [RelayCommand]
    private void ChangeHeight(Flight selected)
    {
        MessageBox.Show("Please enter the new height for the flight", "Change Flight Height", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    // Subscribe to the events for each flight
    private void SubscribeToFlightEvents(Flight flight)
    {
        flight.FlightHeightChange += OnFlightHeightChange;
        flight.TakeOff += OnFlightTakeOff;
        flight.Landed += OnFlightLanded;
    }

    // Unsubscribe from the events when a flight is removed
    private void UnsubscribeFromFlightEvents(Flight flight)
    {
        flight.FlightHeightChange -= OnFlightHeightChange;
        flight.TakeOff -= OnFlightTakeOff;
        flight.Landed -= OnFlightLanded;
    }

    // Event handler for flight height changes
    private void OnFlightHeightChange(object sender, FlightHeightEventArgs e)
    {
        FlightLog.Add($"Flight {e.FlightName}'s height changed to {e.NewHeight} meters.");
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

    // Refresh the flight list from the ControlTower
    private void RefreshFlights()
    {
        Flights.Clear();
        // Add flights back and subscribe to their events
        foreach (var flight in _controlTower.GetAll())
        {
            Flights.Add(flight);
            if(flight.Status !="Landed")
                 SubscribeToFlightEvents(flight); // Subscribe to each flight's events
        }
    }

}
