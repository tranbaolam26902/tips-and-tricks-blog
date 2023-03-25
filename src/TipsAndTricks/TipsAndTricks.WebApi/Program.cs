using TipsAndTricks.WebApi.Endpoints;
using TipsAndTricks.WebApi.Extensions;
using TipsAndTricks.WebApi.Mapsters;
using TipsAndTricks.WebApi.Validations;

var builder = WebApplication.CreateBuilder(args); {
    builder.ConfigureCors()
            .ConfigureNLog()
            .ConfigureServices()
            .ConfigureSwaggerOpenApi()
            .ConfigureMapster()
            .ConfigureFluentValidation();
}

var app = builder.Build(); {
    app.SetupRequestPipeLine();
    app.MapAuthorEndPoints();
    app.MapCategoryEndpoints();
    app.MapPostEndpoints();
    app.MapTagEndPoints();
    app.MapSubscribeEndpoints();

    app.Run();
}
