using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Models;
using CoreStuff;
using System.Data;
using System.Windows.Input;
using DataManager;
using Models.Display_Models;

namespace ViewModels
{
    public class ScoringViewModel : BaseViewModel
    {
        private PuttingLeagueDb _dataManager = new PuttingLeagueDb();
        private Game _game = null;

        private int _scoreToWin = 0;
        private int _fallbackScoreOnBust = 0;

        private ObservableCollection<Team> _teams = new ObservableCollection<Team>();
        private List<PointValue> _pointValues = new List<PointValue>();

        private ObservableCollection<TeamScore> _teamScores = new ObservableCollection<TeamScore>();
        private TeamScore _currentTeamScore = null;
        private TeamScore _selectedTeamScore = null;

        private ObservableCollection<string> _pointValueHeaders = new ObservableCollection<string>();
        private List<bool> _pointValuesForCurrentTurn = new List<bool>();

        private Dictionary<int, List<PointPossibility>> _pointPossibilities = new Dictionary<int, List<PointPossibility>>();

        private int _roundNumber = 0;
        private int _counter = 0;

        // Constructor
        public ScoringViewModel(int gameID)
        {
            SetUpGame(gameID);

            CurrentTeamScore = _teamScores.FirstOrDefault();
        }

        #region Properties

        public ObservableCollection<string> PointValueHeaders
        {
            get { return _pointValueHeaders; }
        }

        public ObservableCollection<TeamScore> TeamScores
        {
            get { return _teamScores; }
        }
        public TeamScore CurrentTeamScore
        {
            get { return _currentTeamScore; }
            private set
            {
                _currentTeamScore = value;
                RaisePropertyChanged("CurrentTeamScore");
            }
        }
        public TeamScore SelectedTeamScore
        {
            get { return _selectedTeamScore; }
            set
            {
                _selectedTeamScore = value;
                RaisePropertyChanged("SelectedTeamScore");
            }
        }

        public int ScoreToWin
        {
            get { return _scoreToWin; }
            private set
            {
                _scoreToWin = value;
                RaisePropertyChanged("ScoreToWin");
            }
        }
        public int FallbackScoreOnBust
        {
            get { return _fallbackScoreOnBust; }
            private set
            {
                _fallbackScoreOnBust = value;
                RaisePropertyChanged("FallbackScoreOnBust");
            }
        }

        public bool PointValue1
        {
            get { return _pointValuesForCurrentTurn[0]; }
            set
            {
                _pointValuesForCurrentTurn[0] = value;
                RefreshPointValuesEnabled();
            }
        }
        public bool PointValue1IsEnabled
        {
            get
            {
                bool isEnabled = !_currentTeamScore.PointValuesHit[0].HitLastRound;

                if (isEnabled && !_pointValuesForCurrentTurn[0] && _pointValuesForCurrentTurn.Count(e => e == true) > 1)
                    isEnabled = false;

                return isEnabled;
            }
        }

        public bool PointValue2
        {
            get { return _pointValuesForCurrentTurn[1]; }
            set
            {
                _pointValuesForCurrentTurn[1] = value;
                RefreshPointValuesEnabled();
            }
        }
        public bool PointValue2IsEnabled
        {
            get
            {
                bool isEnabled = !_currentTeamScore.PointValuesHit[1].HitLastRound;

                if (isEnabled && !_pointValuesForCurrentTurn[1] && _pointValuesForCurrentTurn.Count(e => e == true) > 1)
                    isEnabled = false;

                return isEnabled;
            }
        }

        public bool PointValue3
        {
            get { return _pointValuesForCurrentTurn[2]; }
            set
            {
                _pointValuesForCurrentTurn[2] = value;
                RefreshPointValuesEnabled();
            }
        }
        public bool PointValue3IsEnabled
        {
            get
            {
                bool isEnabled = !_currentTeamScore.PointValuesHit[2].HitLastRound;

                if (isEnabled && !_pointValuesForCurrentTurn[2] && _pointValuesForCurrentTurn.Count(e => e == true) > 1)
                    isEnabled = false;

                return isEnabled;
            }
        }

        public bool PointValue4
        {
            get { return _pointValuesForCurrentTurn[3]; }
            set
            {
                _pointValuesForCurrentTurn[3] = value;
                RefreshPointValuesEnabled();
            }
        }
        public bool PointValue4IsEnabled
        {
            get
            {
                bool isEnabled = !_currentTeamScore.PointValuesHit[3].HitLastRound;

                if (isEnabled && !_pointValuesForCurrentTurn[3] && _pointValuesForCurrentTurn.Count(e => e == true) > 1)
                    isEnabled = false;

                return isEnabled;
            }
        }

        public bool PointValue5
        {
            get { return _pointValuesForCurrentTurn[4]; }
            set
            {
                _pointValuesForCurrentTurn[4] = value;
                RefreshPointValuesEnabled();
            }
        }
        public bool PointValue5IsEnabled
        {
            get
            {
                bool isEnabled = !_currentTeamScore.PointValuesHit[4].HitLastRound;

                if (isEnabled && !_pointValuesForCurrentTurn[4] && _pointValuesForCurrentTurn.Count(e => e == true) > 1)
                    isEnabled = false;

                return isEnabled;
            }
        }

