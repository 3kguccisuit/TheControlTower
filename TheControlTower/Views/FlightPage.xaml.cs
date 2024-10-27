using System.Windows.Controls;

using TheControlTower.ViewModels;

namespace TheControlTower.Views;

public partial class FlightPage : Page
{
    public FlightPage(FlightViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
