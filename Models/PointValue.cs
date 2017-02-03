using CoreStuff;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class PointValue : BaseModel
    {
        private int _gameID = 0;
        private int _pointValueID = 0;
        private int _points = 0;
        private int _index = 0;

        [Key]
        public int PointValueID
        {
            get { return _pointValueID; }
            set
            {
                _pointValueID = value;
                RaisePropertyChanged("PointValueID");
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
        public int Points
        {
            get { return _points; }
            set
            {
                _points = value;
                RaisePropertyChanged("Points");
            }
        }
        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                RaisePropertyChanged("Index");
            }
        }

        public virtual Game Game { get; set; }
    }
}
