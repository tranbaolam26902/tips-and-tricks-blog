using TipsAndTricks.WebApp.Extensions;
using TipsAndTricks.WebApp.Mapster;
using TipsAndTricks.WebApp.Validations;

var builder = WebApplication.CreateBuilder(args); {
    builder.ConfigureMvc()
            .ConfigureServices()
            .ConfigureMapster()
            .ConfigureFluentValidation();
}

var app = builder.Build(); {
    app.UseRequestPipeLine();
    app.UseBlogRoutes();
    app.UseDataSeeder();
}

app.Run();
