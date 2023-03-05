using Microsoft.EntityFrameworkCore;
using TipsAndTricks.Core.Contracts;
using TipsAndTricks.Core.DTO;
using TipsAndTricks.Core.Entities;
using TipsAndTricks.Data.Contexts;
using TipsAndTricks.Services.Extensions;

namespace TipsAndTricks.Services.Blogs {
    public class BlogRepository : IBlogRepository {
        private readonly BlogDbContext _context;

        public BlogRepository(BlogDbContext context) {
            this._context = context;
        }

        #region Author methods
        #endregion

        #region Category methods
        /// <summary>
        /// Get Categories
        /// </summary>
        /// <param name="showOnMenu"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IList<CategoryItem>> GetCategoriesAsync(bool showOnMenu = false, CancellationToken cancellationToken = default) {
            IQueryable<Category> categories = _context.Set<Category>();

            if (showOnMenu)
                categories = categories.Where(x => x.ShowOnMenu);

            return await categories
                .OrderBy(x => x.Name)
                .Select(x => new CategoryItem() {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.UrlSlug,
                    Description = x.Description,
                    ShowOnMenu = x.ShowOnMenu,
                    PostCount = x.Posts.Count(p => p.Published)
                })
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// 1f. Get Category by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Category> GetCategoryByIdAsync(int id, CancellationToken cancellationToken = default) {
            IQueryable<Category> categories = _context.Set<Category>();

            return await categories
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        /// <summary>
        /// 1e. Get Category by Slug
        /// </summary>
        /// <param name="slug"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Category> GetCategoryBySlugAsync(string slug, CancellationToken cancellationToken = default) {
            IQueryable<Category> categories = _context.Set<Category>();

            return await categories
                .FirstOrDefaultAsync(x => x.UrlSlug.ToLower().Contains(slug.ToLower()), cancellationToken);
        }

        /// <summary>
        /// 1h. Delete Category by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> DeleteCategoryByIdAsync(int id, CancellationToken cancellationToken = default) {
            return await _context.Set<Category>()
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync(cancellationToken) > 0;
        }

        /// <summary>
        /// 1g. Edit Category
        /// </summary>
        /// <param name="newCategory"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Category> EditCategoryAsync(Category newCategory, CancellationToken cancellationToken = default) {
            var category = await _context.Set<Category>().AnyAsync(x => x.Id == newCategory.Id, cancellationToken);
            if (category)
                _context.Entry(newCategory).State = EntityState.Modified;
            else
                _context.Categories.Add(newCategory);
            await _context.SaveChangesAsync(cancellationToken);
            return newCategory;
        }

        /// <summary>
        /// 1i. Check whether Category's Slug is existed
        /// </summary>
        /// <param name="slug"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> IsCategorySlugExistedAsync(string slug, CancellationToken cancellationToken = default) {
            return await _context.Set<Category>()
                .AnyAsync(x => x.UrlSlug.CompareTo(slug) == 0, cancellationToken);
        }

        /// <summary>
        /// 1j. Paginate Categories
        /// </summary>
        /// <param name="pagingParams"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default) {
            var categoryQuery = _context.Set<Category>()
                .Select(x => new CategoryItem() {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.UrlSlug,
                    Description = x.Description,
                    PostCount = x.Posts.Count(p => p.Published)
                });

            return await categoryQuery.ToPagedListAsync(pagingParams, cancellationToken);
        }
        #endregion

        #region Tag methods
        /// <summary>
        /// 1c. Get Tags
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IList<TagItem>> GetTagsAsync(CancellationToken cancellationToken = default) {
            return await _context.Set<Tag>()
                .Select(x => new TagItem() {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    UrlSlug = x.UrlSlug,
                    PostCount = x.Posts.Count(p => p.Published)
                })
                .ToListAsync();
        }

        /// <summary>
        /// 1d. Delete Tag by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> DeleteTagByNameAsync(int id, CancellationToken cancellationToken = default) {
            return await _context.Set<Tag>()
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync(cancellationToken) > 0;
        }

        /// <summary>
        /// 1a. Get Tag by Slug
        /// </summary>
        /// <param name="slug"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Tag> GetTagBySlugAsync(string slug, CancellationToken cancellationToken = default) {
            IQueryable<Tag> tags = _context.Set<Tag>();

            return await tags
                .FirstOrDefaultAsync(x => x.UrlSlug.ToLower().Contains(slug.ToLower()), cancellationToken);
        }

        /// <summary>
        /// Paginate Tags
        /// </summary>
        /// <param name="pagingParams"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IPagedList<TagItem>> GetPagedTagsAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default) {
            var tagQuery = _context.Set<Tag>()
                .Select(x => new TagItem() {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.UrlSlug,
                    Description = x.Description,
                    PostCount = x.Posts.Count(p => p.Published)
                });

            return await tagQuery.ToPagedListAsync(pagingParams, cancellationToken);
        }
        #endregion

        #region Post methods
        /// <summary>
        /// Get Posts
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="slug"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Post> GetPostAsync(int year, int month, string slug, CancellationToken cancellationToken = default) {
            IQueryable<Post> postsQuery = _context.Set<Post>()
                .Include(x => x.Category)
                .Include(x => x.Author);

            if (year > 0)
                postsQuery = postsQuery.Where(x => x.PostedDate.Year == year);
            if (month > 0)
                postsQuery = postsQuery.Where(x => x.PostedDate.Month == month);
            if (!string.IsNullOrWhiteSpace(slug))
                postsQuery = postsQuery.Where(x => x.UrlSlug == slug);

            return await postsQuery.FirstOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// Get popular Posts
        /// </summary>
        /// <param name="numPosts"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IList<Post>> GetPopularArticlesAsync(int numPosts, CancellationToken cancellationToken = default) {
            return await _context.Set<Post>()
                .Include(x => x.Author)
                .Include(x => x.Category)
                .OrderByDescending(p => p.ViewCount)
                .Take(numPosts)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Increase Post's view by 1
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task IncreaseViewCountAsync(int postId, CancellationToken cancellationToken = default) {
            await _context.Set<Post>()
                .Where(x => x.Id == postId)
                .ExecuteUpdateAsync(p => p.SetProperty(x => x.ViewCount, x => x.ViewCount + 1), cancellationToken);
        }

        /// <summary>
        ///  Check whether Post's Slug is existed
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="slug"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> IsPostSlugExistedAsync(int postId, string slug, CancellationToken cancellationToken = default) {
            return await _context.Set<Post>()
                .AnyAsync(x => x.Id != postId && x.UrlSlug == slug, cancellationToken);
        }
        #endregion
    }
}
