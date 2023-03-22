using NLog.Web;

namespace TipsAndTricks.WebApi.Extensions {
    public static class WebApplicationExtensions {
        public static WebApplicationBuilder ConfigureNLog(this WebApplicationBuilder builder) {
            builder.Logging.ClearProviders();
            builder.Host.UseNLog();

            return builder;
        }
    }
}
