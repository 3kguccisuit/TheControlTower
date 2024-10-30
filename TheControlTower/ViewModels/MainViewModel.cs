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
using Microsoft.VisualBasic;
using TheControlTowerBLL.Delegate;
using TheControlTower.Contracts.ViewModels;

public partial class MainViewModel : ObservableObject, INavigationAware
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ControlTower _controlTower;
    private readonly FlightLogManager _flightLogManager;

    [ObservableProperty]
    private Flight selectedFlight;

    public ObservableCollection<Flight> Flights { get; private set; } = new ObservableCollection<Flight>();
    public ObservableCollection<string> FlightLog { get; private set; } = new ObservableCollection<string>();

    public MainViewModel(IServiceProvider serviceProvider, ControlTower controlTower, FlightLogManager flightLogManager)
    {
        _serviceProvider = serviceProvider;
        _controlTower = controlTower;
        _flightLogManager = flightLogManager;

        RefreshFlights();
        RefreshFlightLog();
    }

    [RelayCommand]
    private void DeleteFlight(Flight selected)
    {
        if (selected != null)
        {
            MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete the selected flight?\n\n{selected.Name}",
                                         "Confirm Deletion",
                                         MessageBoxButton.YesNo,
                                         MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                Flights.Remove(selected);
                _controlTower.RemoveFlight(selected); // Remove from ControlTower
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
        CreateFlightViewModel viewModel = _serviceProvider.GetRequiredService<CreateFlightViewModel>();
        CreateFlightWindow window = new CreateFlightWindow(viewModel);
        viewModel.Initialize("Flight");
        bool? isOK = window.ShowDialog();

        if (isOK == true)
        {
            Flight newFlight = viewModel.Selected;

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

    [RelayCommand]
    private void ChangeHeight(Flight selected)
    {
        if (selected != null)
        {
            // Prompt the user for the new altitude
            string input = Interaction.InputBox("Enter the new altitude:", "Change Altitude", "1000");

            if (int.TryParse(input, out int newAltitude))
            {
                _controlTower.ChangeAltitude(selected, newAltitude);
            }
            else
            {
                MessageBox.Show("Please enter a valid integer.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        else
        {
            MessageBox.Show("Please select a flight", "No Flight Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    // Event handler for when the flight takes off
    private void OnFlightTakeOff(object sender, FlightEventArgs e)
    {
        string uniqueID = Guid.NewGuid().ToString();
        FlightLog logEntry = new FlightLog(e.Message);
        _flightLogManager.Add(uniqueID, logEntry);
        FlightLog.Add(e.Message);
        //OnPropertyChanged(nameof(FlightLog));
        //RefreshFlightLog();
        RefreshFlights();
    }

    // Event handler for when the flight lands
    private void OnFlightLanded(object sender, FlightEventArgs e)
    {
        string uniqueID = Guid.NewGuid().ToString();
        FlightLog logEntry = new FlightLog(e.Message);

        _flightLogManager.Add(uniqueID, logEntry);
        FlightLog.Add(e.Message);
        //RefreshFlightLog();
        RefreshFlights();
    }

    private void OnAltitudeChange(object sender, FlightEventArgs e)
    {
        string uniqueID = Guid.NewGuid().ToString();
        FlightLog logEntry = new FlightLog(e.Message);

        _flightLogManager.Add(uniqueID, logEntry);
        FlightLog.Add(e.Message);
        //RefreshFlightLog();
        RefreshFlights();
    }

    private void RefreshFlights()
    {
        Flights.Clear();
        // Add flights from ControlTower
        foreach (Flight flight in _controlTower.GetAll())
        {
            Flights.Add(flight);
        }

    }

    private void RefreshFlightLog()
    {
        FlightLog.Clear();
        // Add flight logs from FlightLogManager
        foreach (FlightLog logEntry in _flightLogManager.GetAll())
        {
           FlightLog.Add(logEntry.Message);
        }
    }

    public void OnNavigatedTo(object parameter)
    {
        _controlTower.TakeOff += OnFlightTakeOff;
        _controlTower.Landed += OnFlightLanded;
        _controlTower.Altitude += OnAltitudeChange;
        RefreshFlights();
        RefreshFlightLog();
    }

    // Empty method to handle navigation away from the view
    public void OnNavigatedFrom()
    {
        _controlTower.TakeOff -= OnFlightTakeOff;
        _controlTower.Landed -= OnFlightLanded;
        _controlTower.Altitude -= OnAltitudeChange;
        RefreshFlights();
        RefreshFlightLog();
    }

}
