using TipsAndTricks.WebApp.Extensions;

var builder = WebApplication.CreateBuilder(args); {
    builder.ConfigureMvc().ConfigureServices();
}

var app = builder.Build(); {
    app.UseRequestPipeLine();
    app.UseBlogRoutes();
    app.UseNewsletterRoutes();
    app.UseDataSeeder();
}

app.Run();
