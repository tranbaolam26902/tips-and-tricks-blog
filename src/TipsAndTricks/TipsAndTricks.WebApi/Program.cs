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

    app.Run();
}
