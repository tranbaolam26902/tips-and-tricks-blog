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
            return await _context.Set<Category>()
                .Include(p => p.Posts)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        /// <summary>
        /// 1e. Get Category by Slug
        /// </summary>
        /// <param name="slug"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Category> GetCategoryBySlugAsync(string slug, CancellationToken cancellationToken = default) {
            return await _context.Set<Category>()
                .Include(p => p.Posts)
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
        /// 1g. Edit Category if existed, otherwise insert a new one
        /// </summary>
        /// <param name="newCategory"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Category> EditCategoryAsync(Category newCategory, CancellationToken cancellationToken = default) {
            var category = await _context.Set<Category>()
                .Include(p => p.Posts)
                .AnyAsync(x => x.Id == newCategory.Id, cancellationToken);
            if (category)
                _context.Entry(newCategory).State = EntityState.Modified;
            else
                await _context.Categories.AddAsync(newCategory, cancellationToken);
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
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Get Tag by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Tag> GetTagByIdAsync(int id, CancellationToken cancellationToken = default) {
            return await _context.Set<Tag>()
                .Include(p => p.Posts)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        /// <summary>
        /// 1a. Get Tag by Slug
        /// </summary>
        /// <param name="slug"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Tag> GetTagBySlugAsync(string slug, CancellationToken cancellationToken = default) {
            return await _context.Set<Tag>()
                .Include(p => p.Posts)
                .FirstOrDefaultAsync(x => x.UrlSlug.ToLower().Contains(slug.ToLower()), cancellationToken);
        }

        /// <summary>
        /// 1d. Delete Tag by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> DeleteTagByIdAsync(int id, CancellationToken cancellationToken = default) {
            return await _context.Set<Tag>()
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync(cancellationToken) > 0;
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
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IList<Post>> GetPostsAsync(CancellationToken cancellationToken = default) {
            return await _context.Set<Post>()
                .Include(a => a.Author)
                .Include(c => c.Category)
                .Include(t => t.Tags)
                .Where(x => x.Published)
                .ToListAsync(cancellationToken);
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
                .Include(t => t.Tags)
                .OrderByDescending(p => p.ViewCount)
                .Take(numPosts)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Get random Posts
        /// </summary>
        /// <param name="numberOfPosts"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IList<Post>> GetRandomPostsAsync(int numberOfPosts, CancellationToken cancellationToken = default) {
            return await _context.Set<Post>()
                .Include(a => a.Author)
                .Include(c => c.Category)
                .Include(t => t.Tags)
                .OrderBy(x => Guid.NewGuid())
                .Take(numberOfPosts)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// 1k. Get Post by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Post> GetPostByIdAsync(int id, CancellationToken cancellationToken = default) {
            return await _context.Set<Post>()
                .Include(a => a.Author)
                .Include(c => c.Category)
                .Include(t => t.Tags)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        /// <summary>
        /// Get Post by Year, Month Published and Slug
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="slug"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Post> GetPostAsync(int year, int month, string slug, CancellationToken cancellationToken = default) {
            IQueryable<Post> postsQuery = _context.Set<Post>()
                .Include(x => x.Category)
                .Include(x => x.Author)
                .Include(t => t.Tags);

            if (year > 0)
                postsQuery = postsQuery.Where(x => x.PostedDate.Year == year);
            if (month > 0)
                postsQuery = postsQuery.Where(x => x.PostedDate.Month == month);
            if (!string.IsNullOrWhiteSpace(slug))
                postsQuery = postsQuery.Where(x => x.UrlSlug == slug);

            return await postsQuery.FirstOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// 1m. Edit Post if existed, otherwise insert a new one
        /// </summary>
        /// <param name="newPost"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Post> EditPostAsync(Post newPost, CancellationToken cancellationToken = default) {
            var post = await _context.Set<Post>()
                .Include(a => a.Author)
                .Include(c => c.Category)
                .Include(t => t.Tags)
                .AnyAsync(x => x.Id == newPost.Id, cancellationToken);
            if (post)
                _context.Entry(newPost).State = EntityState.Modified;
            else
                await _context.AddAsync(newPost, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return newPost;
        }

        /// <summary>
        /// 1n. Change Post's Published status
        /// </summary>
        /// <param name="id">Post Id</param>
        /// <param name="status">Published</param>
        /// <returns></returns>
        public async Task ChangePostPublishedStatusAsync(int id, bool status) {
            await _context.Set<Post>()
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(p => p.SetProperty(x => x.Published, status));
        }

        /// <summary>
        /// Increase Post's View by 1
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
        /// Check whether Post's Slug is existed
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="slug"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> IsPostSlugExistedAsync(int postId, string slug, CancellationToken cancellationToken = default) {
            return await _context.Set<Post>()
                .AnyAsync(x => x.Id != postId && x.UrlSlug == slug, cancellationToken);
        }

        /// <summary>
        /// Filter Posts by Queries
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public IQueryable<Post> FilterPosts(IPostQuery query) {
            IQueryable<Post> postQuery = _context.Set<Post>()
                .Include(a => a.Author)
                .Include(c => c.Category)
                .Include(t => t.Tags);
            if (query.PublishedOnly)
                postQuery = postQuery.Where(x => x.Published);
            if (!string.IsNullOrWhiteSpace(query.AuthorSlug))
                postQuery = postQuery.Where(x => x.Author.UrlSlug == query.AuthorSlug);
            if (!string.IsNullOrWhiteSpace(query.CategorySlug))
                postQuery = postQuery.Where(x => x.Category.UrlSlug == query.CategorySlug);
            if (!string.IsNullOrWhiteSpace(query.TagSlug))
                postQuery = postQuery.Where(x => x.Tags.Any(t => t.UrlSlug == query.TagSlug));
            if (query.AuthorId != -1)
                postQuery = postQuery.Where(x => x.AuthorId == query.AuthorId);
            if (query.CategoryId != -1)
                postQuery = postQuery.Where(x => x.CategoryId == query.CategoryId);
            if (query.PostedYear != -1)
                postQuery = postQuery.Where(x => x.PostedDate.Year == query.PostedYear);
            if (query.PostedMonth != -1)
                postQuery = postQuery.Where(x => x.PostedDate.Month == query.PostedMonth);
            if (!string.IsNullOrWhiteSpace(query.Keyword))
                postQuery = postQuery.Where(x => x.Title.Contains(query.Keyword) ||
                                                 x.ShortDescription.Contains(query.Keyword) ||
                                                 x.Description.Contains(query.Keyword) ||
                                                 x.Category.Name.Contains(query.Keyword) ||
                                                 x.Tags.Any(t => t.Name.Contains(query.Keyword)));

            return postQuery;
        }

        /// <summary>
        /// 1q. Find all Posts by queries
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IList<Post>> GetPostsByQuery(IPostQuery query, CancellationToken cancellationToken = default) {
            return await FilterPosts(query).ToListAsync(cancellationToken);
        }

        /// <summary>
        /// 1r. Count number of Posts by queries
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> CountPostsByQueryAsync(IPostQuery query, CancellationToken cancellationToken = default) {
            var posts = await GetPostsByQuery(query);
            return posts.Count();
        }

        /// <summary>
        /// 1s. Paginate Posts found by queries
        /// </summary>
        /// <param name="query"></param>
        /// <param name="pagingParams"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IPagedList<Post>> GetPagedPostsByQueryAsync(IPostQuery query, IPagingParams pagingParams, CancellationToken cancellationToken = default) {
            return await FilterPosts(query).ToPagedListAsync(pagingParams);
        }
        #endregion
    }
}
