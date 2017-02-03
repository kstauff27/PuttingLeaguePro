using Prism.Events;

namespace CoreStuff
{
    public class EventManagerInstance
    {
        private static IEventAggregator _eventManager = null;

        public static IEventAggregator Instance
        {
            get
            {
                if (_eventManager == null)
                    _eventManager = new EventAggregator();

                return _eventManager;
            }
        }
    }
}
