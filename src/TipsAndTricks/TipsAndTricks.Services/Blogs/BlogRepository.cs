﻿using Microsoft.EntityFrameworkCore;
using TipsAndTricks.Core.Entities;
using TipsAndTricks.Data.Contexts;

namespace TipsAndTricks.Services.Blogs {
    public class BlogRepository : IBlogRepository {
        private readonly BlogDbContext _context;

        public BlogRepository(BlogDbContext context) {
            this._context = context;
        }

        public async Task<IList<Post>> GetPopularArticlesAsync(int numPosts, CancellationToken cancellationToken = default) {
            return await _context.Set<Post>()
                .Include(x => x.Author)
                .Include(x => x.Category)
                .OrderByDescending(p => p.ViewCount)
                .Take(numPosts)
                .ToListAsync(cancellationToken);
        }

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

        public async Task IncreaseViewCountAsync(int postId, CancellationToken cancellationToken = default) {
            await _context.Set<Post>()
                .Where(x => x.Id == postId)
                .ExecuteUpdateAsync(p => p.SetProperty(x => x.ViewCount, x => x.ViewCount + 1), cancellationToken);
        }

        public async Task<bool> IsPostPlugExistedAsync(int postId, string slug, CancellationToken cancellationToken = default) {
            return await _context.Set<Post>()
                .AnyAsync(x => x.Id != postId && x.UrlSlug == slug, cancellationToken);
        }
    }
}
