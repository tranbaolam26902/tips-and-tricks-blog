using TipsAndTricks.WebApp.Extensions;
using TipsAndTricks.WebApp.Mapster;

var builder = WebApplication.CreateBuilder(args); {
    builder.ConfigureMvc()
            .ConfigureServices()
            .ConfigureMapster();
}

var app = builder.Build(); {
    app.UseRequestPipeLine();
    app.UseBlogRoutes();
    app.UseDataSeeder();
}

app.Run();
