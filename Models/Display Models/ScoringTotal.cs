using CoreStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Display_Models
{
    public class ScoringTotal : BaseModel
    {
        private int _playerID = 0;
        private string _playerName = string.Empty;
        private int _totalScore = 0;
        private int _gamesPlayed = 0;
        private double _averageScore = 0;

        // Constructor
        public ScoringTotal(Player player)
        {
            _playerID = player.PlayerID;
            _playerName = player.Name;
        }

        public int PlayerID
        {
            get { return _playerID; }
            set
            {
                _playerID = value;
                RaisePropertyChanged("PlayerID");
            }
        }
        public string PlayerName
        {
            get { return _playerName; }
            set
            {
                _playerName = value;
                RaisePropertyChanged("PlayerName");
            }
        }
        public int TotalScore
        {
            get { return _totalScore; }
            set
            {
                _totalScore = value;
                RaisePropertyChanged("TotalScore");
            }
        }
        public int GamesPlayed
        {
            get { return _gamesPlayed; }
            set
            {
                _gamesPlayed = value;
                RaisePropertyChanged("GamesPlayed");
            }
        }
        public double AverageScore
        {
            get { return _averageScore; }
            set
            {
                _averageScore = value;
                RaisePropertyChanged("AverageScore");
            }
        }
    }
}
