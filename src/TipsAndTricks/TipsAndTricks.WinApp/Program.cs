using TipsAndTricks.Data.Contexts;

var context = new BlogDbContext();
// 1.
//IBlogRepository blogRepository = new BlogRepository(context);

// 2.
//IAuthorRepository authorRepository = new AuthorRepository(context);

// 3.
//ISubscriberRepository subscriberRepository = new SubscriberRepository(context);

#region 1.
#region 1a. Get Tag by Slug
//var tag = await blogRepository.GetTagBySlugAsync("neural-network");
//ConsoleLogExtensions.PrintTag(tag);
#endregion

#region 1c. Get Tags
//var tags = await blogRepository.GetTagsAsync();
//ConsoleLogExtensions.PrintTags(tags);
#endregion

#region 1d. Delete Tag by Id
//bool isSuccess = await blogRepository.DeleteTagByIdAsync(2);
//Console.WriteLine(isSuccess);
#endregion

#region 1e. Get Category by Slug
//var category = await blogRepository.GetCategoryBySlugAsync("architecture");
//ConsoleLogExtensions.PrintCategory(category);
#endregion

#region 1f. Get Category by Id
//var category = await blogRepository.GetCategoryByIdAsync(4);
//ConsoleLogExtensions.PrintCategory(category);
#endregion

#region 1g. Edit Category
//var newCategory = new Category() {
//    Id = 16, // Comment this line to add new Category
//    Name = "New Category",
//    Description = "New Category (edited)",
//    UrlSlug = "new-category"
//};
//var check = await blogRepository.EditCategoryAsync(newCategory);
//var categories = await blogRepository.GetCategoriesAsync();
//ConsoleLogExtensions.PrintCategory(check);
//Console.WriteLine();
//Console.WriteLine();
//ConsoleLogExtensions.PrintCategories(categories);
#endregion

#region 1h. Delete Category by Id
//var isSuccess = await blogRepository.DeleteCategoryByIdAsync(16);
//Console.WriteLine(isSuccess);
//var categories = await blogRepository.GetCategoriesAsync();
//ConsoleLogExtensions.PrintCategories(categories);
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

#region 1l. Get Post by Id
//var post = await blogRepository.GetPostByIdAsync(4);
//ConsoleLogExtensions.PrintPost(post);
#endregion

#region 1m. Edit Post
//var newPost = new Post() {
//    Id = 11, // Comment this line to add new Post
//    Title = "New Post (edited)",
//    ShortDescription = "New Post Short Description. (edited)",
//    Description = "New Post Description. Lorem ipsum dolor sit amet.",
//    Meta = "new-post",
//    UrlSlug = "new-post",
//    Published = true,
//    PostedDate = DateTime.Now,
//    ModifiedDate = null,
//    ViewCount = 1,
//    AuthorId = 3,
//    CategoryId = 4,
//};
//await blogRepository.EditPostAsync(newPost);
//var posts = await blogRepository.GetPostsAsync();
//ConsoleLogExtensions.PrintPosts(posts);
#endregion

#region 1n. Change Post's Published status
//await blogRepository.ChangePostPublishedStatusAsync(6, false);
//var post = await blogRepository.GetPostByIdAsync(6);
//ConsoleLogExtensions.PrintPost(post);
#endregion

#region 1o. Get random Posts
//var posts = await blogRepository.GetRandomPostsAsync(4);
//ConsoleLogExtensions.PrintPosts(posts);
#endregion

#region 1q. Find Posts by Queries
//var postQuery = new PostQuery() {
//    CategoryId = 10,
//    AuthorId = 1
//};
//var posts = await blogRepository.GetPostsByQuery(postQuery);
//ConsoleLogExtensions.PrintPosts(posts);
#endregion

#region 1r. Count number of Posts by Queries
//var postQuery = new PostQuery() {
//    CategoryId = 10,
//    AuthorId = 1
//};
//var numberOfPosts = await blogRepository.CountPostsByQueryAsync(postQuery);
//Console.WriteLine("Total posts: {0}", numberOfPosts);
#endregion

#region 1s. Paginate Posts found by queries
//var postQuery = new PostQuery() {
//    CategoryId = 10,
//    AuthorId = 1,
//    PostedYear = 2022,
//};
//var pagingParams = new PagingParams {
//    PageNumber = 1,
//    PageSize = 10,
//    SortColumn = "Id"
//};
//var posts = await blogRepository.GetPagedPostsByQueryAsync(postQuery, pagingParams);
//foreach (var post in posts) {
//    Console.WriteLine("Id: {0}", post.Id);
//    Console.WriteLine("Title: {0}", post.Title);
//    Console.WriteLine("Short description: {0}", post.ShortDescription);
//    Console.WriteLine("Description: {0}", post.Description);
//    Console.WriteLine("Meta: {0}", post.Meta);
//    Console.WriteLine("Url slug: {0}", post.UrlSlug);
//    Console.WriteLine("Image url: {0}", post.ImageUrl);
//    Console.WriteLine("View: {0}", post.ViewCount);
//    Console.WriteLine("Published: {0}", post.Published);
//    Console.WriteLine("Posted date: {0:dd/MM/yyyy}", post.PostedDate);
//    Console.WriteLine("Modified date: {0:dd/MM/yyyy}", post.ModifiedDate);
//    Console.WriteLine("Author: {0}", post.Author.FullName);
//    Console.WriteLine("Category: {0}", post.Category.Name);
//    Console.WriteLine("----------------------------------------");
//}
#endregion
#endregion