        public bool PointValue6
        {
            get { return _pointValuesForCurrentTurn[5]; }
            set
            {
                _pointValuesForCurrentTurn[5] = value;
                RefreshPointValuesEnabled();
            }
        }
        public bool PointValue6IsEnabled
        {
            get
            {
                bool isEnabled = !_currentTeamScore.PointValuesHit[5].HitLastRound;

                if (isEnabled && !_pointValuesForCurrentTurn[5] && _pointValuesForCurrentTurn.Count(e => e == true) > 1)
                    isEnabled = false;

                return isEnabled;
            }
        }

        #endregion

        #region Commands


        private RelayCommand _turnCompleted;
        public ICommand TurnCompleted
        {
            get
            {
                if (_turnCompleted == null)
                {
                    _turnCompleted = new RelayCommand(
                        param => turnCompleted());
                }
                return _turnCompleted;
            }
        }

        private void turnCompleted()
        {
            if(_counter % _teams.Count == 0)
                _roundNumber++;

            if (_pointValuesForCurrentTurn.All(e => e == false))
            {
                RoundScore rs = new RoundScore() { GameID = _game.GameID, TeamID = _currentTeamScore.TeamID, Round = _roundNumber, Points = 0 };
                _dataManager.RoundScores.Add(rs);
                _dataManager.SaveChanges();
            }
            else
            {
                for (int i = 0; i < _pointValuesForCurrentTurn.Count; i++)
                {
                    if (_pointValuesForCurrentTurn[i])
                    {
                        RoundScore rs = new RoundScore() { GameID = _game.GameID, TeamID = _currentTeamScore.TeamID, Round = _roundNumber };
                        rs.Points = _pointValues[i].Points;

                        _dataManager.RoundScores.Add(rs);
                        _dataManager.SaveChanges();
                    }
                }
            }

            _counter++;

            UpdateTeamScore(_currentTeamScore.TeamID, _roundNumber);

            int index = _teamScores.IndexOf(_currentTeamScore);
            index = (index == (_teamScores.Count - 1)) ? 0 : index + 1;

            CurrentTeamScore = _teamScores.ElementAt(index);
            SelectedTeamScore = _teamScores.ElementAt(index);

            ResetPointValuesForCurrentTurn();
        }


        private RelayCommand _editSelectedTeamScore;
        public ICommand EditSelectedTeamScore
        {
            get
            {
                if (_editSelectedTeamScore == null)
                {
                    _editSelectedTeamScore = new RelayCommand(
                        param => editSelectedTeamScore(),
                        param => editSelectedTeamScoreCanExecute());
                }
                return _editSelectedTeamScore;
            }
        }

        private void editSelectedTeamScore()
        {
            EditRoundScoresData data = new EditRoundScoresData() { DataManager = _dataManager, GameID = _game.GameID, TeamID = _selectedTeamScore.TeamID, CurrentRound = _roundNumber };
            EventAggregator.GetEvent<LaunchEditRoundScores>().Publish(data);

            foreach(TeamScore teamScore in _teamScores)
                RefreshTeamScore(teamScore.TeamID);

            RefreshPointValuesEnabled();
        }
        private bool editSelectedTeamScoreCanExecute()
        {
            return _selectedTeamScore != null;
        }

        #endregion

