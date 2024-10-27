using System.Windows.Controls;

using TheControlTower.ViewModels;

namespace TheControlTower.Views;

public partial class SettingsPage : Page
{
    public SettingsPage(SettingsViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
