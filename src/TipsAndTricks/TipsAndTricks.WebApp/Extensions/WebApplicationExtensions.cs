﻿using Microsoft.EntityFrameworkCore;
using NLog.Web;
using TipsAndTricks.Data.Contexts;
using TipsAndTricks.Data.Seeders;
using TipsAndTricks.Services.Blogs;
using TipsAndTricks.Services.Media;
using TipsAndTricks.WebApp.Middlewares;

namespace TipsAndTricks.WebApp.Extensions {
    public static class WebApplicationExtensions {
        public static WebApplicationBuilder ConfigureMvc(this WebApplicationBuilder builder) {
            builder.Services.AddControllersWithViews();
            builder.Services.AddResponseCompression();

            return builder;
        }

        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder) {
            builder.Services.AddDbContext<BlogDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IMediaManager, LocalFileSystemMediaManager>();
            builder.Services.AddScoped<IBlogRepository, BlogRepository>();
            builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
            builder.Services.AddScoped<ISubscriberRepository, SubscriberRepository>();
            builder.Services.AddScoped<ICommentRepository, CommentRepository>();
            builder.Services.AddScoped<IDataSeeder, DataSeeder>();

            return builder;
        }

        public static WebApplication UseRequestPipeLine(this WebApplication app) {
            if (app.Environment.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else {
                app.UseExceptionHandler("/Blog/Error");
                app.UseHsts();
            }
            app.UseResponseCompression();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseMiddleware<UserActivityMiddleware>();

            return app;
        }

        public static IApplicationBuilder UseDataSeeder(this IApplicationBuilder app) {
            using var scope = app.ApplicationServices.CreateScope();

            try {
                scope.ServiceProvider
                    .GetRequiredService<IDataSeeder>()
                    .Initialize();
            }
            catch (Exception ex) {
                scope.ServiceProvider
                    .GetRequiredService<ILogger<Program>>()
                    .LogError(ex, "Could not insert data into database");
            }

            return app;
        }

        public static WebApplicationBuilder ConfigureNLog(this WebApplicationBuilder builder) {
            builder.Logging.ClearProviders();
            builder.Host.UseNLog();

            return builder;
        }
    }
}
