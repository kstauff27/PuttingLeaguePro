using CoreStuff;

namespace Models.Display_Models
{
    public class PointValueHit : BaseModel
    {
        private int _pointValueID = 0;
        private int _points = 0;
        private bool _completed = false;
        private bool _hitLastRound = false;

        public int PointValueID
        {
            get { return _pointValueID; }
            set
            {
                _pointValueID = value;
                RaisePropertyChanged("PointValueID");
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
        public bool Completed
        {
            get { return _completed; }
            set
            {
                _completed = value;
                RaisePropertyChanged("Completed");
            }
        }
        public bool HitLastRound
        {
            get { return _hitLastRound; }
            set
            {
                _hitLastRound = value;
                RaisePropertyChanged("HitLastRound");
            }
        }
    }
}
