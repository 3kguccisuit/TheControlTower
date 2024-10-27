using System.Windows.Controls;

using TheControlTower.ViewModels;

namespace TheControlTower.Views;

public partial class MainPage : Page
{
    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
