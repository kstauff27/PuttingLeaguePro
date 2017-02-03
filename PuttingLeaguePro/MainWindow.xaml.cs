using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ViewModels;

namespace PuttingLeaguePro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        private void NewPlayerNameChanged(object sender, TextChangedEventArgs e)
        {
            MainWindowViewModel vm = DataContext as MainWindowViewModel;
            if(vm != null)
                vm.NewPlayerName = (sender as TextBox)?.Text;
        }

        private void AddNewPlayerOnEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                MainWindowViewModel vm = DataContext as MainWindowViewModel;
                if (vm != null && vm.AddPlayer.CanExecute(null))
                {
                    vm.AddPlayer.Execute(null);

                    (sender as TextBox)?.Focus();
                }
            }
        }
    }
}
