using TipsAndTricks.Data.Contexts;
using TipsAndTricks.Services.Blogs;

var context = new BlogDbContext();
IBlogRepository blogRepository = new BlogRepository(context);
var categories = await blogRepository.GetCategoriesAsync();

Console.WriteLine("{0, -5}{1, -50}{2, 10}", "ID", "Name", "Count");
foreach (var category in categories) {
    Console.WriteLine("{0, -5}{1, -50}{2, 10}", category.Id, category.Name, category.PostCount);
}