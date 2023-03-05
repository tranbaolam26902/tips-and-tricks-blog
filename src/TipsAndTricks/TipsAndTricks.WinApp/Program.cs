using TipsAndTricks.Data.Contexts;
using TipsAndTricks.Services.Blogs;

var context = new BlogDbContext();
IBlogRepository blogRepository = new BlogRepository(context);

#region 1.
#region 1a. Get Tag by Slug
//var tag = await blogRepository.GetTagBySlugAsync("cc");
//Console.WriteLine("{0, -5}{1, -10}{2, 20}{3, 20}", "ID", "Name", "Description", "Slug");
//Console.WriteLine("{0, -5}{1, -10}{2, 20}{3, 20}", tag?.Id, tag?.Name, tag?.Description, tag?.UrlSlug);
#endregion

#region 1c. Get Tags
//var tags = await blogRepository.GetTagsAsync();
//Console.WriteLine("{0, -5}{1, -30}{2, -30}{3, -25}{4, 10}", "ID", "Name", "Description", "Slug", "Posts");
//foreach (var tag in tags) {
//    Console.WriteLine("{0, -5}{1, -30}{2, -30}{3, -25}{4, 10}", tag.Id, tag.Name, tag.Description, tag.UrlSlug, tag.PostCount);
//}
#endregion

#region 1d. Delete Tag by Id
//bool isSuccess = await blogRepository.DeleteTagByNameAsync(2);
//Console.WriteLine(isSuccess);
#endregion

#region 1e. Get Category by Slug
//var category = await blogRepository.GetCategoryBySlugAsync("architecturek");
//Console.WriteLine("{0, -5}{1, -10}{2, 20}{3, 20}", "ID", "Name", "Description", "Slug");
//Console.WriteLine("{0, -5}{1, -10}{2, 20}{3, 20}", category?.Id, category?.Name, category?.Description, category?.UrlSlug);
#endregion

#region 1f. Get Category by Id
//var category = await blogRepository.GetCategoryByIdAsync(4);
//Console.WriteLine("{0, -5}{1, -10}{2, 28}{3, 20}", "ID", "Name", "Description", "Slug");
//Console.WriteLine("{0, -5}{1, -10}{2, 28}{3, 20}", category?.Id, category?.Name, category?.Description, category?.UrlSlug);
#endregion

#region 1g. Edit Category
//var newCategory = new Category() {
//    Id = 1,
//    Name = "New Category (edited)",
//    UrlSlug = "new-category-edited",
//    Description = "New Category Description (edited)",
//};

//var check = await blogRepository.EditCategoryAsync(newCategory);
//Console.WriteLine("{0, -5}{1, -10}{2, 28}{3, 20}", "ID", "Name", "Description", "Slug");
//Console.WriteLine("{0, -5}{1, -10}{2, 28}{3, 20}", check?.Id, check?.Name, check?.Description, check?.UrlSlug);

//Console.WriteLine();
//Console.WriteLine();
//var categories = await blogRepository.GetCategoriesAsync();
//Console.WriteLine("{0, -5}{1, -20}{2, 40}{3, 20}", "ID", "Name", "Description", "Slug");
//foreach (var category in categories) {
//    Console.WriteLine("{0, -5}{1, -20}{2, 40}{3, 20}", category?.Id, category?.Name, category?.Description, category?.UrlSlug);
//}
#endregion

#region 1h. Delete Category by Id
//var isSuccess = await blogRepository.DeleteCategoryByIdAsync(16);
//Console.WriteLine(isSuccess);
#endregion

#region 1i. Check whether Category's Slug is existed
//var isExisted = await blogRepository.IsCategorySlugExistedAsync("oop");
//Console.WriteLine(isExisted);
#endregion

#region 1j. Paginate Categories
//var pagingParams = new PagingParams {
//    PageNumber = 1,
//    PageSize = 5,
//    SortColumn = "Id"
//};

//var categories = await blogRepository.GetPagedCategoriesAsync(pagingParams);
//Console.WriteLine("{0, -5}{1, -30}{2, 40}{3, 20}{4, 15}", "ID", "Name", "Description", "Slug", "Posts");
//foreach (var category in categories) {
//    Console.WriteLine("{0, -5}{1, -30}{2, 40}{3, 20}{4, 15}", category?.Id, category?.Name, category?.Description, category?.UrlSlug, category?.PostCount);
//}
#endregion
#endregion