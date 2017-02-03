using CoreStuff;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class DefaultPointValue : BaseModel
    {
        private int _id = 0;
        private int _points = 0;

        [Key]
        public int ID
        {
            get { return _id; }
            set
            {
                _id = value;
                RaisePropertyChanged("ID");
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
    }
}
