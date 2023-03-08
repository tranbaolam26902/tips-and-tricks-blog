using Microsoft.EntityFrameworkCore;
using TipsAndTricks.Core.Contracts;
using TipsAndTricks.Core.DTO;
using TipsAndTricks.Core.Entities;
using TipsAndTricks.Data.Contexts;
using TipsAndTricks.Services.Extensions;

namespace TipsAndTricks.Services.Blogs {
    public class AuthorRepository : IAuthorRepository {
        private readonly BlogDbContext _context;

        public AuthorRepository(BlogDbContext context) {
            _context = context;
        }

        #region Author methods
        /// <summary>
        /// 2b. Get Author by Id
        /// </summary>
        /// <param name="id">Author's Id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Author> GetAuthorByIdAsync(int id, CancellationToken cancellationToken = default) {
            return await _context.Set<Author>()
                .Include(p => p.Posts)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        /// <summary>
        /// 2c. Get Author by Slug
        /// </summary>
        /// <param name="slug">Author's Slug</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Author> GetAuthorBySlugAsync(string slug, CancellationToken cancellationToken = default) {
            return await _context.Set<Author>()
                .Include(p => p.Posts)
                .FirstOrDefaultAsync(x => x.UrlSlug == slug, cancellationToken);
        }

        /// <summary>
        /// 2e. Edit Author if existed, otherwise insert a new one.
        /// If Author's Id is not provided, insert a new Author with continuous Id
        /// If Author's Id is provided and existed in database, update Author with new values
        /// </summary>
        /// <param name="newAuthor">New Author</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Author> EditAuthorAsync(Author newAuthor, CancellationToken cancellationToken = default) {
            var author = await _context.Set<Author>()
                .Include(p => p.Posts)
                .AnyAsync(x => x.Id == newAuthor.Id, cancellationToken);

            if (author)
                _context.Entry(newAuthor).State = EntityState.Modified;
            else
                _context.Add(newAuthor);
            await _context.SaveChangesAsync(cancellationToken);

            return newAuthor;
        }

        /// <summary>
        /// 2d. Paginate Authors
        /// </summary>
        /// <param name="pagingParams"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IPagedList<AuthorItem>> GetPagedAuthorsAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default) {
            return await _context.Set<Author>()
                .Include(p => p.Posts)
                .Select(x => new AuthorItem() {
                    Id = x.Id,
                    FullName = x.FullName,
                    UrlSlug = x.UrlSlug,
                    Email = x.Email,
                    PostCount = x.Posts.Count(p => p.Published)
                })
                .ToPagedListAsync(pagingParams, cancellationToken);
        }

        /// <summary>
        /// 2f. Get Authors has most Articles
        /// </summary>
        /// <param name="numberOfAuthors"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IList<Author>> GetAuthorsHasMostArticles(int numberOfAuthors, CancellationToken cancellationToken = default) {
            var authorPostCount = await _context.Set<Author>()
                .Select(x => new AuthorItem() {
                    PostCount = x.Posts.Count()
                })
                .ToListAsync(cancellationToken);
            var highestPostCount = authorPostCount.Max(x => x.PostCount);

            return await _context.Set<Author>()
                .Include(p => p.Posts)
                .Where(x => x.Posts.Count(p => p.Published) == highestPostCount)
                .Take(numberOfAuthors)
                .ToListAsync(cancellationToken);
        }
        #endregion
    }
}
