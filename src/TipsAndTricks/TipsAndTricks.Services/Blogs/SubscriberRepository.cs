using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using TipsAndTricks.Core.Contracts;
using TipsAndTricks.Core.Entities;
using TipsAndTricks.Data.Contexts;
using TipsAndTricks.Services.Extensions;

namespace TipsAndTricks.Services.Blogs {
    public class SubscriberRepository : ISubscriberRepository {
        private readonly BlogDbContext _context;

        public SubscriberRepository(BlogDbContext context) {
            _context = context;
        }

        /// <summary>
        /// Get Subscribers
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IList<Subscriber>> GetSubscribersAsync(CancellationToken cancellationToken = default) {
            return await _context.Set<Subscriber>()
                .Where(x => x.SubscribeState == SubscribeState.Subscribe)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Get Subscriber by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Subscriber> GetSubscriberByIdAsync(int id, CancellationToken cancellationToken = default) {
            return await _context.Set<Subscriber>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        /// <summary>
        /// Get Subscriber by Email
        /// </summary>
        /// <param name="email"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Subscriber> GetSubscriberByEmailAsync(string email, CancellationToken cancellationToken = default) {
            return await _context.Set<Subscriber>().FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
        }

        /// <summary>
        /// Subscribe blog
        /// </summary>
        /// <param name="email"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> SubscribeAsync(string email, CancellationToken cancellationToken = default) {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if (!regex.Match(email).Success)
                return false;

            Subscriber subscriber = await _context.Set<Subscriber>().FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

            if (subscriber == null) {
                subscriber = new Subscriber();
                subscriber.Email = email;
                subscriber.SubscribedDate = DateTime.Now;
                subscriber.SubscribeState = SubscribeState.Subscribe;
                _context.Add(subscriber);
            } else if (subscriber.SubscribeState == SubscribeState.Banned || subscriber.SubscribeState == SubscribeState.Subscribe)
                return false;
            else {
                subscriber.SubscribeState = SubscribeState.Subscribe;
                subscriber.SubscribedDate = DateTime.Now;
                _context.Entry(subscriber).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        /// <summary>
        /// Unsubscribe blog
        /// </summary>
        /// <param name="email"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> UnsubscribeAsync(string email, string reason, CancellationToken cancellationToken = default) {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if (!regex.Match(email).Success)
                return false;

            var subscriber = await _context.Set<Subscriber>().FirstOrDefaultAsync(x => x.SubscribeState == SubscribeState.Subscribe && x.Email == email, cancellationToken);

            if (subscriber != null) {
                subscriber.Reason = reason;
                subscriber.SubscribeState = SubscribeState.Unsubscribe;
                subscriber.UnsubscribedDate = DateTime.Now;
                _context.Entry(subscriber).State = EntityState.Modified;
                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Ban a Subscriber
        /// </summary>
        /// <param name="id"></param>
        /// <param name="reason"></param>
        /// <param name="notes"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> BanSubscriberAsync(int id, string reason, string notes, CancellationToken cancellationToken = default) {
            var subscriber = await _context.Set<Subscriber>().FirstOrDefaultAsync(x => x.Id == id && x.SubscribeState != SubscribeState.Banned, cancellationToken);
            if (subscriber == null)
                return false;

            subscriber.SubscribeState = SubscribeState.Banned;
            subscriber.UnsubscribedDate = DateTime.Now;
            subscriber.Reason = reason;
            subscriber.Notes = notes;
            _context.Entry(subscriber).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        /// <summary>
        /// Delete Subscriber
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> DeleteSubscriberAsync(int id, CancellationToken cancellationToken = default) {
            return await _context.Set<Subscriber>()
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync(cancellationToken) > 0;
        }

        /// <summary>
        /// Search Subscriber
        /// </summary>
        /// <param name="pagingParams"></param>
        /// <param name="keyword"></param>
        /// <param name="subscribeState"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IPagedList<Subscriber>> SearchSubscribersAsync(IPagingParams pagingParams, string keyword, SubscribeState subscribeState, CancellationToken cancellationToken = default) {
            var subscriberRequest = _context.Set<Subscriber>()
                .Where(x => x.Email.Contains(keyword) ||
                            x.Reason.ToLower().Contains(keyword.ToLower()) ||
                            x.Notes.ToLower().Contains(keyword.ToLower()) ||
                            x.SubscribeState == subscribeState);
            return await subscriberRequest.ToPagedListAsync(pagingParams, cancellationToken);
        }
    }
}
