using Prism.Events;

namespace CoreStuff
{
    public abstract class BaseViewModel : BaseModel
    {
        public IEventAggregator EventAggregator
        {
            get { return EventManagerInstance.Instance; }
        }
    }
}
