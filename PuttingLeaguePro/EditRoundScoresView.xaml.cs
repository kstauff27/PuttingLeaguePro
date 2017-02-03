using System.Windows;
using ViewModels;

namespace PuttingLeaguePro
{
    /// <summary>
    /// Interaction logic for EditRoundScoresView.xaml
    /// </summary>
    public partial class EditRoundScoresView : Window
    {
        public EditRoundScoresView()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            EditRoundScoresViewModel vm = DataContext as EditRoundScoresViewModel;
            if (vm != null)
            {
                if (vm.DialogResult)
                    vm.OK();
                else
                    vm.Cancel();
            }
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            EditRoundScoresViewModel vm = DataContext as EditRoundScoresViewModel;
            if (vm != null)
                vm.DialogResult = true;

            Close();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            EditRoundScoresViewModel vm = DataContext as EditRoundScoresViewModel;
            if (vm != null)
                vm.DialogResult = false;

            Close();
        }
    }
}
