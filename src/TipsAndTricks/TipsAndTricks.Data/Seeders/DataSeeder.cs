using TipsAndTricks.Core.Entities;
using TipsAndTricks.Data.Contexts;

namespace TipsAndTricks.Data.Seeders {
    public class DataSeeder : IDataSeeder {
        private readonly BlogDbContext _dbContext;

        public DataSeeder(BlogDbContext dbContext) {
            this._dbContext = dbContext;
        }

        public void Initialize() {
            _dbContext.Database.EnsureCreated();
            if (_dbContext.Posts.Any())
                return;
            var authors = AddAuthors();
            var categories = AddCategories();
            var tags = AddTags();
            var posts = AddPosts(authors, categories, tags);
        }

        private IList<Author> AddAuthors() {
            var authors = new List<Author>() {
                new() {
                    FullName = "Jason Mouth",
                    UrlSlug = "json-mouth",
                    Email = "json@gmail.com",
                    JoinedDate = new DateTime(2022, 10, 21)
                },
                new() {
                    FullName = "Jessica Wonder",
                    UrlSlug = "jessica-wonder",
                    Email = "jessica665@motip.com",
                    JoinedDate = new DateTime(2020, 4, 19)
                },
                new() {
                    FullName = "Leanne Graham",
                    UrlSlug = "leanne-graham",
                    Email = "leanne@gmail.com",
                    JoinedDate = new DateTime(2022, 12, 1)
                },
                new() {
                    FullName = "Ervin Howell",
                    UrlSlug = "ervin-howell",
                    Email = "ervin@gmail.com",
                    JoinedDate = new DateTime(2023, 1, 22)
                },
                new() {
                    FullName = "Clementine Bauch",
                    UrlSlug = "clementine-bauch",
                    Email = "clementine@gmail.com",
                    JoinedDate = new DateTime(2022, 11, 23)
                },
                new() {
                    FullName = "Patricia Lebsack",
                    UrlSlug = "patricia-lebsack",
                    Email = "patricia@gmail.com",
                    JoinedDate = new DateTime(2021, 7, 8)
                },
                new() {
                    FullName = "Chelsey Dietrich",
                    UrlSlug = "chelsey-dietrich",
                    Email = "chelsey@gmail.com",
                    JoinedDate = new DateTime(2022, 3, 14)
                }
            };

            _dbContext.Authors.AddRange(authors);
            _dbContext.SaveChanges();

            return authors;
        }

        private IList<Category> AddCategories() {
            var categories = new List<Category>() {
                new() {Name = ".NET Core", Description = ".NET Core", ShowOnMenu = true, UrlSlug = "net-core"},
                new() {Name = "Architecture", Description = "Architecture", ShowOnMenu = true, UrlSlug = "architecture"},
                new() {Name = "Messaging", Description = "Messaging", ShowOnMenu = true, UrlSlug = "messaging"},
                new() {Name = "OOP", Description = "Object-Oriented Program", ShowOnMenu = true, UrlSlug = "oop"},
                new() {Name = "Design Patterns", Description = "Design Patterns", ShowOnMenu = true, UrlSlug = "design-patterns"},
                new() {Name = "React", Description = "React", ShowOnMenu = true, UrlSlug = "react"},
                new() {Name = "Angular", Description = "Angular", ShowOnMenu = true, UrlSlug = "angular"},
                new() {Name = "Vue.js", Description = "Vue.js", ShowOnMenu = true, UrlSlug = "vue-js"},
                new() {Name = "Next.js", Description = "Next.js", ShowOnMenu = true, UrlSlug = "next-js"},
                new() {Name = "Node.js", Description = "Node.js", ShowOnMenu = true, UrlSlug = "node-js"},
                new() {Name = "Golang", Description = "Golang", ShowOnMenu = true, UrlSlug = "golang"},
                new() {Name = "Three.js", Description = "Three.js", ShowOnMenu = true, UrlSlug = "three-js"},
                new() {Name = "PHP", Description = "PHP", ShowOnMenu = true, UrlSlug = "php"},
                new() {Name = "Laravel", Description = "Laravel", ShowOnMenu = true, UrlSlug = "laravel"},
                new() {Name = "Svelte", Description = "Svelte", ShowOnMenu = true, UrlSlug = "svelte"}
            };

            _dbContext.Categories.AddRange(categories);
            _dbContext.SaveChanges();

            return categories;
        }

