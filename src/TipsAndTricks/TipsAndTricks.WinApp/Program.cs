using TipsAndTricks.Data.Contexts;
using TipsAndTricks.Services.Blogs;
using TipsAndTricks.WinApp;

var context = new BlogDbContext();
IBlogRepository blogRepository = new BlogRepository(context);
var pagingParams = new PagingParams {
    PageNumber = 1,
    PageSize = 5,
    SortColumn = "Name",
    SortOrder = "DESC",
};

var tags = await blogRepository.GetPagedTagsAsync(pagingParams);

Console.WriteLine("{0, -5}{1, -50}{2, 10}", "ID", "Name", "Count");
foreach (var tag in tags) {
    Console.WriteLine("{0, -5}{1, -50}{2, 10}", tag.Id, tag.Name, tag.PostCount);
}