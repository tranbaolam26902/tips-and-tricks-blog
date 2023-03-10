using TipsAndTricks.Core.Entities;

namespace TipsAndTricks.Core.DTO {
    public class PostItem {
        public string CategoryName { get; set; }
        public IList<Tag> Tags { get; set; }
    }
}
