using TipsAndTricks.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args); {
    builder.ConfigureNLog();
}

var app = builder.Build();

app.Run();
