using TipsAndTricks.Core.Contracts;
using TipsAndTricks.Core.DTO;
using TipsAndTricks.Core.Entities;

namespace TipsAndTricks.Services.Blogs {
    public interface IBlogRepository {
        Task<Post> GetPostAsync(int year, int month, string slug, CancellationToken cancellationToken = default);
        Task<IList<Post>> GetPopularArticlesAsync(int numPosts, CancellationToken cancellationToken = default);
        Task<bool> IsPostSlugExistedAsync(int postId, string slug, CancellationToken cancellationToken = default);
        Task IncreaseViewCountAsync(int postId, CancellationToken cancellationToken = default);
        Task<IList<CategoryItem>> GetCategoriesAsync(bool showOnMenu = false, CancellationToken cancellationToken = default);
        Task<IPagedList<TagItem>> GetPagedTagsAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default);

        /// <summary>
        /// 1a. Get Tag by Slug
        /// </summary>
        /// <param name="slug"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Tag> GetTagBySlugAsync(string slug, CancellationToken cancellationToken = default);

        /// <summary>
        /// 1c. Get Tags
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<TagItem>> GetTagsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 1d. Delete Tag by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> DeleteTagByNameAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// 1e. Get Category by Slug
        /// </summary>
        /// <param name="slug"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Category> GetCategoryBySlugAsync(string slug, CancellationToken cancellationToken = default);

        /// <summary>
        /// 1f. Get Category by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Category> GetCategoryByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// 1g. Edit Category
        /// </summary>
        /// <param name="newCategory"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Category> EditCategoryAsync(Category newCategory, CancellationToken cancellationToken = default);

        /// <summary>
        /// 1h. Delete Category by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> DeleteCategoryByIdAsync(int id, CancellationToken cancellationToken = default);

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

        /// <summary>
        /// 1k. Get Posts by number of months
        /// </summary>
        /// <param name="numberOfMonths"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //Task<IList<PostItem>> GetPostsByNumberOfMonths(int numberOfMonths, CancellationToken cancellationToken = default);
    }
}
