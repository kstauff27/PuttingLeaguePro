using CoreStuff;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class TeamPlayer : BaseModel
    {
        private int _teamPlayerID = 0;
        private int _teamID = 0;
        private int _playerID = 0;
        private string _playerName = string.Empty;

        // Constructors
        public TeamPlayer() { }
        public TeamPlayer(Player player)
        {
            _playerID = player.PlayerID;
            _playerName = player.Name;
        }
        public TeamPlayer(Player player, Team team)
        {
            _playerID = player.PlayerID;
            _playerName = player.Name;
            _teamID = team.TeamID;
        }

        [Key]
        public int TeamPlayerID
        {
            get { return _teamPlayerID; }
            set
            {
                _teamPlayerID = value;
                RaisePropertyChanged("TeamPlayerID");
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
    }
}
