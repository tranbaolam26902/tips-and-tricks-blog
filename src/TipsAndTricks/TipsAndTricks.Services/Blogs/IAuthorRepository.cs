using TipsAndTricks.Core.Contracts;
using TipsAndTricks.Core.DTO;
using TipsAndTricks.Core.Entities;

namespace TipsAndTricks.Services.Blogs {
    public interface IAuthorRepository {
        #region Author methods
        /// <summary>
        /// 2b. Get Author by Id
        /// </summary>
        /// <param name="id">Author's Id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Author> GetAuthorByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get cached Author by Id
        /// </summary>
        /// <param name="id">Author's Id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Author> GetCachedAuthorByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// 2c. Get Author by Slug
        /// </summary>
        /// <param name="slug">Author's Slug</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Author> GetAuthorBySlugAsync(string slug, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get cached Author by Slug
        /// </summary>
        /// <param name="slug">Author's Slug</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Author> GetCachedAuthorBySlugAsync(string slug, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete Author by Id
        /// </summary>
        /// <param name="id">Author's Id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> DeleteAuthorByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// 2e. Edit Author if existed, otherwise insert a new one.
        /// If Author's Id is not provided, insert a new Author with continuous Id
        /// If Author's Id is provided and existed in database, update Author with new values
        /// </summary>
        /// <param name="newAuthor">New Author</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Author> EditAuthorAsync(Author newAuthor, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get Authors
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<AuthorItem>> GetAuthorsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 2d. Paginate Authors
        /// </summary>
        /// <param name="pagingParams"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IPagedList<AuthorItem>> GetPagedAuthorsAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default);

        /// <summary>
        /// Filter Authors by queries
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        IQueryable<AuthorItem> FilterAuthors(IAuthorQuery query);

        /// <summary>
        /// Paginate Authors found by queries
        /// </summary>
        /// <param name="query"></param>
        /// <param name="pagingParams"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IPagedList<AuthorItem>> GetPagedAuthorsByQueryAsync(IAuthorQuery query, IPagingParams pagingParams, CancellationToken cancellationToken = default);

        /// <summary>
        /// 2f. Get Authors has most Articles
        /// </summary>
        /// <param name="numberOfAuthors"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<Author>> GetAuthorsHasMostArticles(int numberOfAuthors, CancellationToken cancellationToken = default);

        /// <summary>
        /// Check whether Author's Slug is existed
        /// </summary>
        /// <param name="id">Author's Id</param>
        /// <param name="slug">Author's Slug</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> IsAuthorSlugExistedAsync(int id, string slug, CancellationToken cancellationToken = default);

        /// <summary>
        /// Paginate Authors
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mapper"></param>
        /// <param name="pagingParams"></param>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IPagedList<T>> GetPagedAuthorsAsync<T>(Func<IQueryable<Author>, IQueryable<T>> mapper, IPagingParams pagingParams, string name = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Set Author's Image URL
        /// </summary>
        /// <param name="id">Author's Id</param>
        /// <param name="imageUrl">Image URL</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> SetImageUrlAsync(int id, string imageUrl,
            CancellationToken cancellationToken = default);
        #endregion
    }
}
