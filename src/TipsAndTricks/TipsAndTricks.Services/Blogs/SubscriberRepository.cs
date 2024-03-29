﻿using Microsoft.EntityFrameworkCore;
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
        /// <param name="id">Subscriber's Id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Subscriber> GetSubscriberByIdAsync(int id, CancellationToken cancellationToken = default) {
            return await _context.Set<Subscriber>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        /// <summary>
        /// Get Subscriber by Email
        /// </summary>
        /// <param name="email">Subscriber's Email</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Subscriber> GetSubscriberByEmailAsync(string email, CancellationToken cancellationToken = default) {
            return await _context.Set<Subscriber>().FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
        }

        /// <summary>
        /// Subscribe blog
        /// </summary>
        /// <param name="email">Subscriber's Email</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> SubscribeAsync(string email, CancellationToken cancellationToken = default) {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if (!regex.Match(email).Success)
                return false;

            Subscriber subscriber = await GetSubscriberByEmailAsync(email, cancellationToken);

            if (subscriber == null) {
                subscriber = new Subscriber();
                subscriber.Email = email;
                subscriber.SubscribedDate = DateTime.Now;
                subscriber.SubscribeState = SubscribeState.Subscribe;
                subscriber.PreviousBannedState = SubscribeState.Subscribe;
                _context.Add(subscriber);
            } else if (subscriber.SubscribeState == SubscribeState.Banned || subscriber.SubscribeState == SubscribeState.Subscribe)
                return false;
            else {
                subscriber.SubscribeState = SubscribeState.Subscribe;
                subscriber.PreviousBannedState = SubscribeState.Subscribe;
                subscriber.SubscribedDate = DateTime.Now;
                _context.Entry(subscriber).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        /// <summary>
        /// Unsubscribe blog
        /// </summary>
        /// <param name="email">Subscriber's Email</param>
        /// <param name="reason">Reason why Subscriber Unsubscribe</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> UnsubscribeAsync(string email, string reason, CancellationToken cancellationToken = default) {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if (!regex.Match(email).Success)
                return false;

            var subscriber = await GetSubscriberByEmailAsync(email, cancellationToken);

            if (subscriber == null)
                return false;

            if (subscriber.SubscribeState == SubscribeState.Subscribe) {
                subscriber.Reason = reason;
                subscriber.SubscribeState = SubscribeState.Unsubscribe;
                subscriber.PreviousBannedState = SubscribeState.Unsubscribe;
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
        /// <param name="id">Subscriber's Id</param>
        /// <param name="notes">Admin's notes</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> BanSubscriberAsync(int id, string notes, CancellationToken cancellationToken = default) {
            var subscriber = await _context.Set<Subscriber>().FirstOrDefaultAsync(x => x.Id == id && x.SubscribeState != SubscribeState.Banned, cancellationToken);
            if (subscriber == null)
                return false;

            subscriber.SubscribeState = SubscribeState.Banned;
            subscriber.Notes = notes;
            _context.Entry(subscriber).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        /// <summary>
        /// Delete Subscriber
        /// </summary>
        /// <param name="id">Subscriber's Id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> DeleteSubscriberByIdAsync(int id, CancellationToken cancellationToken = default) {
            return await _context.Set<Subscriber>()
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync(cancellationToken) > 0;
        }

        /// <summary>
        /// Search Subscriber
        /// </summary>
        /// <param name="pagingParams"></param>
        /// <param name="keywords"></param>
        /// <param name="subscribeStatus"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IPagedList<Subscriber>> SearchSubscribersAsync(IPagingParams pagingParams, string keywords, SubscribeState subscribeState, CancellationToken cancellationToken = default) {
            var subscriberRequest = _context.Set<Subscriber>()
                .Where(x => x.Email.Contains(keywords) ||
                            x.Reason.ToLower().Contains(keywords.ToLower()) ||
                            x.Notes.ToLower().Contains(keywords.ToLower()) ||
                            x.SubscribeState == subscribeState);
            return await subscriberRequest.ToPagedListAsync(pagingParams, cancellationToken);
        }

        /// <summary>
        /// Filter Subscribers by queries
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IQueryable<Subscriber> FilterSubscribers(ISubscriberQuery query) {
            IQueryable<Subscriber> subscriberQuery = _context.Set<Subscriber>();

            if (!string.IsNullOrWhiteSpace(query.Keyword)) {
                subscriberQuery = subscriberQuery.Where(x => x.Email.Contains(query.Keyword) ||
                                                            x.Reason.Contains(query.Keyword) ||
                                                            x.Notes.Contains(query.Keyword));
            }

            if (query.SubscribeState != null)
                switch (query.SubscribeState) {
                    case SubscribeState.Subscribe:
                        subscriberQuery = subscriberQuery.Where(x => x.SubscribeState == SubscribeState.Subscribe);
                        break;
                    case SubscribeState.Unsubscribe:
                        subscriberQuery = subscriberQuery.Where(x => x.SubscribeState == SubscribeState.Unsubscribe);
                        break;
                    case SubscribeState.Banned:
                        subscriberQuery = subscriberQuery.Where(x => x.SubscribeState == SubscribeState.Banned);
                        break;
                }

            if (query.LastSubscribedYear != null) {
                subscriberQuery = subscriberQuery.Where(x => x.SubscribedDate.Year == query.LastSubscribedYear);
            }

            if (query.LastSubscribedMonth != null) {
                subscriberQuery = subscriberQuery.Where(x => x.SubscribedDate.Month == query.LastSubscribedMonth);
            }

            if (query.LastSubscribedDay != null) {
                subscriberQuery = subscriberQuery.Where(x => x.SubscribedDate.Day == query.LastSubscribedDay);
            }

            return subscriberQuery;
        }

        /// <summary>
        /// Paginate Subscribers by queries
        /// </summary>
        /// <param name="query"></param>
        /// <param name="pagingParams"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IPagedList<Subscriber>> GetPagedSubscribersByQueryAsync(ISubscriberQuery query, IPagingParams pagingParams, CancellationToken cancellationToken = default) {
            return await FilterSubscribers(query).ToPagedListAsync(pagingParams);
        }

        /// <summary>
        /// Unban Subscriber by Id
        /// </summary>
        /// <param name="id">Subscriber's Id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> UnbanSubscriberAsync(int id, CancellationToken cancellationToken = default) {
            var subscriber = await _context.Set<Subscriber>().FirstOrDefaultAsync(x => x.Id == id && x.SubscribeState == SubscribeState.Banned, cancellationToken);
            if (subscriber == null)
                return false;

            subscriber.SubscribeState = subscriber.PreviousBannedState;
            subscriber.Notes = "";
            _context.Entry(subscriber).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
