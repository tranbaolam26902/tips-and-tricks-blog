﻿namespace TipsAndTricks.WebApp.Extensions {
    public static class RouteExtensions {
        public static IEndpointRouteBuilder UseBlogRoutes(this IEndpointRouteBuilder endpoints) {
            endpoints.MapControllerRoute(
                name: "posts-by-category",
                pattern: "Blog/Category/{slug}",
                defaults: new { controller = "Blog", action = "Category" });

            endpoints.MapControllerRoute(
                name: "posts-by-author",
                pattern: "Blog/Author/{slug}",
                defaults: new { controller = "Blog", action = "Author" });

            endpoints.MapControllerRoute(
                name: "posts-by-tag",
                pattern: "Blog/Tag/{slug}",
                defaults: new { controller = "Blog", action = "Tag" });

            endpoints.MapControllerRoute(
                name: "single-post",
                pattern: "Blog/Post/{year:int}/{month:int}/{day:int}/{slug}",
                defaults: new { controller = "Blog", action = "Post" });

            endpoints.MapControllerRoute(
                name: "posts-by-month",
                pattern: "Blog/Archive/{year:int}/{month:int}",
                defaults: new { controller = "Blog", action = "Archive" });

            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Blog}/{action=Index}");

            return endpoints;
        }

        public static IEndpointRouteBuilder UseNewsletterRoutes(this IEndpointRouteBuilder endpoints) {
            endpoints.MapControllerRoute(
                name: "subscribe",
                pattern: "Newsletter/Subscribe",
                defaults: new { controller = "Newsletter", action = "Subscribe" });

            endpoints.MapControllerRoute(
                name: "subscribe",
                pattern: "Newsletter/SubscribeResult",
                defaults: new { controller = "Newsletter", action = "SubscribeResult" });

            endpoints.MapControllerRoute(
                name: "unsubscribe",
                pattern: "Newsletter/Unsubscribe",
                defaults: new { controller = "Newsletter", action = "Unsubscribe" });

            endpoints.MapControllerRoute(
                name: "unsubscribe",
                pattern: "Newsletter/UnsubscribeResult",
                defaults: new { controller = "Newsletter", action = "UnsubscribeResult" });

            return endpoints;
        }
    }
}
