using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using TheControlTowerBLL.Models;

namespace TheControlTower.ViewModels
{
    public partial class CreateFlightViewModel : ObservableObject
    {
        public IRelayCommand<CancelEventArgs> OnWindowClosingCommand { get; }

        [ObservableProperty]
        private Flight selected;

        private bool _isCancelConfirmed = false;
        private bool _isSaved = false;

        // List of possible statuses for flights
        public List<string> Statuses { get; } = new List<string> { "Ready", "In-Flight", "Landed" };

        public CreateFlightViewModel()
        {
            OnWindowClosingCommand = new RelayCommand<CancelEventArgs>(OnWindowClosing);
        }

        // Initialize the flight with basic data
        public void Initialize(string type)
        {
            var id = Guid.NewGuid().ToString();
            if (type == "Flight")
            {
                // Directly initialize the Selected flight with necessary properties
                Selected = new Flight(id, "Flight 123", "New York", 5.0); // Flight duration is set as 5 hours
            }
        }

        // Command to handle the Save action
        [RelayCommand]
        private void Save(Window window)
        {
            if (string.IsNullOrEmpty(Selected.Name) || string.IsNullOrEmpty(Selected.Destination))
            {
                MessageBox.Show("Flight Name and Destination are required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Proceed with saving the flight
            _isSaved = true;
            window.DialogResult = true; // Mark the window's dialog result as true for saving
            window.Close(); // Close the window
        }

        // Command to handle the Cancel action
        [RelayCommand]
        private void Cancel(Window window)
        {
            var result = MessageBox.Show("Do you really want to cancel?", "Cancel Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                _isCancelConfirmed = true;
                window.DialogResult = false; // Cancelled result
                window.Close(); // Close the window
            }
        }

        // This method will be invoked when the window is trying to close
        public void OnWindowClosing(CancelEventArgs e)
        {
            Window window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            if (_isSaved)
            {
                window.DialogResult = true; // Save successful
            }
            else if (!_isCancelConfirmed)
            {
                var result = MessageBox.Show("Do you really want to cancel?", "Cancel Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.No)
                {
                    e.Cancel = true;  // Prevent the window from closing
                }
                else
                {
                    window.DialogResult = false; // Cancel confirmed
                }
            }
        }
    }
}
