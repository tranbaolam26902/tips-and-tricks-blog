using TipsAndTricks.Core.Entities;

namespace TipsAndTricks.Services.Blogs {
    public interface ICommentRepository {
        /// <summary>
        /// Send Comment to a Post
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="description"></param>
        /// <param name="postId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> SendCommentAsync(string name, string email, string description, int postId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Approve a Comment to show on a Post
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> ApproveCommentAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete Comment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> DeleteCommentAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get Comments of Post
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<Comment>> GetPostCommentsAsync(int id, CancellationToken cancellationToken = default);
    }
}
