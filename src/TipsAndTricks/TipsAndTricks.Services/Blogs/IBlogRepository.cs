using TipsAndTricks.Core.Entities;

namespace TipsAndTricks.Services.Blogs {
    public interface IBlogRepository {
        Task<Post> GetPostAsync(int year, int month, string slug, CancellationToken cancellationToken = default);
        Task<IList<Post>> GetPopularArticlesAsync(int numPosts, CancellationToken cancellationToken = default);
        Task<bool> IsPostPlugExistedAsync(int postId, string slug, CancellationToken cancellationToken = default);
        Task IncreaseViewCountAsync(int postId, CancellationToken cancellationToken = default);
    }
}
