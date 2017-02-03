using CoreStuff;
using System.Collections.ObjectModel;

namespace Models.Display_Models
{
    public class TeamScore : BaseModel
    {
        private int _teamID = 0;
        private string _teamName = string.Empty;
        private int _order = 0;
        private int _currentScore = 0;
        private ObservableCollection<PointValueHit> _pointValuesHit = new ObservableCollection<PointValueHit>();

        public int TeamID
        {
            get { return _teamID; }
            set
            {
                _teamID = value;
                RaisePropertyChanged("TeamID");
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
        public int Order
        {
            get { return _order; }
            set
            {
                _order = value;
                RaisePropertyChanged("Order");
            }
        }
        public int CurrentScore
        {
            get { return _currentScore; }
            set
            {
                _currentScore = value;
                RaisePropertyChanged("CurrentScore");
            }
        }
        public ObservableCollection<PointValueHit> PointValuesHit
        {
            get { return _pointValuesHit; }
        }
    }
}
