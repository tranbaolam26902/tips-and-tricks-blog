using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using TipsAndTricks.Core.Entities;
using TipsAndTricks.Data.Contexts;

namespace TipsAndTricks.Services.Blogs {
    public class CommentRepository : ICommentRepository {
        private readonly BlogDbContext _context;

        public CommentRepository(BlogDbContext context) {
            _context = context;
        }

        #region Comment methods
        /// <summary>
        /// Approve a Comment to show on a Post
        /// </summary>
        /// <param name="id">Comment's Id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> ApproveCommentAsync(int id, CancellationToken cancellationToken = default) {
            var comment = await _context.Set<Comment>().FirstOrDefaultAsync(x => x.Id == id && x.IsApproved == false);
            if (comment == null)
                return true;

            comment.IsApproved = true;
            _context.Entry(comment).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        /// <summary>
        /// Delete Comment
        /// </summary>
        /// <param name="id">Comment's Id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> DeleteCommentAsync(int id, CancellationToken cancellationToken = default) {
            return await _context.Set<Comment>()
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync(cancellationToken) > 0;
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
        /// <param name="email">User Email</param>
        /// <param name="description">Comment's content</param>
        /// <param name="postId">Post's Id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> SendCommentAsync(string name, string email, string description, int postId, CancellationToken cancellationToken = default) {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if (!regex.Match(email).Success)
                return false;

            if (!string.IsNullOrWhiteSpace(name) || !string.IsNullOrWhiteSpace(description)) {
                var comment = new Comment() {
                    Name = name,
                    Email = email,
                    Description = description,
                    PostId = postId,
                    PostedDate = DateTime.Now
                };
                _context.Set<Comment>().Add(comment);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return true;
        }
        #endregion
    }
}
