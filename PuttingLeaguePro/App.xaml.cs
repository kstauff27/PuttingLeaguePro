using System.Windows;
using CoreStuff;
using ViewModels;
using System.Data.Entity;
using DataManager;
using Models;

namespace PuttingLeaguePro
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
		public App()
        {
            Database.SetInitializer(new PuttingLeagueInitializer());

            EventManagerInstance.Instance.GetEvent<LaunchNewGame>().Subscribe(OnLaunchNewGame);
            EventManagerInstance.Instance.GetEvent<LaunchEditRoundScores>().Subscribe(OnEditRoundScores);
        }

		private void OnLaunchNewGame(int gameID)
		{
			ScoringViewModel vm = new ScoringViewModel(gameID);
			ScoringView view = new ScoringView() { DataContext = vm };

			view.ShowDialog();
		}

        private void OnEditRoundScores(EditRoundScoresData data)
        {
            EditRoundScoresViewModel vm = new EditRoundScoresViewModel(data.DataManager, data.GameID, data.TeamID, data.CurrentRound);
            EditRoundScoresView view = new EditRoundScoresView() { DataContext = vm, Owner = this.MainWindow };

            view.ShowDialog();
        }
    }



}
