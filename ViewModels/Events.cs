using Prism.Events;

namespace ViewModels
{
    public class LaunchNewGame : PubSubEvent<int> { }
    public class LaunchEditRoundScores : PubSubEvent<EditRoundScoresData> { }
    public class LaunchScoringLeaders : PubSubEvent { }
}
