﻿using CoreStuff;
using DataManager;
using Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows.Input;

namespace ViewModels
{
    public class EditRoundScoresViewModel : BaseViewModel
    {
        private PuttingLeagueDb _dataManager = null;
        private int _gameID = 0;
        private int _teamID = 0;

        private ObservableCollection<Team> _teams = new ObservableCollection<Team>();
        private Team _selectedTeam = null;

        private List<RoundScore> _allRoundScores = new List<RoundScore>();
        private ObservableCollection<RoundScore> _roundScores = new ObservableCollection<RoundScore>();
        private RoundScore _selectedRoundScore = null;

        // Constructor
        public EditRoundScoresViewModel(PuttingLeagueDb db, int gameID, int teamID)
        {
            _dataManager = db;
            _gameID = gameID;
            _teamID = teamID;

            // Get the game
            Game game = _dataManager.Games.SingleOrDefault(e => e.GameID == _gameID);

            // Get the teams
            foreach (Team team in game.Teams)
                _teams.Add(team);

            // Get all of the round scores
            foreach (RoundScore rs in game.RoundScores)
                _allRoundScores.Add(rs);

            // Set the entity state to detached so changes aren't automatically saved
            foreach (RoundScore rs in _allRoundScores)
                _dataManager.Entry(rs).State = EntityState.Detached;

            SelectedTeam = _teams.SingleOrDefault(e => e.TeamID == teamID);
            DialogResult = false;
        }

        // Properties
        public ObservableCollection<Team> Teams
        {
            get { return _teams; }
        }
        public Team SelectedTeam
        {
            get { return _selectedTeam; }
            set
            {
                _selectedTeam = value;
                RaisePropertyChanged("SelectedTeam");
                GetRoundScores();
            }
        }

        public ObservableCollection<RoundScore> RoundScores
        {
            get { return _roundScores; }
        }
        public RoundScore SelectedRoundScore
        {
            get { return _selectedRoundScore; }
            set
            {
                _selectedRoundScore = value;
                RaisePropertyChanged("SelectedRoundScore");
            }
        }

        public bool DialogResult { get; set; }

        // Commands
        private RelayCommand _addRoundScore;
        private RelayCommand _deleteRoundScore;

        public ICommand AddRoundScore
        {
            get
            {
                if (_addRoundScore == null)
                {
                    _addRoundScore = new RelayCommand(
                        param => addRoundScore(),
                        param => addRoundScoreCanExecute());
                }
                return _addRoundScore;
            }
        }
        public ICommand DeleteRoundScore
        {
            get
            {
                if (_deleteRoundScore == null)
                {
                    _deleteRoundScore = new RelayCommand(
                        param => deleteRoundScore(),
                        param => deleteRoundScoreCanExecute());
                }
                return _deleteRoundScore;
            }
        }

        private void addRoundScore()
        {
            RoundScore rs = new RoundScore() {
                GameID = _gameID,
                TeamID = _selectedTeam.TeamID,
                Points = 0,
                Round = 1
            };

            _allRoundScores.Add(rs);
            _roundScores.Add(rs);
            _dataManager.Entry(rs).State = EntityState.Added;

            SelectedRoundScore = rs;
        }
        private bool addRoundScoreCanExecute()
        {
            return _selectedTeam != null;
        }

        private void deleteRoundScore()
        {
            _dataManager.Entry(_selectedRoundScore).State = EntityState.Deleted;
            _roundScores.Remove(_selectedRoundScore);
        }
        private bool deleteRoundScoreCanExecute()
        {
            return _selectedRoundScore != null;
        }

        public void OK()
        {
            foreach (RoundScore rs in _allRoundScores)
            {
                var entry = _dataManager.Entry(rs);

                if (entry.State == EntityState.Detached)
                    entry.State = EntityState.Modified;
            }

            _dataManager.SaveChanges();
        }
        public void Cancel()
        {
            foreach (RoundScore rs in _allRoundScores)
                _dataManager.Entry(rs).State = EntityState.Detached;
        }


        // Helpers
        private void GetRoundScores()
        {
            _roundScores.Clear();

            if(_selectedTeam != null)
            {
                foreach (RoundScore rs in _allRoundScores.Where(e => e.GameID == _gameID && e.TeamID == _selectedTeam.TeamID).OrderBy(e => e.Round))
                    _roundScores.Add(rs);
            }
        }
    }
}