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
        /// <param name="reason">Reason why Subscriber is banned</param>
        /// <param name="notes">Admin's notes</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> BanSubscriberAsync(int id, string reason, string notes, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete Subscriber
        /// </summary>
        /// <param name="id">Subscriber's Id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> DeleteSubscriberAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Search Subscriber
        /// </summary>
        /// <param name="pagingParams"></param>
        /// <param name="keywords"></param>
        /// <param name="subscribeStatus"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IPagedList<Subscriber>> SearchSubscribersAsync(IPagingParams pagingParams, string keywords, SubscribeState subscribeStatus, CancellationToken cancellationToken = default);
    }
}
