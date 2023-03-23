using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TipsAndTricks.Core.Contracts;
using TipsAndTricks.Core.DTO;
using TipsAndTricks.Core.Entities;
using TipsAndTricks.Data.Contexts;
using TipsAndTricks.Services.Extensions;

namespace TipsAndTricks.Services.Blogs {
    public class AuthorRepository : IAuthorRepository {
        private readonly BlogDbContext _context;
        private readonly IMemoryCache _memoryCache;

        public AuthorRepository(BlogDbContext context, IMemoryCache memoryCache) {
            _context = context;
            _memoryCache = memoryCache;
        }

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
        /// Get cached Author by Id
        /// </summary>
        /// <param name="id">Author's Id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Author> GetCachedAuthorByIdAsync(int id, CancellationToken cancellationToken = default) {
            return await _memoryCache.GetOrCreateAsync(
            $"author.by-id.{id}",
            async (entry) => {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                return await GetAuthorByIdAsync(id);
            });
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
        /// Get cached Author by Slug
        /// </summary>
        /// <param name="slug">Author's Slug</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Author> GetCachedAuthorBySlugAsync(string slug, CancellationToken cancellationToken = default) {
            return await _memoryCache.GetOrCreateAsync(
            $"author.by-slug.{slug}",
            async (entry) => {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                return await GetAuthorBySlugAsync(slug, cancellationToken);
            });
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
        /// Get Authors
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IList<AuthorItem>> GetAuthorsAsync(CancellationToken cancellationToken = default) {
            return await _context.Set<Author>()
            .OrderBy(a => a.FullName)
            .Select(a => new AuthorItem() {
                Id = a.Id,
                FullName = a.FullName,
                Email = a.Email,
                JoinedDate = a.JoinedDate,
                ImageUrl = a.ImageUrl,
                UrlSlug = a.UrlSlug,
                PostCount = a.Posts.Count(p => p.Published)
            })
            .ToListAsync(cancellationToken);
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
        /// Paginate Authors by Name
        /// </summary>
        /// <param name="pagingParams"></param>
        /// <param name="name">Author's Name</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IPagedList<AuthorItem>> GetPagedAuthorsAsync(
        IPagingParams pagingParams,
        string name = null,
        CancellationToken cancellationToken = default) {
            var authorQuery = _context.Set<Author>()
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(name)) {
                authorQuery = authorQuery.Where(x => x.FullName.Contains(name));
            }

            return await authorQuery.Select(a => new AuthorItem() {
                Id = a.Id,
                FullName = a.FullName,
                Email = a.Email,
                JoinedDate = a.JoinedDate,
                ImageUrl = a.ImageUrl,
                UrlSlug = a.UrlSlug,
                PostCount = a.Posts.Count(p => p.Published)
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
        public async Task<IList<AuthorItem>> GetAuthorsHasMostArticles(int numberOfAuthors, CancellationToken cancellationToken = default) {
            return await _context.Set<Author>()
                .Include(p => p.Posts)
                .Select(x => new AuthorItem() {
                    Id = x.Id,
                    FullName = x.FullName,
                    UrlSlug = x.UrlSlug,
                    Email = x.Email,
                    ImageUrl = x.ImageUrl,
                    Notes = x.UrlSlug,
                    PostCount = x.Posts.Count(p => p.Published)
                })
                .OrderByDescending(x => x.PostCount)
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

        /// <summary>
        /// Paginate Authors
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mapper"></param>
        /// <param name="pagingParams"></param>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IPagedList<T>> GetPagedAuthorsAsync<T>(Func<IQueryable<Author>, IQueryable<T>> mapper, IPagingParams pagingParams, string name = null, CancellationToken cancellationToken = default) {
            var authorQuery = _context.Set<Author>().AsNoTracking();

            if (!string.IsNullOrEmpty(name)) {
                authorQuery = authorQuery.Where(x => x.FullName.Contains(name));
            }

            return await mapper(authorQuery)
                .ToPagedListAsync(pagingParams, cancellationToken);
        }

        /// <summary>
        /// Set Author's Image URL
        /// </summary>
        /// <param name="id">Author's Id</param>
        /// <param name="imageUrl">Image URL</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> SetImageUrlAsync(int id, string imageUrl,
            CancellationToken cancellationToken = default) {
            return await _context.Authors
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(x => x.SetProperty(a => a.ImageUrl, a => imageUrl), cancellationToken) > 0;
        }
        #endregion
    }
}
