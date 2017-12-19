using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DocumentDbSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var db = new BloggingContext())
            {
                // Recreate database
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                Console.WriteLine(((Model)db.Model).ToDebugString());

                var blogId = 1;
                var postId = 1;

                // Seed database
                db.Blogs.AddRange(
                    new Blog
                    {
                        BlogId = blogId++,
                        Url = "http://blogs.msdn.com/adonet",
                        Posts = new List<Post>
                        {
                            new Post{ PostId = postId++, Title = "One", Content = "Random"},
                            new Post{ PostId = postId++, Title = "Two", Content = "Random"}
                        }
                    },
                    new Blog
                    {
                        BlogId = blogId++,
                        Url = "http://blogs.msdn.com/adonet",
                        Posts = new List<Post>
                        {
                            new Post{ PostId = postId++, Title = "One", Content = "Random"},
                            new Post{ PostId = postId++, Title = "Two", Content = "Random"}
                        }
                    },
                    new Blog
                    {
                        BlogId = blogId++,
                        Url = "http://blogs.msdn.com/adonet",
                        Posts = new List<Post>
                        {
                            new Post{ PostId = postId++, Title = "One", Content = "Random"},
                            new Post{ PostId = postId++, Title = "Two", Content = "Random"}
                        }
                    },
                    new SpecialBlog
                    {
                        BlogId = blogId++,
                        Url = "SpecialBlog"
                    });
                var count = db.SaveChanges();
                Console.WriteLine("{0} records saved to database", count);
                Console.WriteLine();
            }

            using (var db = new BloggingContext())
            {
                // Run queries
                Console.WriteLine("All blogs in database:");
                foreach (var blog in db.Blogs.Include(e => e.Posts))
                {
                    Console.WriteLine(" - {0}", blog.Url);
                    Console.WriteLine(blog.GetType());
                    Console.WriteLine(" - Posts", blog.Url);
                    foreach (var post in blog.Posts)
                    {
                        Console.WriteLine("   - {0}", post.Title);
                    }
                }
            }

            using (var db = new BloggingContext())
            {
                var secondBlog = db.Blogs.Include(e => e.Posts).FirstOrDefault(e => e.BlogId == 2);
                secondBlog.Url = "CustomUrl";
                secondBlog.Posts.Clear();
                secondBlog.Posts.Add(new Post { PostId = 7, Title = "Modified" });
                var count = db.SaveChanges();
                Console.WriteLine("{0} records saved to database", count);
                Console.WriteLine();
            }

            Console.WriteLine("Program finished.");
        }
    }


    public class BloggingContext : DbContext
    {
        private static ILoggerFactory LoggerFactory => new LoggerFactory().AddConsole(LogLevel.Trace);

        // Declare DBSets
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<SpecialBlog> SpecialBlogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Select 1 provider
            optionsBuilder
                .UseDocumentDb("SampleApp")
                .EnableSensitiveDataLogging()
                .UseLoggerFactory(LoggerFactory);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure model
        }
    }

    public class Blog
    {
        public int BlogId { get; set; }
        public string Url { get; set; }

        public List<Post> Posts { get; set; }
    }

    public class SpecialBlog : Blog
    {
        public string Special { get; set; }
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}
