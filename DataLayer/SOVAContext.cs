using DataLayer.Model;
using Microsoft.EntityFrameworkCore;


namespace DataLayer
{

    public class SOVAContext: DbContext
    {


        public DbSet<Annotations> Annotations { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Mark> Marked { get; set; }
        public DbSet<PostTag> PostTags { get; set; }
        public DbSet<PostLink> PostLinks { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<SearchHistories> SearchHistory{ get; set; }

        public DbQuery<SearchResult> SearchResults { get; set; }
        public DbQuery<WordCloud> WordClouds { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseNpgsql("host=localhost;db=stackoverflow;uid=postgres;pwd=postgres");


        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Map Class property: Post. 
           
            modelBuilder.Entity<Post>().ToTable("posts");
            modelBuilder.Entity<Post>().HasKey(x => x.Id);
            modelBuilder.Entity<Post>().Property(x => x.Id).HasColumnName("id");
            modelBuilder.Entity<Post>().Property(x => x.PostType).HasColumnName("posttype");
            modelBuilder.Entity<Post>().Property(x => x.AcceptedAnswerId).HasColumnName("acceptedanswerid");
            modelBuilder.Entity<Post>().Property(x => x.Score).HasColumnName("score");
            modelBuilder.Entity<Post>().Property(x => x.CreationDate).HasColumnName("creationdate");
            modelBuilder.Entity<Post>().Property(x => x.Body).HasColumnName("body");
            modelBuilder.Entity<Post>().Property(x => x.ClosedDate).HasColumnName("closeddate");
            modelBuilder.Entity<Post>().Property(x => x.AuthorId).HasColumnName("authorid");
            modelBuilder.Entity<Post>().HasDiscriminator(x => x.PostType)
                .HasValue<Question>(1)
                .HasValue<Answer>(2);

           
            modelBuilder.Entity<Question>().Property(x => x.Title).HasColumnName("title");
            modelBuilder.Entity<Answer>().Property(x => x.ParentId).HasColumnName("parentid");

            //search result mapping
            modelBuilder.Query<SearchResult>().Property(x => x.Id).HasColumnName("id");
            modelBuilder.Query<SearchResult>().Property(x => x.Body).HasColumnName("body");
            modelBuilder.Query<SearchResult>().Property(x => x.Title).HasColumnName("title");


            //word cloud mapping
            modelBuilder.Query<WordCloud>().Property(x => x.Word).HasColumnName("word");
           

            //Map Class Propert: Author

            modelBuilder.Entity<Author>().ToTable("authors");
            modelBuilder.Entity<Author>().Property(x => x.AuthorId).HasColumnName("id");
            modelBuilder.Entity<Author>().Property(x => x.DisplayName).HasColumnName("displayname");
            modelBuilder.Entity<Author>().Property(x => x.CreationDate).HasColumnName("creationdate");
            modelBuilder.Entity<Author>().Property(x => x.Location).HasColumnName("location");
            modelBuilder.Entity<Author>().Property(x => x.Age).HasColumnName("age");

            //Map Class Propert: Annotations
           
            modelBuilder.Entity<Annotations>().ToTable("annotations");
            modelBuilder.Entity<Annotations>().Property(x => x.Id).HasColumnName("id");
            modelBuilder.Entity<Annotations>().HasKey(x => x.Id);
            modelBuilder.Entity<Annotations>().Property(x => x.Body).HasColumnName("body");
            modelBuilder.Entity<Annotations>().Property(x => x.CreationDate).HasColumnName("creationdate");
            modelBuilder.Entity<Annotations>().Property(x => x.PostId).HasColumnName("postid");
            modelBuilder.Entity<Annotations>().Property(x => x.UserId).HasColumnName("userid");

            //Map Class Propert: Mark

            modelBuilder.Entity<Mark>().ToTable("marked");
            modelBuilder.Entity<Mark>().HasKey(x =>new { x.UserId,x.PostId });
            modelBuilder.Entity<Mark>().Property(x => x.CreationDate).HasColumnName("creationdate");
            modelBuilder.Entity<Mark>().Property(x => x.PostId).HasColumnName("postid");
            modelBuilder.Entity<Mark>().Property(x => x.UserId).HasColumnName("userid");


            //Map Class Propert: Comment 
           
            modelBuilder.Entity<Comment>().ToTable("comments");
            modelBuilder.Entity<Comment>().Property(x => x.Id).HasColumnName("id");
            modelBuilder.Entity<Comment>().Property(x => x.Score).HasColumnName("score");
            modelBuilder.Entity<Comment>().Property(x => x.CreationDate).HasColumnName("creationdate");
            modelBuilder.Entity<Comment>().Property(x => x.Body).HasColumnName("body");
            modelBuilder.Entity<Comment>().Property(x => x.PostId).HasColumnName("parent");
            modelBuilder.Entity<Comment>().Property(x => x.AuthorId).HasColumnName("authorid");
            modelBuilder.Entity<Comment>()
                .HasOne<Question>(c => c.Question)
                .WithMany(q => q.Comments)
                .HasForeignKey(c => c.PostId);
            modelBuilder.Entity<Comment>()
                .HasOne<Answer>(c => c.Answer)
                .WithMany(q => q.Comments)
                .HasForeignKey(c => c.PostId);

           
            modelBuilder.Entity<PostTag>().ToTable("posttags");
            modelBuilder.Entity<PostTag>().Property(x => x.PostTagId).HasColumnName("id");
            modelBuilder.Entity<PostTag>().Property(x => x.Tag).HasColumnName("tag");

            //Map Class Property: PostLinks
            modelBuilder.Entity<PostLink>().ToTable("postlinks");
            modelBuilder.Entity<PostLink>().HasKey(x => new { x.Id, x.PostLinkId });
            modelBuilder.Entity<PostLink>().Property(x => x.Id).HasColumnName("id");
            modelBuilder.Entity<PostLink>().Property(x => x.PostLinkId).HasColumnName("postlinkid");
            modelBuilder.Entity<PostLink>()
               .HasOne<Question>(c => c.Question)
               .WithMany(q => q.PostLinks)
               .HasForeignKey(c => c.Id);
            modelBuilder.Entity<PostLink>()
               .HasOne<Answer>(c => c.Answer)
               .WithMany(q => q.PostLinks)
               .HasForeignKey(c => c.Id);

            //Map class property for users

            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<User>().Property(x => x.Id).HasColumnName("id");
            modelBuilder.Entity<User>().Property(x => x.UserName).HasColumnName("username");
            modelBuilder.Entity<User>().Property(x => x.CreationDate).HasColumnName("creationdate");
            modelBuilder.Entity<User>().Property(x => x.Password).HasColumnName("password");

            

            modelBuilder.Entity<SearchHistories>().ToTable("searchhistory");
            modelBuilder.Entity<SearchHistories>().HasKey(x => new { x.Search, x.UserId, x.Date });
            modelBuilder.Entity<SearchHistories>().Property(x => x.Search).HasColumnName("search");
            modelBuilder.Entity<SearchHistories>().Property(x => x.UserId).HasColumnName("userid");
            modelBuilder.Entity<SearchHistories>().Property(x => x.Date).HasColumnName("creationdate");

        }

    }
    

}
