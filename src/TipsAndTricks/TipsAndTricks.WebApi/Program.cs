using TipsAndTricks.WebApi.Extensions;
using TipsAndTricks.WebApi.Mapsters;

var builder = WebApplication.CreateBuilder(args); {
    builder.ConfigureCors()
            .ConfigureNLog()
            .ConfigureServices()
            .ConfigureSwaggerOpenApi()
            .ConfigureMapster();
}

var app = builder.Build(); {
    app.SetupRequestPipeLine();

    app.Run();
}
