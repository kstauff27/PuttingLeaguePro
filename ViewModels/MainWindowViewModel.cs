using CoreStuff;
using DataManager;
using Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using System;

namespace ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private PuttingLeagueDb _dataContext = new PuttingLeagueDb();

        private ObservableCollection<PointValue> _pointValues = new ObservableCollection<PointValue>();
        private PointValue _selectedPointValue = null;

        private ObservableCollection<Player> _players = new ObservableCollection<Player>();
        private Player _selectedPlayer = null;

        private string _newPlayerName = string.Empty;
        private int _scoreToWin = 101;
        private int _fallbackScoreOnBust = 80;

        // Constructor
        public MainWindowViewModel()
        {
            GetDefaultPointValues();
            GetPlayers();
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
        private RelayCommand _selectAllPlayers;

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
        public ICommand SelectAllPlayers
        {
            get
            {
                if(_selectAllPlayers == null)
                {
                    _selectAllPlayers = new RelayCommand(
                        param => selectAllPlayers(),
                        param => selectAllPlayersCanExecute());
                }
                return _selectAllPlayers;
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

        private void selectAllPlayers()
        {
            foreach (Player player in _players)
                player.Included = true;
        }
        private bool selectAllPlayersCanExecute()
        {
            return _players.Count > 0;
        }

        // Helper Methods
        private void GetDefaultPointValues()
        {
            foreach (DefaultPointValue pv in _dataContext.DefaultPointValues)
            {
                PointValue pointVal = new PointValue() { Index = pv.ID, Points = pv.Points };
                _pointValues.Add(pointVal);
            }
        }
        private void GetPlayers()
        {
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
                team.Players.Add(playerStack.Pop());
                team.TeamName = team.Players.FirstOrDefault().Name;

                game.Teams.Add(team);

                order++;
            }

            while(playerStack.Count > 0)
            {
                Team team = new Team();
                team.Players.Add(playerStack.Pop());
                team.Players.Add(playerStack.Pop());
                team.TeamName = string.Format("{0}/{1}", team.Players.Select(e => e.Name).ToArray());

                game.Teams.Add(team);

                order++;
            }
        }

    }
}
