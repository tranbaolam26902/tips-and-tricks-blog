using TipsAndTricks.Core.Contracts;
using TipsAndTricks.Core.Entities;

namespace TipsAndTricks.Services.Blogs {
    public interface ICommentRepository {
        #region Comment methods
        /// <summary>
        /// Toggle Comment's Approved state
        /// </summary>
        /// <param name="id">Comment's Id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task ChangeCommentApprovedState(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete Comment
        /// </summary>
        /// <param name="id">Comment's Id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> DeleteCommentByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Filter Comments by queries
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IQueryable<Comment> FilterComments(ICommentQuery query);

        /// <summary>
        /// Paginate Comments found by queries
        /// </summary>
        /// <param name="query"></param>
        /// <param name="pagingParams"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IPagedList<Comment>> GetPagedCommentsByQueryAsync(ICommentQuery query, IPagingParams pagingParams, CancellationToken cancellationToken = default);

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
        /// <param name="description">Comment's content</param>
        /// <param name="postId">Post's Id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> SendCommentAsync(string name, string description, int postId, CancellationToken cancellationToken = default);
        #endregion
    }
}