        // Helper Methods
        private void SetUpGame(int gameID)
        {
            _game = _dataManager.Games.SingleOrDefault(e => e.GameID == gameID);

            _scoreToWin = _game.ScoreToWin;
            _fallbackScoreOnBust = _game.FallbackScore;

            // Point Values
            foreach (PointValue pv in _game.PointValues)
            {
                _pointValues.Add(pv);
                _pointValuesForCurrentTurn.Add(false);
            }

            GeneratePointValueHeaders();
            GeneratePointPossibilities();

            // Teams
            foreach (Team team in _game.Teams)
                _teams.Add(team);

            GenerateTeamScores();
        }
        private void GeneratePointPossibilities()
        {
            foreach(PointValue pv in _pointValues)
            {
                int index = _pointValues.IndexOf(pv);
                int points1 = pv.Points;

                for(int i = index + 1; i < _pointValues.Count; i++)
                {
                    int points2 = _pointValues[i].Points;
                    int total = points1 + points2;

                    if (!_pointPossibilities.ContainsKey(total))
                        _pointPossibilities[total] = new List<PointPossibility>();

                    _pointPossibilities[total].Add(new PointPossibility() { Points1 = points1, Points2 = points2 });
                }
            }
        }
        private void GeneratePointValueHeaders()
        {
            foreach(PointValue pv in _pointValues)
                _pointValueHeaders.Add(pv.Points.ToString());
        }
        private void GenerateTeamScores()
        {
            int order = 1;

            foreach(Team team in _teams)
            {
                TeamScore teamScore = new TeamScore() { TeamID = team.TeamID, CurrentScore = 0, Order = order };
                teamScore.TeamName = team.Players.Count % 2 == 0 ? string.Format("{0} / {1}", team.Players.Select(e => e.PlayerName).ToArray()) : team.Players.FirstOrDefault().PlayerName;

                foreach(PointValue pv in _pointValues)
                {
                    PointValueHit pvh = new PointValueHit() { PointValueID = pv.PointValueID, Points = pv.Points, Completed = false, HitLastRound = false };
                    teamScore.PointValuesHit.Add(pvh);
                }

                _teamScores.Add(teamScore);
                order++;
            }
        }
        private void ResetPointValuesForCurrentTurn()
        {
            for (int i = 0; i < _pointValuesForCurrentTurn.Count; i++)
                _pointValuesForCurrentTurn[i] = false;

            RaisePropertyChanged("PointValue1");
            RaisePropertyChanged("PointValue2");
            RaisePropertyChanged("PointValue3");
            RaisePropertyChanged("PointValue4");
            RaisePropertyChanged("PointValue5");
            RaisePropertyChanged("PointValue6");

            RaisePropertyChanged("PointValue1IsEnabled");
            RaisePropertyChanged("PointValue2IsEnabled");
            RaisePropertyChanged("PointValue3IsEnabled");
            RaisePropertyChanged("PointValue4IsEnabled");
            RaisePropertyChanged("PointValue5IsEnabled");
            RaisePropertyChanged("PointValue6IsEnabled");
        }
        private void RefreshPointValuesEnabled()
        {
            RaisePropertyChanged("PointValue1IsEnabled");
            RaisePropertyChanged("PointValue2IsEnabled");
            RaisePropertyChanged("PointValue3IsEnabled");
            RaisePropertyChanged("PointValue4IsEnabled");
            RaisePropertyChanged("PointValue5IsEnabled");
            RaisePropertyChanged("PointValue6IsEnabled");
        }
        private void UpdateTeamScore(int teamID, int round)
        {
            TeamScore teamScore = _teamScores.SingleOrDefault(e => e.TeamID == teamID);
            List<RoundScore> roundScores = _game.RoundScores.Where(e => e.TeamID == teamID).ToList();

            foreach(PointValueHit pv in teamScore.PointValuesHit)
                pv.HitLastRound = false;

            teamScore.CurrentScore = roundScores.Sum(e => e.Points);

            foreach(RoundScore rs in roundScores.Where(e => e.Round == round))
            {
                PointValueHit pvh = teamScore.PointValuesHit.SingleOrDefault(e => e.Points == rs.Points);
                if(pvh != null)
                {
                    pvh.Completed = true;
                    pvh.HitLastRound = true;
                }
            }

            if(teamScore.CurrentScore == _scoreToWin)
            {
                System.Windows.Forms.MessageBox.Show(string.Format("{0} Win!", teamScore.TeamName));
            }
            else if (teamScore.CurrentScore > _scoreToWin)
            {
                RoundScore negRoundScore = new RoundScore() { GameID = _game.GameID, TeamID = teamID, Round = round };
                negRoundScore.Points = _fallbackScoreOnBust - teamScore.CurrentScore;

                _game.RoundScores.Add(negRoundScore);
                _dataManager.SaveChanges();

                teamScore.CurrentScore = roundScores.Sum(e => e.Points) + negRoundScore.Points;
            }
        }
        private void RefreshTeamScore(int teamID)
        {
            // Get the backend data objects
            Team team = _game.Teams.SingleOrDefault(e => e.TeamID == teamID);
            IEnumerable<RoundScore> teamRoundScores = _game.RoundScores.Where(e => e.TeamID == teamID).OrderBy(e => e.Round);

            // Get the Displayed Data Object
            TeamScore teamScore = _teamScores.SingleOrDefault(e => e.TeamID == teamID);

            // Set the current score
            teamScore.CurrentScore = teamRoundScores.Sum(e => e.Points);

            // Reset the points hit
            foreach (PointValueHit pvh in teamScore.PointValuesHit)
            {
                pvh.Completed = false;
                pvh.HitLastRound = false;
            }

            // Get the distinct points hit
            IEnumerable<int> pointsHit = teamRoundScores.Select(e => e.Points).Distinct();
            foreach(int points in pointsHit)
            {
                PointValueHit pvh = teamScore.PointValuesHit.SingleOrDefault(e => e.Points == points);
                if(pvh != null)
                    pvh.Completed = true;
            }

            // Get the points hit in the latest round
            if (teamRoundScores.Count() > 0)
            {
                int maxRound = teamRoundScores.Max(e => e.Round);
                IEnumerable<int> pointsHitLastRound = teamRoundScores.Where(e => e.Round == maxRound).Select(e => e.Points);
                foreach (int points in pointsHitLastRound)
                {
                    PointValueHit pvh = teamScore.PointValuesHit.SingleOrDefault(e => e.Points == points);
                    if (pvh != null)
                        pvh.HitLastRound = true;
                }
            }
        }
    }

    public class PointPossibility
    {
        public int Points1 { get; set; }
        public int Points2 { get; set; }
    }

    public class EditRoundScoresData
    {
        public PuttingLeagueDb DataManager { get; set; }
        public int GameID { get; set; }
        public int TeamID { get; set; }
        public int CurrentRound { get; set; }
    }
}
