using CoreStuff;
using DataManager;
using Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using System;
using Models.Display_Models;
using System.Windows.Forms;

namespace ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private PuttingLeagueDb _dataContext = null;

        private ObservableCollection<PointValue> _pointValues = new ObservableCollection<PointValue>();
        private PointValue _selectedPointValue = null;

        private ObservableCollection<Player> _players = new ObservableCollection<Player>();
        private Player _selectedPlayer = null;

        private ObservableCollection<ScoringTotal> _scoringLeaders = new ObservableCollection<ScoringTotal>();

        private string _newPlayerName = string.Empty;
        private int _scoreToWin = 101;
        private int _fallbackScoreOnBust = 80;

        // Constructor
        public MainWindowViewModel()
        {
            _dataContext = new PuttingLeagueDb();

            try
            {
                GetDefaultPointValues();
                GetPlayers();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Database could not be opened. If the problem persists, try recreating the database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        // Properties
        public ObservableCollection<PointValue> PointValues
        {
            get { return _pointValues; }
        }
        public PointValue SelectedPointValue
        {
            get { return _selectedPointValue; }
            set
            {
                _selectedPointValue = value;
                RaisePropertyChanged("SelectedPointValue");
            }
        }

        public ObservableCollection<Player> Players
        {
            get { return _players; }
        }
        public Player SelectedPlayer
        {
            get { return _selectedPlayer; }
            set
            {
                _selectedPlayer = value;
                RaisePropertyChanged("SelectedPlayer");
            }
        }

        public string NewPlayerName
        {
            get { return _newPlayerName; }
            set
            {
                _newPlayerName = value;
                RaisePropertyChanged("NewPlayerName");
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
        public int FallbackScoreOnBust
        {
            get { return _fallbackScoreOnBust; }
            set
            {
                _fallbackScoreOnBust = value;
                RaisePropertyChanged("FallbackScoreOnBust");
            }
        }

        // Commands
        private RelayCommand _addPlayer;
        private RelayCommand _startGame;
        private RelayCommand _displayScoringLeaders;
        private RelayCommand _includeAll;
        private RelayCommand _recreateDatabase;

        public ICommand AddPlayer
        {
            get
            {
                if (_addPlayer == null)
                {
                    _addPlayer = new RelayCommand(
                        param => addPlayer(),
                        param => addPlayerCanExecute());
                }
                return _addPlayer;
            }
        }
        public ICommand StartGame
        {
            get
            {
                if (_startGame == null)
                {
                    _startGame = new RelayCommand(
                        param => startGame(),
                        param => startGameCanExecute());
                }
                return _startGame;
            }
        }
        public ICommand DisplayScoringLeaders
        {
            get
            {
                if(_displayScoringLeaders == null)
                {
                    _displayScoringLeaders = new RelayCommand(
                        param => displayScoringLeaders());
                }
                return _displayScoringLeaders;
            }
        }
        public ICommand IncludeAll
        {
            get
            {
                if(_includeAll == null)
                {
                    _includeAll = new RelayCommand(
                        param => includeAll());
                }
                return _includeAll;
            }
        }
        public ICommand RecreateDatabase
        {
            get
            {
                if(_recreateDatabase == null)
                {
                    _recreateDatabase = new RelayCommand(
                        param => recreateDatabase());
                }
                return _recreateDatabase;
            }
        }

        // Command Methods
        private void addPlayer()
        {
            Player p = new Player() { Name = _newPlayerName, Included = true };

            _dataContext.Players.Add(p);
            _dataContext.SaveChanges();
            _players.Add(p);

            NewPlayerName = string.Empty;
        }
        private bool addPlayerCanExecute()
        {
            return _newPlayerName.Trim() != string.Empty;
        }

        private void startGame()
        {
            // Create the new Game
            Game newGame = new Game()
            {
                Date = DateTime.Now,
                ScoreToWin = _scoreToWin,
                FallbackScore = _fallbackScoreOnBust
            };

            foreach (PointValue pv in _pointValues)
                newGame.PointValues.Add(pv);

            _dataContext.SaveChanges();

            GenerateTeams(newGame, _players.Where(e => e.Included));

            // Save the game with all relevant team info to the database
            _dataContext.Games.Add(newGame);
            _dataContext.SaveChanges();

            // Launch the New Game with the GameID
            EventAggregator.GetEvent<LaunchNewGame>().Publish(newGame.GameID);
        }
        private bool startGameCanExecute()
        {
            return _pointValues.Count > 0 && _players.Count > 0;
        }

        private void displayScoringLeaders()
        {
            EventAggregator.GetEvent<LaunchScoringLeaders>().Publish();
        }

        private void includeAll()
        {
            if(_players.All(e => e.Included))
            {
                foreach (Player player in _players)
                    player.Included = false;
            }
            else
            {
                foreach (Player player in _players)
                    player.Included = true;
            }
        }

        private void recreateDatabase()
        {
            DialogResult result = MessageBox.Show("Recreating the database will delete all data in current database. Do you wish to continue?", "Recreate Database", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if(result == DialogResult.Yes)
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

                _dataContext.Database.Delete();
                _dataContext = new PuttingLeagueDb();

                GetDefaultPointValues();
                GetPlayers();

                Mouse.OverrideCursor = null;
            }
        }

        // Helper Methods
        private void GetDefaultPointValues()
        {
            _pointValues.Clear();

            foreach (DefaultPointValue pv in _dataContext.DefaultPointValues)
            {
                PointValue pointVal = new PointValue() { Index = pv.ID, Points = pv.Points };
                _pointValues.Add(pointVal);
            }
        }
        private void GetPlayers()
        {
            _players.Clear();

            foreach (Player p in _dataContext.Players)
                _players.Add(p);
        }
        private void GenerateTeams(Game game, IEnumerable<Player> players)
        {
            Random random = new Random();

            List<Player> startingList = new List<Player>(players);
            Stack<Player> playerStack = new Stack<Player>();

            // Push all players onto the stack at random
            while (startingList.Count > 0)
            {
                int index = random.Next(0, startingList.Count - 1);

                Player player = startingList[index];
                playerStack.Push(player);
                startingList.Remove(player);
            }

            int order = 1;

            // If there is an odd number create a team with only 1 member
            if(playerStack.Count % 2 == 1)
            {
                Team team = new Team();
                team.Players.Add(new TeamPlayer(playerStack.Pop(), team));
                team.TeamName = team.Players.FirstOrDefault().PlayerName;

                game.Teams.Add(team);

                order++;
            }

            while(playerStack.Count > 0)
            {
                Team team = new Team();
                game.Teams.Add(team);
                _dataContext.SaveChanges();

                team.Players.Add(new TeamPlayer(playerStack.Pop(), team));
                team.Players.Add(new TeamPlayer(playerStack.Pop(), team));
                team.TeamName = string.Format("{0}/{1}", team.Players.Select(e => e.PlayerName).ToArray());

                order++;
            }
        }
    }
}
