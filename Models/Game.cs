using CoreStuff;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Game : BaseModel
    {
        private int _gameID = 0;
        private DateTime _date = DateTime.MinValue;
        private int _scoreToWin = 0;
        private int _fallbackScore = 0;

        private ObservableCollection<PointValue> _pointValues = new ObservableCollection<PointValue>();
        private ObservableCollection<Team> _teams = new ObservableCollection<Team>();
        private ObservableCollection<RoundScore> _roundScores = new ObservableCollection<RoundScore>();


        [Key]
        public int GameID
        {
            get { return _gameID; }
            set
            {
                _gameID = value;
                RaisePropertyChanged("GameID");
            }
        }
        public DateTime Date
        {
            get { return _date; }
            set
            {
                _date = value;
                RaisePropertyChanged("Date");
            }
        }
        public int ScoreToWin
        {
            get { return _scoreToWin; }
            set
            {
                _scoreToWin = value;
                RaisePropertyChanged("ScoreToWin");
            }
        }
        public int FallbackScore
        {
            get { return _fallbackScore; }
            set
            {
                _fallbackScore = value;
                RaisePropertyChanged("FallbackScore");
            }
        }

        public virtual ICollection<PointValue> PointValues
        {
            get { return _pointValues; }
            set
            {
                _pointValues = value as ObservableCollection<PointValue>;
                RaisePropertyChanged("PointValues");
            }
        }
        public virtual ICollection<Team> Teams
        {
            get { return _teams; }
            set
            {
                _teams = value as ObservableCollection<Team>;
                RaisePropertyChanged("Teams");
            }
        }
        public virtual ICollection<RoundScore> RoundScores
        {
            get { return _roundScores; }
            set
            {
                _roundScores = value as ObservableCollection<RoundScore>;
                RaisePropertyChanged("RoundScores");
            }
        }
    }
}