        private IList<Tag> AddTags() {
            var tags = new List<Tag>() {
                new() {Name = "Google", Description = "Google", UrlSlug = "google"},
                new() {Name = "ASP.NET MVC", Description = "ASP.NET MVC", UrlSlug = "asp-net-mvc"},
                new() {Name = "Razor Page", Description = "Razor Page", UrlSlug = "razor-page"},
                new() {Name = "Blazor", Description = "Blazor", UrlSlug = "blazor"},
                new() {Name = "Deep Learning", Description = "Deep Learning", UrlSlug = "deep-learning"},
                new() {Name = "Neural Network", Description = "Neural Network", UrlSlug = "neural-network"},
                new() {Name = "Front-End Applications", Description = "Front-End Applications", UrlSlug = "font-end-application"},
                new() {Name = "Visual Studio", Description = "Visual Studio", UrlSlug = "visual-studio"},
                new() {Name = "SQL Server", Description = "SQL Server", UrlSlug = "sql-server"},
                new() {Name = "Git", Description = "Git", UrlSlug = "git"},
                new() {Name = "Entity Framework Core", Description = "EF Core", UrlSlug = "entity-framework-core"},
                new() {Name = ".NET Framework", Description = ".NET Framework", UrlSlug = "net-framework"},
                new() {Name = "ASP.NET Core", Description = "ASP.NET Core", UrlSlug = "aspnet-core"},
                new() {Name = "Postman", Description = "Postman", UrlSlug = "postman"},
                new() {Name = "ChatGPT", Description = "Chat GPT", UrlSlug = "chat-gpt"},
                new() {Name = "Data cleansing", Description = "Data cleaning", UrlSlug = "data-cleansing"},
                new() {Name = "Fetch API", Description = "Fetch API", UrlSlug = "fetch-api"},
                new() {Name = "Microsoft", Description = "Microsoft", UrlSlug = "microsoft"},
                new() {Name = "Microservices", Description = "Microservices", UrlSlug = "microservices"},
                new() {Name = "Web API Security", Description = "Web API Security", UrlSlug = "web-api-security"}
            };

            _dbContext.Tags.AddRange(tags);
            _dbContext.SaveChanges();

            return tags;
        }

