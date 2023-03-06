using TipsAndTricks.Core.DTO;
using TipsAndTricks.Core.Entities;

namespace TipsAndTricks.Services.Extensions {
    public static class ConsoleLogExtensions {
        /// <summary>
        /// Print Author information
        /// </summary>
        /// <param name="author">Author</param>
        public static void PrintAuthor(Author author) {
            if (author == null) {
                Console.WriteLine("Item is null!");
                return;
            }
            Console.WriteLine("Id: {0}", author.Id);
            Console.WriteLine("Name: {0}", author.FullName);
            Console.WriteLine("Url slug: {0}", author.UrlSlug);
            Console.WriteLine("Image url: {0}", author.ImageUrl);
            Console.WriteLine("Joined date: {0:dd/MM/yyyy}", author.JoinedDate);
            Console.WriteLine("Email: {0}", author.Email);
            Console.WriteLine("Notes: {0}", author.Notes);
            if (author.Posts != null)
                Console.WriteLine("Total posts: {0}", author.Posts.Count());
        }

        /// <summary>
        /// Print Category information
        /// </summary>
        /// <param name="category">Category</param>
        public static void PrintCategory(Category category) {
            if (category == null) {
                Console.WriteLine("Item is null!");
                return;
            }
            Console.WriteLine("Id: {0}", category.Id);
            Console.WriteLine("Name: {0}", category.Name);
            Console.WriteLine("Url slug: {0}", category.UrlSlug);
            Console.WriteLine("Description: {0}", category.Description);
            Console.WriteLine("Show on menu: {0}", category.ShowOnMenu);
            Console.WriteLine("Total posts: {0}", category.Posts.Count());
        }

        /// <summary>
        /// Print Category information
        /// </summary>
        /// <param name="category">CategoryItem</param>
        public static void PrintCategory(CategoryItem category) {
            if (category == null) {
                Console.WriteLine("Item is null!");
                return;
            }
            Console.WriteLine("Id: {0}", category.Id);
            Console.WriteLine("Name: {0}", category.Name);
            Console.WriteLine("Url slug: {0}", category.UrlSlug);
            Console.WriteLine("Description: {0}", category.Description);
            Console.WriteLine("Show on menu: {0}", category.ShowOnMenu);
            Console.WriteLine("Total posts: {0}", category.PostCount);
        }

        /// <summary>
        /// Print Tag information
        /// </summary>
        /// <param name="tag">Tag</param>
        public static void PrintTag(Tag tag) {
            if (tag == null) {
                Console.WriteLine("Item is null!");
                return;
            }
            Console.WriteLine("Id: {0}", tag.Id);
            Console.WriteLine("Name: {0}", tag.Name);
            Console.WriteLine("Url slug: {0}", tag.UrlSlug);
            Console.WriteLine("Description: {0}", tag.Description);
            Console.WriteLine("Total posts: {0}", tag.Posts.Count());
        }

        /// <summary>
        /// Print Tag information
        /// </summary>
        /// <param name="tag">TagItem</param>
        public static void PrintTag(TagItem tag) {
            if (tag == null) {
                Console.WriteLine("Item is null!");
                return;
            }
            Console.WriteLine("Id: {0}", tag.Id);
            Console.WriteLine("Name: {0}", tag.Name);
            Console.WriteLine("Url slug: {0}", tag.UrlSlug);
            Console.WriteLine("Description: {0}", tag.Description);
            Console.WriteLine("Total posts: {0}", tag.PostCount);
        }

        /// <summary>
        /// Print Post information
        /// </summary>
        /// <param name="post">Post</param>
        public static void PrintPost(Post post) {
            if (post == null) {
                Console.WriteLine("Item is null!");
                return;
            };
            Console.WriteLine("Id: {0}", post.Id);
            Console.WriteLine("Title: {0}", post.Title);
            Console.WriteLine("Short description: {0}", post.ShortDescription);
            Console.WriteLine("Description: {0}", post.Description);
            Console.WriteLine("Meta: {0}", post.Meta);
            Console.WriteLine("Url slug: {0}", post.UrlSlug);
            Console.WriteLine("Image url: {0}", post.ImageUrl);
            Console.WriteLine("View: {0}", post.ViewCount);
            Console.WriteLine("Published: {0}", post.Published);
            Console.WriteLine("Posted date: {0:dd/MM/yyyy}", post.PostedDate);
            Console.WriteLine("Modified date: {0:dd/MM/yyyy}", post.ModifiedDate);
            Console.WriteLine("Author: {0}", post.Author.FullName);
            Console.WriteLine("Category: {0}", post.Category.Name);
        }

        /// <summary>
        /// Print list of Authors
        /// </summary>
        /// <param name="authors">List Authors</param>
        public static void PrintAuthors(IList<Author> authors) {
            foreach (var author in authors) {
                PrintAuthor(author);
                Console.WriteLine("----------------------------------------");
            }
        }

        /// <summary>
        /// Print list of Categories
        /// </summary>
        /// <param name="authors">List Categories</param>
        public static void PrintCategories(IList<Category> categories) {
            foreach (var category in categories) {
                PrintCategory(category);
                Console.WriteLine("----------------------------------------");
            }
        }

        /// <summary>
        /// Print list of Categories
        /// </summary>
        /// <param name="authors">List CategoryItems</param>
        public static void PrintCategories(IList<CategoryItem> categories) {
            foreach (var category in categories) {
                PrintCategory(category);
                Console.WriteLine("----------------------------------------");
            }
        }

        /// <summary>
        /// Print list of Tags
        /// </summary>
        /// <param name="tags">List Tags</param>
        public static void PrintTags(IList<Tag> tags) {
            foreach (var tag in tags) {
                PrintTag(tag);
                Console.WriteLine("----------------------------------------");
            }
        }

        /// <summary>
        /// Print list of Tags
        /// </summary>
        /// <param name="tags">List TagItems</param>
        public static void PrintTags(IList<TagItem> tags) {
            foreach (var tag in tags) {
                PrintTag(tag);
                Console.WriteLine("----------------------------------------");
            }
        }

        /// <summary>
        /// Print list of Posts
        /// </summary>
        /// <param name="posts">List Posts</param>
        public static void PrintPosts(IList<Post> posts) {
            foreach (var post in posts) {
                PrintPost(post);
                Console.WriteLine("----------------------------------------");
            }
        }
    }
}
