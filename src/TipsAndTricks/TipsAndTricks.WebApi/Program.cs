using TipsAndTricks.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args); {
    builder.ConfigureCors()
            .ConfigureNLog()
            .ConfigureServices()
            .ConfigureSwaggerOpenApi();
}

var app = builder.Build(); {
    app.SetupRequestPipeLine();

    app.Run();
}
