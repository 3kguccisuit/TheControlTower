using System.Windows;
using TheControlTower.ViewModels;

namespace TheControlTower.Windows
{
    /// <summary>
    /// Interaction logic for CreateFlightWindow.xaml
    /// </summary>
    public partial class CreateFlightWindow : Window
    {
        public CreateFlightWindow(CreateFlightViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            this.Closing += (sender, e) =>
            {
                var vm = DataContext as CreateFlightViewModel;
                if (vm != null && vm.OnWindowClosingCommand.CanExecute(e))
                {
                    vm.OnWindowClosingCommand.Execute(e);
                }
            };
        }
    }
}
