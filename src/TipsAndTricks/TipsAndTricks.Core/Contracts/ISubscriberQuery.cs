using TipsAndTricks.Core.Entities;

namespace TipsAndTricks.Core.Contracts {
    public interface ISubscriberQuery {
        public string Keyword { get; set; }
        public int? LastSubscribedMonth { get; set; }
        public int? LastSubscribedYear { get; set; }
        public SubscribeState? SubscribeState { get; set; }
    }
}
