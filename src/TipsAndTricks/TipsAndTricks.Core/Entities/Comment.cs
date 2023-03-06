using TipsAndTricks.Core.Contracts;

namespace TipsAndTricks.Core.Entities {
    public class Comment : IEntity {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public DateTime PostedDate { get; set; }
        public bool IsApproved { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}
