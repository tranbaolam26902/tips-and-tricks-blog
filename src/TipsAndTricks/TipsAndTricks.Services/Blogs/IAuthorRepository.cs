using TipsAndTricks.Core.Contracts;
using TipsAndTricks.Core.DTO;
using TipsAndTricks.Core.Entities;

namespace TipsAndTricks.Services.Blogs {
    public interface IAuthorRepository {
        #region Author methods
        /// <summary>
        /// 2b. Get Author by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Author> GetAuthorByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// 2c. Get Author by Slug
        /// </summary>
        /// <param name="slug"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Author> GetAuthorBySlugAsync(string slug, CancellationToken cancellationToken = default);

        /// <summary>
        /// 2e. Edit Author if existed, otherwise insert a new one
        /// </summary>
        /// <param name="newAuthor"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Author> EditAuthorAsync(Author newAuthor, CancellationToken cancellationToken = default);

        /// <summary>
        /// 2d. Paginate Authors
        /// </summary>
        /// <param name="pagingParams"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IPagedList<AuthorItem>> GetPagedAuthorsAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default);

        /// <summary>
        /// 2f. Get Authors has most Articles
        /// </summary>
        /// <param name="numberOfAuthors"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<Author>> GetAuthorsHasMostArticles(int numberOfAuthors, CancellationToken cancellationToken = default);
        #endregion
    }
}
