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
        /// Delete Author by Id
        /// </summary>
        /// <param name="id">Author's Id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAuthorByIdAsync(int id, CancellationToken cancellationToken = default) {
            return await _context.Set<Author>()
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync(cancellationToken) > 0;
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
                    ImageUrl = x.ImageUrl,
                    JoinedDate = x.JoinedDate,
                    Email = x.Email,
                    Notes = x.Notes,
                    PostCount = x.Posts.Count(p => p.Published)
                })
                .ToPagedListAsync(pagingParams, cancellationToken);
        }

        /// <summary>
        /// Filter Authors by queries
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IQueryable<AuthorItem> FilterAuthors(IAuthorQuery query) {
            IQueryable<AuthorItem> authorQuery = _context.Set<Author>()
                .Select(x => new AuthorItem() {
                    Id = x.Id,
                    FullName = x.FullName,
                    UrlSlug = x.UrlSlug,
                    ImageUrl = x.ImageUrl,
                    JoinedDate = x.JoinedDate,
                    Email = x.Email,
                    Notes = x.Notes,
                    PostCount = x.Posts.Count(p => p.Published)
                });

            if (!string.IsNullOrWhiteSpace(query.Keyword)) {
                authorQuery = authorQuery.Where(x => x.FullName.Contains(query.Keyword) ||
                                                    x.Notes.Contains(query.Keyword) ||
                                                    x.Email.Contains(query.Keyword));
            }
            if (query.JoinedMonth != null) {
                authorQuery = authorQuery.Where(x => x.JoinedDate.Month == query.JoinedMonth);
            }
            if (query.JoinedYear != null) {
                authorQuery = authorQuery.Where(x => x.JoinedDate.Year == query.JoinedYear);
            }

            return authorQuery;
        }

        /// <summary>
        /// Paginate Authors found by queries
        /// </summary>
        /// <param name="query"></param>
        /// <param name="pagingParams"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IPagedList<AuthorItem>> GetPagedAuthorsByQueryAsync(IAuthorQuery query, IPagingParams pagingParams, CancellationToken cancellationToken = default) {
            return await FilterAuthors(query).ToPagedListAsync(pagingParams, cancellationToken);
        }

        /// <summary>
        /// 2f. Get Authors has most Articles
        /// </summary>
        /// <param name="numberOfAuthors"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IList<Author>> GetAuthorsHasMostArticles(int numberOfAuthors, CancellationToken cancellationToken = default) {
            return await _context.Set<Author>()
                .OrderByDescending(x => x.Posts.Count)
                .Take(numberOfAuthors)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Check whether Author's Slug is existed
        /// </summary>
        /// <param name="id">Author's Id</param>
        /// <param name="slug">Author's Slug</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> IsAuthorSlugExistedAsync(int id, string slug, CancellationToken cancellationToken = default) {
            return await _context.Set<Author>()
                .AnyAsync(x => x.Id != id && x.UrlSlug == slug, cancellationToken);
        }
        #endregion
    }
}
