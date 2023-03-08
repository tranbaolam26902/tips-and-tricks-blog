using TipsAndTricks.Core.Contracts;
using TipsAndTricks.Core.DTO;
using TipsAndTricks.Core.Entities;

namespace TipsAndTricks.Services.Blogs {
    public interface IBlogRepository {
        #region Category methods
        /// <summary>
        /// Get Categories
        /// </summary>
        /// <param name="showOnMenu"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<CategoryItem>> GetCategoriesAsync(bool showOnMenu = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// 1f. Get Category by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Category> GetCategoryByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// 1e. Get Category by Slug
        /// </summary>
        /// <param name="slug"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Category> GetCategoryBySlugAsync(string slug, CancellationToken cancellationToken = default);

        /// <summary>
        /// 1h. Delete Category by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> DeleteCategoryByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// 1g. Edit Category if existed, otherwise insert a new one
        /// </summary>
        /// <param name="newCategory"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Category> EditCategoryAsync(Category newCategory, CancellationToken cancellationToken = default);

        /// <summary>
        /// 1i. Check whether Category's Slug is existed
        /// </summary>
        /// <param name="slug"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> IsCategorySlugExistedAsync(string slug, CancellationToken cancellationToken = default);

        /// <summary>
        /// 1j. Paginate Categories
        /// </summary>
        /// <param name="pagingParams"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default);
        #endregion

        #region Tags methods
        /// <summary>
        /// 1c. Get Tags
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<TagItem>> GetTagsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get Tag by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Tag> GetTagByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// 1a. Get Tag by Slug
        /// </summary>
        /// <param name="slug"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Tag> GetTagBySlugAsync(string slug, CancellationToken cancellationToken = default);

        /// <summary>
        /// 1d. Delete Tag by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> DeleteTagByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Paginate Tags
        /// </summary>
        /// <param name="pagingParams"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IPagedList<TagItem>> GetPagedTagsAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default);
        #endregion

        #region Post methods
        /// <summary>
        /// Get Posts
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<Post>> GetPostsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get popular Posts
        /// </summary>
        /// <param name="numPosts"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<Post>> GetPopularArticlesAsync(int numPosts, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get random Posts
        /// </summary>
        /// <param name="numberOfPosts"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<Post>> GetRandomPostsAsync(int numberOfPosts, CancellationToken cancellationToken = default);

        /// <summary>
        /// 1k. Get Post by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Post> GetPostByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get Post by Year, Month Published and Slug
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="slug"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Post> GetPostAsync(int year, int month, string slug, CancellationToken cancellationToken = default);

        /// <summary>
        /// 1m. Edit Post if existed, otherwise insert a new one
        /// </summary>
        /// <param name="newPost"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Post> EditPostAsync(Post newPost, CancellationToken cancellationToken = default);

        /// <summary>
        /// 1n. Change Post's Published status
        /// </summary>
        /// <param name="id">Post Id</param>
        /// <param name="state">Published</param>
        /// <returns></returns>
        Task ChangePostPublishedStatusAsync(int id, bool status);

        /// <summary>
        /// Increase Post's view by 1
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task IncreaseViewCountAsync(int postId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Filter Posts by Queries
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        IQueryable<Post> FilterPosts(IPostQuery query);

        /// <summary>
        /// Check whether Post's Slug is existed
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="slug"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> IsPostSlugExistedAsync(int postId, string slug, CancellationToken cancellationToken = default);

        /// <summary>
        /// 1q. Find all Posts by queries
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<Post>> GetPostsByQuery(IPostQuery query, CancellationToken cancellationToken = default);

        /// <summary>
        /// 1r. Count number of Posts by queries
        /// </summary>
        /// <param name="query"></param>
        /// <param name="count"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> CountPostsByQueryAsync(IPostQuery query, CancellationToken cancellationToken = default);

        /// <summary>
        /// 1s. Paginate Posts found by queries
        /// </summary>
        /// <param name="pagingParams"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IPagedList<Post>> GetPagedPostsByQueryAsync(IPostQuery query, IPagingParams pagingParams, CancellationToken cancellationToken = default);
        #endregion
    }
}
