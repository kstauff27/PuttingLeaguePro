using CoreStuff;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Player : BaseModel
    {
        private int _playerID = 0;
        private string _name = string.Empty;
        private bool _included = false;

        [Key]
        public int PlayerID
        {
            get { return _playerID; }
            set
            {
                _playerID = value;
                RaisePropertyChanged("PlayerID");
            }
        }
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }

        [NotMapped]
        public bool Included
        {
            get { return _included; }
            set
            {
                _included = value;
                RaisePropertyChanged("Included");
            }
        }
    }
}
