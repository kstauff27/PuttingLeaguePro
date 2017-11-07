using CoreStuff;
using DataManager;
using Models;
using Models.Display_Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class ScoringLeadersViewModel : BaseViewModel
    {
        private PuttingLeagueDb _db = new PuttingLeagueDb();
        private ObservableCollection<ScoringTotal> _scoringLeaders = new ObservableCollection<ScoringTotal>();

        // Constructor
        public ScoringLeadersViewModel()
        {
            GetScoringLeaders();
        }

        // Properties
        public ObservableCollection<ScoringTotal> ScoringLeaders
        {
            get { return _scoringLeaders; }
        }

        // Methods
        private void GetScoringLeaders()
        {
            List<ScoringTotal> playerTotals = new List<ScoringTotal>();

            foreach (Player player in _db.Players)
            {
                ScoringTotal playerTotal = new ScoringTotal(player);
                List<int> teamIDs = _db.TeamPlayers.Where(e => e.PlayerID == player.PlayerID).Select(e => e.TeamID).Distinct().ToList();

                IEnumerable<RoundScore> scores = _db.RoundScores.Where(e => teamIDs.Contains(e.TeamID));

                if (scores.Count() > 0)
                    playerTotal.TotalScore = scores.Sum(e => e.Points);

                playerTotal.GamesPlayed = teamIDs.Count;
                playerTotal.AverageScore = playerTotal.GamesPlayed > 0 ? Math.Round(((double)playerTotal.TotalScore / playerTotal.GamesPlayed), 1) : 0;

                playerTotals.Add(playerTotal);
            }

            foreach (ScoringTotal total in playerTotals.OrderByDescending(e => e.TotalScore))
                _scoringLeaders.Add(total);
        }

    }
}
