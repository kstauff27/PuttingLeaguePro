using CoreStuff;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Team : BaseModel
    {
        private int _teamID = 0;
        private ObservableCollection<TeamPlayer> _players = new ObservableCollection<TeamPlayer>();
        private string _teamName = string.Empty;

        [Key]
        public int TeamID
        {
            get { return _teamID; }
            set
            {
                _teamID = value;
                RaisePropertyChanged("TeamID");
            }
        }
        public virtual ICollection<TeamPlayer> Players
        {
            get { return _players; }
            set
            {
                _players = value as ObservableCollection<TeamPlayer>;
                RaisePropertyChanged("Players");
            }
        }
        public string TeamName
        {
            get { return _teamName; }
            set
            {
                _teamName = value;
                RaisePropertyChanged("TeamName");
            }
        }
    }
}
