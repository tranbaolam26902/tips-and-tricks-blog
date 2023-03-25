namespace TipsAndTricks.WebApi.Models.Dashboard {
    public class DashboardModel {
        public int TotalPosts { get; set; }
        public int TotalUnpublishedPosts { get; set; }
        public int TotalCategories { get; set; }
        public int TotalAuthors { get; set; }
        public int TotalNotApprovedComments { get; set; }
        public int TotalSubscribers { get; set; }
        public int TotalSubscriberToday { get; set; }
    }
}
