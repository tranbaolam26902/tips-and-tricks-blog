using TipsAndTricks.Core.Contracts;

namespace TipsAndTricks.Core.Entities {
    public enum SubscribeState {
        Subscribe,
        Unsubscribe,
        Banned
    }
    public class Subscriber : IEntity {
        public int Id { get; set; }
        public string Email { get; set; }
        public DateTime SubscribedDate { get; set; }
        public DateTime? UnsubscribedDate { get; set; }
        public SubscribeState SubscribeState { get; set; }
        public SubscribeState PreviousBannedState { get; set; }
        public string Reason { get; set; }
        public string Notes { get; set; }
    }
}
