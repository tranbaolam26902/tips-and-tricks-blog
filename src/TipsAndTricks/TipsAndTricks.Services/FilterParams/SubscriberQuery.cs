using TipsAndTricks.Core.Contracts;
using TipsAndTricks.Core.Entities;

namespace TipsAndTricks.Services.FilterParams {
    public class SubscriberQuery : ISubscriberQuery {
        public string Keyword { get; set; }
        public int? LastSubscribedDay { get; set; }
        public int? LastSubscribedMonth { get; set; }
        public int? LastSubscribedYear { get; set; }
        public SubscribeState? SubscribeState { get; set; }
    }
}
