using Microsoft.EntityFrameworkCore;
using TipsAndTricks.Core.Contracts;
using TipsAndTricks.Core.Entities;
using TipsAndTricks.Data.Contexts;
using TipsAndTricks.Services.Extensions;

namespace TipsAndTricks.Services.Blogs {
    public class CommentRepository : ICommentRepository {
        private readonly BlogDbContext _context;

        public CommentRepository(BlogDbContext context) {
            _context = context;
        }

        #region Comment methods
        /// <summary>
        /// Change Comment's Approved state
        /// </summary>
        /// <param name="id">Comment's Id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> ChangeCommentApprovedState(int id, CancellationToken cancellationToken = default) {
            return await _context.Set<Comment>()
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(c => c.SetProperty(x => x.IsApproved, x => !x.IsApproved), cancellationToken) > 0;
        }

        /// <summary>
        /// Delete Comment
        /// </summary>
        /// <param name="id">Comment's Id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> DeleteCommentByIdAsync(int id, CancellationToken cancellationToken = default) {
            return await _context.Set<Comment>()
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync(cancellationToken) > 0;
        }

        /// <summary>
        /// Filter Comments by queries
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IQueryable<Comment> FilterComments(ICommentQuery query) {
            IQueryable<Comment> commentQuery = _context.Set<Comment>();

            if (!string.IsNullOrWhiteSpace(query.Keyword)) {
                commentQuery = commentQuery.Where(x => x.Name.Contains(query.Keyword) ||
                                                        x.Description.Contains(query.Keyword) ||
                                                        x.Post.Title.Contains(query.Keyword));
            }
            if (query.PostedMonth != null) {
                commentQuery = commentQuery.Where(x => x.PostedDate.Month == query.PostedMonth);
            }
            if (query.PostedYear != null) {
                commentQuery = commentQuery.Where(x => x.PostedDate.Year == query.PostedYear);
            }
            if (query.IsNotApproved) {
                commentQuery = commentQuery.Where(x => !x.IsApproved);
            }
            if (query.PostId != null) {
                commentQuery = commentQuery.Where(x => x.PostId == query.PostId);
            }

            return commentQuery;
        }

        /// <summary>
        /// Paginate Comments found by queries
        /// </summary>
        /// <param name="query"></param>
        /// <param name="pagingParams"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IPagedList<Comment>> GetPagedCommentsByQueryAsync(ICommentQuery query, IPagingParams pagingParams, CancellationToken cancellationToken = default) {
            return await FilterComments(query).ToPagedListAsync(pagingParams);
        }

        /// <summary>
        /// Get Comments of Post
        /// </summary>
        /// <param name="id">Post's Id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IList<Comment>> GetPostCommentsAsync(int id, CancellationToken cancellationToken = default) {
            return await _context.Set<Comment>()
                .Where(x => x.PostId == id && x.IsApproved)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Send Comment to a Post
        /// </summary>
        /// <param name="name">User Name</param>
        /// <param name="description">Comment's content</param>
        /// <param name="postId">Post's Id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> SendCommentAsync(string name, string description, int postId, CancellationToken cancellationToken = default) {
            if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(description)) {
                var comment = new Comment() {
                    Name = name,
                    Description = description,
                    PostId = postId,
                    PostedDate = DateTime.Now
                };
                await _context.Set<Comment>().AddAsync(comment, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return true;
            }
            return false;
        }
        #endregion
    }
}