        private IList<Post> AddPosts(IList<Author> authors, IList<Category> categories, IList<Tag> tags) {
            var posts = new List<Post>() {
                new() {
                    Title = "His mother had always taught him",
                    ShortDescription = "His mother had always taught him not to ever think of himself as better than others.",
                    Description = "His mother had always taught him not to ever think of himself as better than others. He'd tried to live by this motto. He never looked down on those who were less fortunate or who had less money than him. But the stupidity of the group of people he was talking to made him change his mind.",
                    Meta = "post-01",
                    UrlSlug = "his-mother-had-always-taught-him",
                    Published = true,
                    PostedDate = new DateTime(2023, 2, 22, 1, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 2,
                    Author = authors[0],
                    Category = categories[0],
                    Tags = new List<Tag>()
                    {
                        tags[0],
                        tags[1],
                        tags[2],
                    }
                },
                new() {
                    Title = "He was an expert but not in a discipline",
                    ShortDescription = "He was an expert but not in a discipline that anyone could fully appreciate.",
                    Description = "He was an expert but not in a discipline that anyone could fully appreciate. He knew how to hold the cone just right so that the soft server ice-cream fell into it at the precise angle to form a perfect cone each and every time. It had taken years to perfect and he could now do it without even putting any thought behind it.",
                    Meta = "post-02",
                    UrlSlug = "he-was-an-expert-but-not-in-a-discipline",
                    Published = true,
                    PostedDate = new DateTime(2022, 12, 1, 7, 6, 0),
                    ModifiedDate = null,
                    ViewCount = 2,
                    Author = authors[2],
                    Category = categories[4],
                    Tags = new List<Tag>()
                    {
                        tags[2],
                        tags[3],
                        tags[4],
                    }
                },
                new() {
                    Title = "Dave watched as the forest burned up on the hill",
                    ShortDescription = "Dave watched as the forest burned up on the hill.",
                    Description = "Dave watched as the forest burned up on the hill, only a few miles from her house. The car had been hastily packed and Marta was inside trying to round up the last of the pets. Dave went through his mental list of the most important papers and documents that they couldn't leave behind. He scolded himself for not having prepared these better in advance and hoped that he had remembered everything that was needed. He continued to wait for Marta to appear with the pets, but she still was nowhere to be seen.",
                    Meta = "post-03",
                    UrlSlug = "dave-watched-as-the-forest-burned-up-on-the-hill",
                    Published = true,
                    PostedDate = new DateTime(2023, 1, 26, 6, 1, 0),
                    ModifiedDate = null,
                    ViewCount = 5,
                    Author = authors[3],
                    Category = categories[5],
                    Tags = new List<Tag>()
                    {
                        tags[3],
                        tags[4],
                        tags[5],
                    }
                },
                new() {
                    Title = "All he wanted was a candy bar.",
                    ShortDescription = "All he wanted was a candy bar.",
                    Description = "All he wanted was a candy bar. It didn't seem like a difficult request to comprehend, but the clerk remained frozen and didn't seem to want to honor the request. It might have had something to do with the gun pointed at his face.",
                    Meta = "post-04",
                    UrlSlug = "all-he-wanted-was-a-candy-bar",
                    Published = true,
                    PostedDate = new DateTime(2022, 11, 1, 6, 32, 0),
                    ModifiedDate = null,
                    ViewCount = 1,
                    Author = authors[3],
                    Category = categories[6],
                    Tags = new List<Tag>()
                    {
                        tags[4],
                        tags[5],
                    }
                },
                new() {
                    Title = "Hopes and dreams were dashed that day",
                    ShortDescription = "Hopes and dreams were dashed that day.",
                    Description = "Hopes and dreams were dashed that day. It should have been expected, but it still came as a shock. The warning signs had been ignored in favor of the possibility, however remote, that it could actually happen. That possibility had grown from hope to an undeniable belief it must be destiny. That was until it wasn't and the hopes and dreams came crashing down.",
                    Meta = "post-05",
                    UrlSlug = "hopes-and-dreams-were-dashed-that-day",
                    Published = true,
                    PostedDate = new DateTime(2022, 10, 7, 7, 53, 0),
                    ModifiedDate = null,
                    ViewCount = 2,
                    Author = authors[1],
                    Category = categories[7],
                    Tags = new List<Tag>()
                    {
                        tags[7],
                        tags[1],
                        tags[2],
                    }
                },
                new() {
                    Title = "Dave wasn't exactly sure how he had ended up in this predicament",
                    ShortDescription = "Dave wasn't exactly sure how he had ended up in this predicament.",
                    Description = "Dave wasn't exactly sure how he had ended up in this predicament. He ran through all the events that had lead to this current situation and it still didn't make sense. He wanted to spend some time to try and make sense of it all, but he had higher priorities at the moment. The first was how to get out of his current situation of being naked in a tree with snow falling all around and no way for him to get down.",
                    Meta = "post-06",
                    UrlSlug = "dave-wasnt-exactly-sure-how-he-had-ended-up-in-this-predicament",
                    Published = true,
                    PostedDate = new DateTime(2022, 4, 30, 8, 0, 0),
                    ModifiedDate = null,
                    ViewCount = 3,
                    Author = authors[4],
                    Category = categories[4],
                    Tags = new List<Tag>()
                    {
                        tags[5],
                        tags[1],
                        tags[2],
                    }
                },
                new() {
                    Title = "This is important to remember",
                    ShortDescription = "This is important to remember.",
                    Description = "This is important to remember. Love isn't like pie. You don't need to divide it among all your friends and loved ones. No matter how much love you give, you can always give more. It doesn't run out, so don't try to hold back giving it as if it may one day run out. Give it freely and as much as you want.",
                    Meta = "post-07",
                    UrlSlug = "this-is-important-to-remember",
                    Published = true,
                    PostedDate = new DateTime(2022, 12, 1, 3, 8, 0),
                    ModifiedDate = null,
                    ViewCount = 8,
                    Author = authors[4],
                    Category = categories[5],
                    Tags = new List<Tag>()
                    {
                        tags[0],
                        tags[1],
                        tags[2],
                    }
                },
                new() {
                    Title = "One can cook on and with an open fire",
                    ShortDescription = "One can cook on and with an open fire.",
                    Description = "One can cook on and with an open fire. These are some of the ways to cook with fire outside. Cooking meat using a spit is a great way to evenly cook meat. In order to keep meat from burning, it's best to slowly rotate it.",
                    Meta = "post-08",
                    UrlSlug = "one-can-cook-on-and-with-an-open-fire",
                    Published = true,
                    PostedDate = new DateTime(2023, 2, 22, 2, 12, 0),
                    ModifiedDate = null,
                    ViewCount = 9,
                    Author = authors[1],
                    Category = categories[9],
                    Tags = new List<Tag>()
                    {
                        tags[9],
                        tags[5],
                        tags[15],
                    }
                },
                new() {
                    Title = "There are different types of secrets",
                    ShortDescription = "There are different types of secrets.",
                    Description = "There are different types of secrets. She had held onto plenty of them during her life, but this one was different. She found herself holding onto the worst type. It was the type of secret that could gnaw away at your insides if you didn't tell someone about it, but it could end up getting you killed if you did.",
                    Meta = "post-09",
                    UrlSlug = "there-are-different-types-of-secrets",
                    Published = true,
                    PostedDate = new DateTime(2022, 11, 8, 12, 2, 0),
                    ModifiedDate = null,
                    ViewCount = 2,
                    Author = authors[3],
                    Category = categories[6],
                    Tags = new List<Tag>()
                    {
                        tags[10],
                        tags[13],
                        tags[14],
                    }
                },
                new() {
                    Title = "They rushed out the door",
                    ShortDescription = "They rushed out the door, grabbing anything and everything they could think of they might need.",
                    Description = "They rushed out the door, grabbing anything and everything they could think of they might need. There was no time to double-check to make sure they weren't leaving something important behind. Everything was thrown into the car and they sped off. Thirty minutes later they were safe and that was when it dawned on them that they had forgotten the most important thing of all.",
                    Meta = "post-10",
                    UrlSlug = "they-rushed-out-the-door",
                    Published = true,
                    PostedDate = new DateTime(2022, 6, 7, 8, 9, 0),
                    ModifiedDate = null,
                    ViewCount = 4,
                    Author = authors[2],
                    Category = categories[6],
                    Tags = new List<Tag>()
                    {
                        tags[10],
                        tags[15],
                        tags[14],
                    }
                },
                new() {
                    Title = "Using Reactive UI in your .NET MAUI app",
                    ShortDescription = "Using Reactive UI in your .NET MAUI app, grabbing anything and everything they could think of they might need.",
                    Description = "Using Reactive UI in your.NET MAUI app. We will rewrite the Hello World.NETMAUI project using ReactiveUI in this post. We will implement a property that we will bind to the button text.",
                    Meta = "post-11",
                    UrlSlug = "using-reactive-ui-in-your-net-maui-app",
                    Published = true,
                    PostedDate = new DateTime(2023, 2, 3),
                    ModifiedDate = null,
                    ViewCount = 12,
                    Author = authors[4],
                    Category = categories[6],
                    Tags = new List<Tag>()
                    {
                        tags[5],
                        tags[6],
                        tags[9],
                        tags[12],
                        tags[14],
                    }
                },
                new() {
                    Title = "Daily bit(e) of C++ | Learn Modern C++ 3/N",
                    ShortDescription = "Daily bit(e) of C++ | Learn Modern C++ 3/N",
                    Description = "Today we will take a crash course in types. If you missed the previous lesson, check it out here: Learn Modern C++ 2/N Daily bit(e) of C++ #90. A Modern-only C++ course (including C++23) is part 2 of N itnext.",
                    Meta = "post-12",
                    UrlSlug = "daily-bit-e-of-c-or-learn-modern-c-3-n",
                    Published = true,
                    PostedDate = new DateTime(2023, 1, 23),
                    ModifiedDate = null,
                    ViewCount = 30,
                    Author = authors[2],
                    Category = categories[8],
                    Tags = new List<Tag>()
                    {
                        tags[3],
                        tags[6],
                        tags[10],
                        tags[17],
                        tags[16],
                    }
                },
                new() {
                    Title = "Retrying a bash command",
                    ShortDescription = "Retrying a bash command",
                    Description = "To retry a command in Bash, you can use a loop to execute a command. The loop will execute the command and if the command returns a non-zero exit code, the loop will try again. If the command succeeds, theloop will exit.",
                    Meta = "post-13",
                    UrlSlug = "retrying-a-bash-command",
                    Published = true,
                    PostedDate = new DateTime(2023, 2, 16),
                    ModifiedDate = null,
                    ViewCount = 25,
                    Author = authors[3],
                    Category = categories[1],
                    Tags = new List<Tag>()
                    {
                        tags[1],
                        tags[6],
                        tags[8],
                        tags[17],
                        tags[16],
                    }
                },
                new() {
                    Title = "Open Closed Principle in SOLID",
                    ShortDescription = "Open Closed Principle in SOLID",
                    Description = "The Open Closed Principle is an essential principle of object-oriented design that states that software entities should be. We all are using OCP principle in DTO's / POCO's without even knowing that we are using it. When designing software, it is essential to consider the possibility of future changes.",
                    Meta = "post-14",
                    UrlSlug = "open-closed-principle-in-solid",
                    Published = true,
                    PostedDate = new DateTime(2023, 3, 14),
                    ModifiedDate = null,
                    ViewCount = 100,
                    Author = authors[1],
                    Category = categories[7],
                    Tags = new List<Tag>()
                    {
                        tags[2],
                        tags[7],
                        tags[12],
                        tags[13],
                        tags[15],
                    }
                },
                new() {
                    Title = "UI/UX Design Trends 2023",
                    ShortDescription = "UI/UX Design Trends 2023",
                    Description = "UI/UX Design Trends 2023. Yet another year is coming to a close. We are taking a more careful look at both UI and UX trends that continue to evolve. We highlight some of the trends we think will persist and perhaps gain even more traction in the next year.",
                    Meta = "post-15",
                    UrlSlug = "ui-ux-design-trends-2023",
                    Published = true,
                    PostedDate = new DateTime(2023, 1, 20),
                    ModifiedDate = null,
                    ViewCount = 27,
                    Author = authors[4],
                    Category = categories[8],
                    Tags = new List<Tag>()
                    {
                        tags[1],
                        tags[2],
                        tags[15],
                        tags[11],
                        tags[12],
                    }
                },
                new() {
                    Title = "The Definitive TypeScript 5.0 Guide",
                    ShortDescription = "The Definitive TypeScript 5.0 Guide",
                    Description = "The Definitive TypeScript 5.0 Guide - SitePen was first published October 2018. TypeScript is a superset of JavaScript, but with optional types, interfaces, generics, and more. The Definitive Guide provides one of the best overviews of the key features of TypeScript.",
                    Meta = "post-16",
                    UrlSlug = "the-definitive-typescript-5-0-guide",
                    Published = true,
                    PostedDate = new DateTime(2022, 12, 31),
                    ModifiedDate = null,
                    ViewCount = 27,
                    Author = authors[2],
                    Category = categories[3],
                    Tags = new List<Tag>()
                    {
                        tags[10],
                        tags[11],
                        tags[12],
                        tags[13],
                        tags[14],
                    }
                },
                new() {
                    Title = "7 Tools for Faster Development in React",
                    ShortDescription = "7 Tools for Faster Development in React",
                    Description = "Gatsby allows developers to build websites that are fast, secure, and easy to maintain. NextJs NextJS is a tool for generating React applications and server code. Preact is not React. It is based on the same API as React and shares many of its features, such as components, state management, and a virtual DOM.",
                    Meta = "post-17",
                    UrlSlug = "7-tools-for-faster-development-in-react",
                    Published = true,
                    PostedDate = new DateTime(2022, 12, 26),
                    ModifiedDate = null,
                    ViewCount = 13,
                    Author = authors[4],
                    Category = categories[7],
                    Tags = new List<Tag>()
                    {
                        tags[1],
                        tags[11],
                        tags[12],
                        tags[13],
                        tags[14],
                    }
                },
                new() {
                    Title = "My VS Code setup",
                    ShortDescription = "My VS Code setup",
                    Description = "extension adds language support for C/C++ to Visual Studio Code, including editing (IntelliSense) and debugging features. Auto Rename Tag Automatically rename paired HTML/XML tag, same as Visual Studio IDE does. Error Lens ErrorLens turbo-charges language diagnostic features by making diagnostics stand out more prominently.",
                    Meta = "post-18",
                    UrlSlug = "my-vs-code-setup",
                    Published = true,
                    PostedDate = new DateTime(2023, 1, 26),
                    ModifiedDate = null,
                    ViewCount = 30,
                    Author = authors[2],
                    Category = categories[8],
                    Tags = new List<Tag>()
                    {
                        tags[12],
                        tags[13],
                        tags[15],
                        tags[17],
                        tags[18],
                    }
                },
                new() {
                    Title = "EP56: System Design Blueprint: The Ultimate Guide",
                    ShortDescription = "EP56: System Design Blueprint: The Ultimate Guide",
                    Description = "Amazon has created a unique build system, known as Brazil, to enhance productivity and empower Amazon’s micro-repo driven collaboration. McDonald's standardizes events using the following components: an event registry to define a standardized schema. An event gateway that performs identity authentication and authorization.",
                    Meta = "post-19",
                    UrlSlug = "ep56-system-design-blueprint-the-ultimate-guide",
                    Published = true,
                    PostedDate = new DateTime(2023, 4, 23),
                    ModifiedDate = null,
                    ViewCount = 75,
                    Author = authors[4],
                    Category = categories[2],
                    Tags = new List<Tag>()
                    {
                        tags[7],
                        tags[8],
                        tags[13],
                        tags[15],
                        tags[18],
                    }
                },
                new() {
                    Title = "SOLID Principles in JavaScript",
                    ShortDescription = "SOLID Principles in JavaScript",
                    Description = "SOLID principles are a set of software designs introduced by Robert C. Martin. These principles guide developers in building robust, maintainable applications while minimizing the cost of changes. In this article, we will discuss how to use these principles in JavaScript and demonstrate them with code examples.",
                    Meta = "post-20",
                    UrlSlug = "solid-principles-in-javascript",
                    Published = true,
                    PostedDate = new DateTime(2023, 4, 12),
                    ModifiedDate = null,
                    ViewCount = 36,
                    Author = authors[1],
                    Category = categories[6],
                    Tags = new List<Tag>()
                    {
                        tags[1],
                        tags[6],
                        tags[7],
                        tags[14],
                        tags[16],
                    }
                },
                new() {
                    Title = "Password, Session, Cookie, Token, JWT, SSO, OAuth - Authentication Explained - Part 1",
                    ShortDescription = "Password, Session, Cookie, Token, JWT, SSO, OAuth - Authentication Explained - Part 1",
                    Description = "Password, Session, Cookie, Token, JWT, SSO, OAuth - Authentication Explained - Part 1. We discuss the problems each method solves and how to choose the right authentication method for our needs. The diagram below shows where these methods apply in a typical website architecture and their meanings.",
                    Meta = "post-21",
                    UrlSlug = "password-session-cookie-token-jwt-sso-oauth-authentication-explained-part-1",
                    Published = true,
                    PostedDate = new DateTime(2023, 4, 18),
                    ModifiedDate = null,
                    ViewCount = 72,
                    Author = authors[3],
                    Category = categories[7],
                    Tags = new List<Tag>()
                    {
                        tags[12],
                        tags[16],
                        tags[17],
                        tags[18],
                        tags[19],
                    }
                },
                new() {
                    Title = "Advanced JavaScript Concepts To Write High-Quality Code That Scales",
                    ShortDescription = "Advanced JavaScript Concepts To Write High-Quality Code That Scales",
                    Description = "JavaScript is a powerful programming language extensively used for web development, server-side scripting, and more. While it has an easy learning curve for beginners, JavaScript is also used to build complex applications and systems that require many advanced programming concepts. The tutorial",
                    Meta = "post-22",
                    UrlSlug = "advanced-javascript-concepts-to-write-high-quality-code-that-scales",
                    Published = true,
                    PostedDate = new DateTime(2023, 4, 23),
                    ModifiedDate = null,
                    ViewCount = 16,
                    Author = authors[4],
                    Category = categories[8],
                    Tags = new List<Tag>()
                    {
                        tags[2],
                        tags[6],
                        tags[7],
                        tags[8],
                        tags[9],
                    }
                },
                new() {
                    Title = "JavaScript One-Liners to Use in Every Project",
                    ShortDescription = "JavaScript One-Liners to Use in Every Project",
                    Description = "JavaScript is a powerful language that can do a lot with very little code. In some cases, the amount of code you need to write doesn't exceed more than a single line. Let's go through 10 essential one liners worth using in virtually every project you create.",
                    Meta = "post-23",
                    UrlSlug = "javascript-one-liners-to-use-in-every-project",
                    Published = true,
                    PostedDate = new DateTime(2023, 1, 30),
                    ModifiedDate = null,
                    ViewCount = 29,
                    Author = authors[2],
                    Category = categories[1],
                    Tags = new List<Tag>()
                    {
                        tags[12],
                        tags[16],
                        tags[17],
                    }
                },
                new() {
                    Title = "Tools For Remote Developers",
                    ShortDescription = "Tools For Remote Developers",
                    Description = "At the top of our list of tools for work at home developers are project management tools that offer several features in one place. For instance, the right project management tool could have collaborative features like chat and messaging. Floobits is an open-source tool that allows remote developers",
                    Meta = "post-24",
                    UrlSlug = "tools-for-remote-developers",
                    Published = true,
                    PostedDate = new DateTime(2023, 2, 16),
                    ModifiedDate = null,
                    ViewCount = 31,
                    Author = authors[4],
                    Category = categories[5],
                    Tags = new List<Tag>()
                    {
                        tags[2],
                        tags[5],
                        tags[16],
                        tags[17],
                    }
                },
                new() {
                    Title = "Angular 16 is huge",
                    ShortDescription = "Angular 16 is huge",
                    Description = "angular 16 is just the first release candidate version of v16. There are a lot of features/changes coming with this version. It is still experimental and some more performance related improvements and optimisations are to be made. It already enables a much faster startup time (at least 2 times) than webpack implementation.",
                    Meta = "post-25",
                    UrlSlug = "angular-16-is-huge",
                    Published = true,
                    PostedDate = new DateTime(2023, 1, 31),
                    ModifiedDate = null,
                    ViewCount = 65,
                    Author = authors[3],
                    Category = categories[2],
                    Tags = new List<Tag>()
                    {
                        tags[2],
                        tags[5],
                        tags[7],
                        tags[16],
                        tags[17],
                    }
                },
                new() {
                    Title = "You can now try Microsoft Loop, a Notion competitor with futuristic Office documents",
                    ShortDescription = "You can now try Microsoft Loop, a Notion competitor with futuristic Office documents",
                    Description = "Microsoft is now letting anyone preview Microsoft Loop, a collaborative hub offering a new way of working across Office apps and managing tasks and projects. The main interface looks a lot like Notion, a workspace app that is used by Adobe, Figma, Amazon, and many other businesses. Loop pages are",
                    Meta = "post-26",
                    UrlSlug = "you-can-now-try-microsoft-loop-a-notion-competitor-with-futuristic-office-documents",
                    Published = true,
                    PostedDate = new DateTime(2023, 1, 15),
                    ModifiedDate = null,
                    ViewCount = 52,
                    Author = authors[2],
                    Category = categories[2],
                    Tags = new List<Tag>()
                    {
                        tags[1],
                        tags[5],
                        tags[13],
                        tags[16],
                        tags[17],
                    }
                },
                new() {
                    Title = "10 Trending Node.js Libraries and Frameworks to Boost Your Web Development",
                    ShortDescription = "10 Trending Node.js Libraries and Frameworks to Boost Your Web Development",
                    Description = "Node.js libraries and frameworks can help you streamline your development process and improve the performance of your applications. We have compiled a list of the top 10 trending Node.JS libraries and Frameworks to boost your development experience. Here's a sample code snippet to create a simple HTTP server using Express.js.",
                    Meta = "post-27",
                    UrlSlug = "10-trending-node-js-libraries-and-frameworks-to-boost-your-web-development",
                    Published = true,
                    PostedDate = new DateTime(2023, 1, 1),
                    ModifiedDate = null,
                    ViewCount = 30,
                    Author = authors[4],
                    Category = categories[7],
                    Tags = new List<Tag>()
                    {
                        tags[5],
                        tags[11],
                        tags[13],
                        tags[16],
                        tags[17],
                    }
                },
                new() {
                    Title = "Dew Drop – April 24, 2023 (#3928) – Morning Dew by Alvin Ashcraft",
                    ShortDescription = "Dew Drop – April 24, 2023 (#3928) – Morning Dew by Alvin Ashcraft",
                    Description = "How to create a mobile app with.NET MAUI in 10 easy steps (Luis Matos) Building an Apple I from Parts with SmartyKit.io (Scott Hanselman) Deep dive – Uno Platform Template Wizard – Projects, Testing and Features (Nick Randolph) Improving the Syntax Highlighting of JavaScript in Visual Studio.",
                    Meta = "post-28",
                    UrlSlug = "dew-drop-april-24-2023-3928-morning-dew-by-alvin-ashcraft",
                    Published = true,
                    PostedDate = new DateTime(2023, 3, 1),
                    ModifiedDate = null,
                    ViewCount = 37,
                    Author = authors[2],
                    Category = categories[8],
                    Tags = new List<Tag>()
                    {
                        tags[5],
                        tags[11],
                        tags[16],
                        tags[17],
                    }
                },
                new() {
                    Title = "SEGA workers are forming a union",
                    ShortDescription = "SEGA workers are forming a union",
                    Description = "A group of 144 workers are forming a union at SEGA’s American headquarters in Irvine, California. SEGA follows in the footsteps of other gaming companies like Microsoft-owned ZeniMax and Activision Blizzard, which both unionized last year. If SEGA does not recognize the union, the eligible workers can conduct an election.",
                    Meta = "post-29",
                    UrlSlug = "sega-workers-are-forming-a-union",
                    Published = true,
                    PostedDate = new DateTime(2023, 1, 12),
                    ModifiedDate = null,
                    ViewCount = 64,
                    Author = authors[4],
                    Category = categories[1],
                    Tags = new List<Tag>()
                    {
                        tags[1],
                        tags[3],
                        tags[5],
                        tags[17],
                    }
                },
                new() {
                    Title = "API Gateway Pattern: Features and the AWS Implementation",
                    ShortDescription = "API Gateway Pattern: Features and the AWS Implementation",
                    Description = "API Gateway Pattern is an architectural pattern to create a façade that exposes the internal system’s data to external clients. This pattern is widely spread in the computing field being used in business applications and integration solutions for small, medium, and large enterprises. The value of",
                    Meta = "post-30",
                    UrlSlug = "api-gateway-pattern-features-and-the-aws-implementation",
                    Published = true,
                    PostedDate = new DateTime(2023, 4, 12),
                    ModifiedDate = null,
                    ViewCount = 42,
                    Author = authors[1],
                    Category = categories[1],
                    Tags = new List<Tag>()
                    {
                        tags[1],
                        tags[13],
                        tags[15],
                        tags[17],
                    }
                }
            };

            _dbContext.Posts.AddRange(posts);
            _dbContext.SaveChanges();

            return posts;
        }
    }
}
