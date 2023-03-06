using Microsoft.EntityFrameworkCore;
using TipsAndTricks.Data.Contexts;
using TipsAndTricks.Data.Seeders;
using TipsAndTricks.Services.Blogs;

var builder = WebApplication.CreateBuilder(args); {
    builder.Services.AddControllersWithViews();
    builder.Services.AddDbContext<BlogDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Services.AddScoped<IBlogRepository, BlogRepository>();
    builder.Services.AddScoped<IDataSeeder, DataSeeder>();
}

var app = builder.Build(); {
    if (app.Environment.IsDevelopment())
        app.UseDeveloperExceptionPage();
    else {
        app.UseExceptionHandler("/Blog/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.MapControllerRoute(name: "default", pattern: "{controller=Blog}/{action=Index}/{id?}");
}

using (var scope = app.Services.CreateScope()) {
    var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
    seeder.Initialize();
}

app.Run();
