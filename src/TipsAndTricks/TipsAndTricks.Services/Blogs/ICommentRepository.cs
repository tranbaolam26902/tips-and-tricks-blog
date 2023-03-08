using TipsAndTricks.Core.Entities;

namespace TipsAndTricks.Services.Blogs {
    public interface ICommentRepository {
        #region Comment methods
        /// <summary>
        /// Approve a Comment to show on a Post
        /// </summary>
        /// <param name="id">Comment's Id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> ApproveCommentAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete Comment
        /// </summary>
        /// <param name="id">Comment's Id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> DeleteCommentAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get Comments of Post
        /// </summary>
        /// <param name="id">Post's Id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<Comment>> GetPostCommentsAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send Comment to a Post
        /// </summary>
        /// <param name="name">User Name</param>
        /// <param name="email">User Email</param>
        /// <param name="description">Comment's content</param>
        /// <param name="postId">Post's Id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> SendCommentAsync(string name, string email, string description, int postId, CancellationToken cancellationToken = default);
        #endregion
    }
}
