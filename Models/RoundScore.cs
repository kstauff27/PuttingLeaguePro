using CoreStuff;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class RoundScore : BaseModel
    {
        private int _roundScoreID = 0;
        private int _gameID = 0;
        private int _teamID = 0;
        private int _round = 0;
        private int _points = 0;

        [Key]
        public int RoundScoreID
        {
            get { return _roundScoreID; }
            set
            {
                _roundScoreID = value;
                RaisePropertyChanged("RoundScoreID");
            }
        }
        public int GameID
        {
            get { return _gameID; }
            set
            {
                _gameID = value;
                RaisePropertyChanged("GameID");
            }
        }
        public int TeamID
        {
            get { return _teamID; }
            set
            {
                _teamID = value;
                RaisePropertyChanged("TeamID");
            }
        }
        public int Round
        {
            get { return _round; }
            set
            {
                _round = value;
                RaisePropertyChanged("Round");
            }
        }
        public int Points
        {
            get { return _points; }
            set
            {
                _points = value;
                RaisePropertyChanged("Points");
            }
        }

        public virtual Game Game { get; set; }
        public virtual Team Team { get; set; }
    }
}
