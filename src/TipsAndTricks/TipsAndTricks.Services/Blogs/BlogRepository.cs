using Microsoft.EntityFrameworkCore;
using SlugGenerator;
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
        /// <param name="id">Category's Id</param>
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
        /// <param name="slug">Category's Slug</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Category> GetCategoryBySlugAsync(string slug, CancellationToken cancellationToken = default) {
            return await _context.Set<Category>()
                .Include(p => p.Posts)
                .FirstOrDefaultAsync(x => x.UrlSlug == slug, cancellationToken);
        }

        /// <summary>
        /// 1h. Delete Category by Id
        /// </summary>
        /// <param name="id">Category's Id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> DeleteCategoryByIdAsync(int id, CancellationToken cancellationToken = default) {
            return await _context.Set<Category>()
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync(cancellationToken) > 0;
        }

        /// <summary>
        /// 1g. Edit Category if existed, otherwise insert a new one
        /// If Category's Id is not provided, insert a new Category with continuous Id
        /// If Category's Id is provided and existed in database, update Category with new values
        /// </summary>
        /// <param name="newCategory">New Category</param>
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
        /// Set Post's Image URL
        /// </summary>
        /// <param name="id">Post's Id</param>
        /// <param name="imageUrl"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> SetImageUrlAsync(int id, string imageUrl,
           CancellationToken cancellationToken = default) {
            return await _context.Set<Post>()
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(x => x.SetProperty(a => a.ImageUrl, a => imageUrl), cancellationToken) > 0;
        }

        /// <summary>
        /// 1i. Check whether Category's Slug is existed
        /// </summary>
        /// <param name="id">Category's Id</param>
        /// <param name="slug">Category's Slug</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> IsCategorySlugExistedAsync(int id, string slug, CancellationToken cancellationToken = default) {
            return await _context.Set<Category>()
                .AnyAsync(x => x.Id != id && x.UrlSlug == slug, cancellationToken);
        }

        /// <summary>
        /// 1j. Paginate Categories
        /// </summary>
        /// <param name="pagingParams"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default) {
            return await _context.Set<Category>()
                .Select(x => new CategoryItem() {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.UrlSlug,
                    Description = x.Description,
                    ShowOnMenu = x.ShowOnMenu,
                    PostCount = x.Posts.Count(p => p.Published)
                })
                .ToPagedListAsync(pagingParams, cancellationToken);
        }

        /// <summary>
        /// Filter Categories by queries
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IQueryable<CategoryItem> FilterCategories(ICategoryQuery query) {
            IQueryable<CategoryItem> categoryQuery = _context.Set<Category>()
                .Select(x => new CategoryItem() {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.UrlSlug,
                    Description = x.Description,
                    ShowOnMenu = x.ShowOnMenu,
                    PostCount = x.Posts.Count(p => p.Published)
                });

            if (!string.IsNullOrWhiteSpace(query.Keyword)) {
                categoryQuery = categoryQuery.Where(x => x.Name.Contains(query.Keyword) ||
                                                            x.Description.Contains(query.Keyword));
            }
            if (query.ShowOnMenu) {
                categoryQuery = categoryQuery.Where(x => x.ShowOnMenu);
            }

            return categoryQuery;
        }

        /// <summary>
        /// Paginate Categories found by queries
        /// </summary>
        /// <param name="query"></param>
        /// <param name="pagingParams"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IPagedList<CategoryItem>> GetPagedCategoriesByQueryAsync(ICategoryQuery query, IPagingParams pagingParams, CancellationToken cancellationToken = default) {
            return await FilterCategories(query).ToPagedListAsync(pagingParams);
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
        /// <param name="id">Tag's Id</param>
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
        /// <param name="slug">Tag's Slug</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Tag> GetTagBySlugAsync(string slug, CancellationToken cancellationToken = default) {
            return await _context.Set<Tag>()
                .Include(p => p.Posts)
                .FirstOrDefaultAsync(x => x.UrlSlug == slug, cancellationToken);
        }

        /// <summary>
        /// 1d. Delete Tag by Id
        /// </summary>
        /// <param name="id">Tag's Id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> DeleteTagByIdAsync(int id, CancellationToken cancellationToken = default) {
            return await _context.Set<Tag>()
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync(cancellationToken) > 0;
        }

        /// <summary>
        /// Edit Tag if existed, otherwise insert a new one
        /// If Tag's Id is not provided, insert a new Tag with continuous Id
        /// If Tag's Id is provided and existed in database, update Tag with new values
        /// </summary>
        /// <param name="newTag"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Tag> EditTagAsync(Tag newTag, CancellationToken cancellationToken = default) {
            var tag = await _context.Set<Tag>()
                .Include(p => p.Posts)
                .AnyAsync(x => x.Id == newTag.Id, cancellationToken);
            if (tag)
                _context.Entry(newTag).State = EntityState.Modified;
            else
                await _context.Tags.AddAsync(newTag, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return newTag;
        }

        /// <summary>
        /// Check whether Tag's Slug is existed
        /// </summary>
        /// <param name="id">Tag's Id</param>
        /// <param name="slug">Tag's Slug</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> IsTagSlugExistedAsync(int id, string slug, CancellationToken cancellationToken = default) {
            return await _context.Set<Tag>()
                .AnyAsync(x => x.Id != id && x.UrlSlug == slug, cancellationToken);
        }

        /// <summary>
        /// Paginate Tags
        /// </summary>
        /// <param name="pagingParams"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IPagedList<TagItem>> GetPagedTagsAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default) {
            return await _context.Set<Tag>()
                .Select(x => new TagItem() {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.UrlSlug,
                    Description = x.Description,
                    PostCount = x.Posts.Count(p => p.Published)
                })
                .ToPagedListAsync(pagingParams, cancellationToken);
        }

        /// <summary>
        /// Filter Tags by queries
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IQueryable<TagItem> FilterTags(ITagQuery query) {
            IQueryable<TagItem> tagQuery = _context.Set<Tag>()
                .Select(x => new TagItem() {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.UrlSlug,
                    Description = x.Description,
                    PostCount = x.Posts.Count(p => p.Published)
                });

            if (!string.IsNullOrWhiteSpace(query.Keyword)) {
                tagQuery = tagQuery.Where(x => x.Name.Contains(query.Keyword) || x.Description.Contains(query.Keyword));
            }

            return tagQuery;
        }

        /// <summary>
        /// Paginate Tags found by queries
        /// </summary>
        /// <param name="query"></param>
        /// <param name="pagingParams"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IPagedList<TagItem>> GetPagedTagsByQueryAsync(ITagQuery query, IPagingParams pagingParams, CancellationToken cancellationToken = default) {
            return await FilterTags(query).ToPagedListAsync(pagingParams);
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
                .Where(x => x.Published)
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
                .Where(x => x.Published)
                .OrderBy(x => Guid.NewGuid())
                .Take(numberOfPosts)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// 1k. Get Post by Id
        /// </summary>
        /// <param name="id">Post's Id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Post> GetPostByIdAsync(int id, bool includeDetails = false, CancellationToken cancellationToken = default) {
            if (!includeDetails)
                return await _context.Set<Post>().FindAsync(id, cancellationToken);
            return await _context.Set<Post>()
                .Include(a => a.Author)
                .Include(c => c.Category)
                .Include(t => t.Tags)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        /// <summary>
        /// Get Post by Id
        /// </summary>
        /// <param name="slug">Post's Slug</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Post> GetPostBySlugAsync(string slug, CancellationToken cancellationToken = default) {
            return await _context.Set<Post>()
                .Include(a => a.Author)
                .Include(c => c.Category)
                .Include(t => t.Tags)
                .FirstOrDefaultAsync(x => x.UrlSlug == slug, cancellationToken);
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
        /// Delete Post by Id
        /// </summary>
        /// <param name="id">Post's Id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> DeletePostById(int id, CancellationToken cancellationToken = default) {
            return await _context.Set<Post>()
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync(cancellationToken) > 0;
        }

        /// <summary>
        /// 1m. Edit Post if existed, otherwise insert a new one
        /// If Post's Id is not provided, insert a new Post with continuous Id
        /// If Post's Id is provided and existed in database, update Post with new values
        /// </summary>
        /// <param name="newPost">New Post</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Post> EditPostAsync(Post post, IEnumerable<string> tags, CancellationToken cancellationToken = default) {
            var existedPost = await _context.Set<Post>().AnyAsync(s => s.Id == post.Id, cancellationToken);


            if (!existedPost || post.Tags == null) {
                post.Tags = new List<Tag>();
            } else if (post.Tags == null || post.Tags.Count == 0) {
                await _context.Entry(post)
                    .Collection(x => x.Tags)
                    .LoadAsync(cancellationToken);
            }

            var validTags = tags.Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => new {
                    Name = x,
                    Slug = x.GenerateSlug()
                })
                .GroupBy(x => x.Slug)
                .ToDictionary(g => g.Key, g => g.First().Name);

            if (existedPost) {
                var oldPost = await GetPostByIdAsync(post.Id, true, cancellationToken);
                var oldTags = oldPost.Tags.ToList();
                foreach (var tag in oldTags) {
                    if (!validTags.ContainsKey(tag.UrlSlug)) {
                        tag.Posts.Remove(post);
                        post.Tags.Remove(tag);
                    }
                }
            }

            foreach (var item in validTags) {
                var tagExists = post.Tags.Any(x => string.Compare(x.UrlSlug, item.Key, StringComparison.InvariantCultureIgnoreCase) == 0);
                if (tagExists)
                    continue;

                var tag = await GetTagBySlugAsync(item.Key, cancellationToken) ?? new Tag() {
                    Name = item.Value,
                    Description = item.Value,
                    UrlSlug = item.Key,
                    Posts = new List<Post>()
                };

                if (tag.Posts.All(p => p.Id != post.Id)) {
                    tag.Posts.Add(post);
                }

                post.Tags.Add(tag);
            }

            if (existedPost) {
                _context.Posts.Update(post);
            } else {
                _context.Posts.Add(post);
            }

            var enries = _context.ChangeTracker.Entries();
            await _context.SaveChangesAsync(cancellationToken);

            return post;
        }

        /// <summary>
        /// 1n. Change Post's Published status
        /// </summary>
        /// <param name="id">Post's Id</param>
        /// <returns></returns>
        public async Task ChangePostPublishedStatusAsync(int id, CancellationToken cancellationToken = default) {
            await _context.Set<Post>()
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(p => p.SetProperty(x => x.Published, x => !x.Published), cancellationToken);
        }

        /// <summary>
        /// Increase Post's View by 1
        /// </summary>
        /// <param name="id">Post's Id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task IncreaseViewCountAsync(int id, CancellationToken cancellationToken = default) {
            await _context.Set<Post>()
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(p => p.SetProperty(x => x.ViewCount, x => x.ViewCount + 1), cancellationToken);
        }

        /// <summary>
        /// Check whether Post's Slug is existed
        /// </summary>
        /// <param name="id">Post's Id</param>
        /// <param name="slug">Post's Slug</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> IsPostSlugExistedAsync(int id, string slug, CancellationToken cancellationToken = default) {
            return await _context.Set<Post>()
                .AnyAsync(x => x.Id != id && x.UrlSlug == slug, cancellationToken);
        }

        /// <summary>
        /// Count Monthly Posts
        /// </summary>
        /// <param name="numMonths">Number of Posts</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IList<MonthlyPostCountItem>> CountMonthlyPostsAsync(int numMonths, CancellationToken cancellationToken = default) {
            return await _context.Set<Post>()
                .Where(x => x.Published)
                .GroupBy(x => new { x.PostedDate.Year, x.PostedDate.Month })
                .Select(g => new MonthlyPostCountItem() {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    PostCount = g.Count(x => x.Published)
                })
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Month)
                .Take(numMonths)
                .ToListAsync(cancellationToken);
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

            if (query.Published)
                postQuery = postQuery.Where(x => x.Published);
            if (query.Unpublished)
                postQuery = postQuery.Where(x => !x.Published);
            if (!string.IsNullOrWhiteSpace(query.AuthorSlug))
                postQuery = postQuery.Where(x => x.Author.UrlSlug == query.AuthorSlug);
            if (!string.IsNullOrWhiteSpace(query.CategorySlug))
                postQuery = postQuery.Where(x => x.Category.UrlSlug == query.CategorySlug);
            if (!string.IsNullOrWhiteSpace(query.TagSlug))
                postQuery = postQuery.Where(x => x.Tags.Any(t => t.UrlSlug == query.TagSlug));
            if (query.AuthorId != null)
                postQuery = postQuery.Where(x => x.AuthorId == query.AuthorId);
            if (query.CategoryId != null)
                postQuery = postQuery.Where(x => x.CategoryId == query.CategoryId);
            if (query.PostedYear != null)
                postQuery = postQuery.Where(x => x.PostedDate.Year == query.PostedYear);
            if (query.PostedMonth != null)
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

        /// <summary>
        /// Paginate Posts found by queries and cast to T type
        /// </summary>
        /// <typeparam name="T">Destination Type</typeparam>
        /// <param name="mapper">Function to cast type</param>
        /// <param name="query"></param>
        /// <param name="pagingParams"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IPagedList<T>> GetPagedPostsByQueryAsync<T>(Func<IQueryable<Post>, IQueryable<T>> mapper, IPostQuery query, IPagingParams pagingParams, CancellationToken cancellationToken = default) {
            return await mapper(FilterPosts(query).AsNoTracking()).ToPagedListAsync(pagingParams, cancellationToken);
        }
        #endregion
    }
}
