using TipsAndTricks.Core.Contracts;
using TipsAndTricks.Core.Entities;

namespace TipsAndTricks.Services.Blogs {
    public interface ISubscriberRepository {
        /// <summary>
        /// Get Subscribers
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<Subscriber>> GetSubscribersAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get Subscriber by Id
        /// </summary>
        /// <param name="id">Subscriber's Id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Subscriber> GetSubscriberByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get Subscriber by Email
        /// </summary>
        /// <param name="email">Subscriber's Email</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Subscriber> GetSubscriberByEmailAsync(string email, CancellationToken cancellationToken = default);

        /// <summary>
        /// Subscribe blog
        /// </summary>
        /// <param name="email">Subscriber's Email</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> SubscribeAsync(string email, CancellationToken cancellationToken = default);

        /// <summary>
        /// Unsubscribe blog
        /// </summary>
        /// <param name="email">Subscriber's Email</param>
        /// <param name="reason">Reason why Subscriber Unsubscribe</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> UnsubscribeAsync(string email, string reason, CancellationToken cancellationToken = default);

        /// <summary>
        /// Ban a Subscriber
        /// </summary>
        /// <param name="id">Subscriber's Id</param>
        /// <param name="notes">Admin's notes</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> BanSubscriberAsync(int id, string notes, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete Subscriber
        /// </summary>
        /// <param name="id">Subscriber's Id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> DeleteSubscriberByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Search Subscriber
        /// </summary>
        /// <param name="pagingParams"></param>
        /// <param name="keywords"></param>
        /// <param name="subscribeStatus"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IPagedList<Subscriber>> SearchSubscribersAsync(IPagingParams pagingParams, string keywords, SubscribeState subscribeStatus, CancellationToken cancellationToken = default);

        /// <summary>
        /// Filter Subscribers by queries
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IQueryable<Subscriber> FilterSubscribers(ISubscriberQuery query);

        /// <summary>
        /// Paginate Subscribers found by queries
        /// </summary>
        /// <param name="query"></param>
        /// <param name="pagingParams"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IPagedList<Subscriber>> GetPagedSubscribersByQueryAsync(ISubscriberQuery query, IPagingParams pagingParams, CancellationToken cancellationToken = default);

        /// <summary>
        /// Unban Subscriber by Id
        /// </summary>
        /// <param name="id">Subscriber's Id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> UnbanSubscriberAsync(int id, CancellationToken cancellationToken = default);
    }
}
