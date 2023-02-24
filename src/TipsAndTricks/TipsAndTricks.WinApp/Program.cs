using TipsAndTricks.Data.Contexts;
using TipsAndTricks.Services.Blogs;

var context = new BlogDbContext();
IBlogRepository blogRepository = new BlogRepository(context);
var posts = await blogRepository.GetPopularArticlesAsync(3);

foreach (var post in posts) {
    Console.WriteLine("ID       : {0}", post.Id);
    Console.WriteLine("Title    : {0}", post.Title);
    Console.WriteLine("View     : {0}", post.ViewCount);
    Console.WriteLine("Date     : {0:dd/MM/yyyy}", post.PostedDate);
    Console.WriteLine("Author   : {0}", post.Author.FullName);
    Console.WriteLine("Category : {0}", post.Category.Name);
    Console.WriteLine("".PadRight(80, '-'));
}
