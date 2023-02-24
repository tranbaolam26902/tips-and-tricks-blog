using TipsAndTricks.Data.Contexts;
using TipsAndTricks.Data.Seeders;

var context = new BlogDbContext();
var seeder = new DataSeeder(context);

seeder.Initialize();

var authors = context.Authors.ToList();

Console.WriteLine("{0, -4}{1, -30}{2, -30}{3, 12}", "ID", "Full Name", "Email", "Joined Date");
foreach (var author in authors) {
    Console.WriteLine("{0, -4}{1, -30}{2, -30}{3, 12}", author.Id, author.FullName, author.Email, author.JoinedDate);
}