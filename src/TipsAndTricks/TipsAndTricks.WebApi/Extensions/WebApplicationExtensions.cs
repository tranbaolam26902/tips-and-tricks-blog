﻿using Microsoft.EntityFrameworkCore;
using NLog.Web;
using TipsAndTricks.Data.Contexts;
using TipsAndTricks.Services.Blogs;
using TipsAndTricks.Services.Media;
using TipsAndTricks.Services.Timing;

namespace TipsAndTricks.WebApi.Extensions {
    public static class WebApplicationExtensions {
        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder) {
            builder.Services.AddMemoryCache();
            builder.Services.AddDbContext<BlogDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<ITimeProvider, LocalTimeProvider>();
            builder.Services.AddScoped<IMediaManager, LocalFileSystemMediaManager>();
            builder.Services.AddScoped<IBlogRepository, BlogRepository>();
            builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
            builder.Services.AddScoped<ICommentRepository, CommentRepository>();
            builder.Services.AddScoped<ISubscriberRepository, SubscriberRepository>();

            return builder;
        }

        public static WebApplicationBuilder ConfigureCors(this WebApplicationBuilder builder) {
            builder.Services.AddCors(options => {
                options.AddPolicy("TipsAndTricksBlogApp", policyBuilder => policyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });

            return builder;
        }

        public static WebApplicationBuilder ConfigureNLog(this WebApplicationBuilder builder) {
            builder.Logging.ClearProviders();
            builder.Host.UseNLog();

            return builder;
        }

        public static WebApplicationBuilder ConfigureSwaggerOpenApi(this WebApplicationBuilder builder) {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            return builder;
        }

        public static WebApplication SetupRequestPipeLine(this WebApplication app) {
            if (app.Environment.IsDevelopment()) {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseCors("TipsAndTricksBlogApp");

            return app;
        }
    }
}