#region 2.
#region 2b. Get Author by Id
//var author = await authorRepository.GetAuthorByIdAsync(4);
//ConsoleLogExtensions.PrintAuthor(author);
#endregion

#region 2b. Get Author by Id
//var author = await authorRepository.GetAuthorBySlugAsync("leanne-graham");
//ConsoleLogExtensions.PrintAuthor(author);
#endregion

#region 2d. Paginate Authors
//var pagingParams = new PagingParams {
//    PageNumber = 1,
//    PageSize = 10,
//    SortColumn = "Id"
//};
//var authors = await authorRepository.GetPagedAuthorsAsync(pagingParams);
//foreach (var author in authors) {
//    Console.WriteLine("Id: {0}", author.Id);
//    Console.WriteLine("Name: {0}", author.FullName);
//    Console.WriteLine("Url slug: {0}", author.UrlSlug);
//    Console.WriteLine("Email: {0}", author.Email);
//    Console.WriteLine("Total posts: {0}", author.PostCount);
//    Console.WriteLine("----------------------------------------");
//}
#endregion

#region 2e. Edit Author
//var newAuthor = new Author() {
//    Id = 8, // Comment this line to add new Author
//    FullName = "Tran Bao Lam (edited)",
//    UrlSlug = "tran-bao-lam",
//    Email = "2011401@dlu.edu.vn",
//    JoinedDate = DateTime.Now
//};
//var isSuccess = await authorRepository.EditAuthorAsync(newAuthor);
//ConsoleLogExtensions.PrintAuthor(isSuccess);
#endregion

#region 2f. Get Authors has most Articles
//var authors = await authorRepository.GetAuthorsHasMostArticles(1);
//ConsoleLogExtensions.PrintAuthors(authors);
#endregion
#endregion

#region 3.
#region Subscribe blog
//var isSuccess = await subscriberRepository.SubscribeAsync("2011401@dlu.edu.vn");
//Console.WriteLine(isSuccess);
//var subscribers = await subscriberRepository.GetSubscribersAsync();
//ConsoleLogExtensions.PrintSubscribers(subscribers);
#endregion

#region Unsubscribe 
//var isSuccess = await subscriberRepository.UnsubscribeAsync("2011401@dlu.edu.vn", "No useful");
//Console.WriteLine(isSuccess);
//var subscribers = await subscriberRepository.GetSubscribersAsync();
//ConsoleLogExtensions.PrintSubscribers(subscribers);
#endregion

#region Ban Subscriber
//var isSuccess = await subscriberRepository.BanSubscriberAsync(1, "Spam", "Spammmm");
//Console.WriteLine(isSuccess);
//var isSubscribe = await subscriberRepository.SubscribeAsync("2011401@dlu.edu.vn");
//Console.WriteLine(isSubscribe);
//var subscribers = await subscriberRepository.GetSubscribersAsync();
//ConsoleLogExtensions.PrintSubscribers(subscribers);
#endregion

#region Delete Subscriber
//var isSuccess = await subscriberRepository.DeleteSubscriberAsync(1);
//Console.WriteLine(isSuccess);
//var subscribers = await subscriberRepository.GetSubscribersAsync();
//ConsoleLogExtensions.PrintSubscribers(subscribers);
#endregion

#region Get Subscriber by Id
//var subscriber = await subscriberRepository.GetSubscriberByIdAsync(2);
//ConsoleLogExtensions.PrintSubscriber(subscriber);
#endregion

#region Get Subscriber by Email
//var subscriber = await subscriberRepository.GetSubscriberByEmailAsync("2011401@dlu.edu.vn");
//ConsoleLogExtensions.PrintSubscriber(subscriber);
#endregion

#region Search Subscriber
//var pagingParams = new PagingParams {
//    PageNumber = 1,
//    PageSize = 5,
//    SortColumn = "Id"
//};
//var subscribers = await subscriberRepository.SearchSubscribersAsync(pagingParams, "2011", SubscribeState.Subscribe);
//foreach (var subscriber in subscribers) {
//    Console.WriteLine("Id: {0}", subscriber.Id);
//    Console.WriteLine("Email: {0}", subscriber.Email);
//    Console.WriteLine("Subscribed date: {0:dd/MM/yyyy}", subscriber.SubscribedDate);
//    Console.WriteLine("Unsubscribed date: {0:dd/MM/yyyy}", subscriber.UnsubscribedDate);
//    Console.WriteLine("Subscribed state: {0}", subscriber.SubscribeState);
//    Console.WriteLine("Reason: {0}", subscriber.Reason);
//    Console.WriteLine("Notes: {0}", subscriber.Notes);
//    Console.WriteLine("----------------------------------------");
//}
#endregion
#endregion