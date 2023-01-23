
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using realworldProject.Models;



namespace realworldProject.DBModel
{
    public class MyDatabase : DbContext
    {
        

        public DbSet<UserModel> Users { get; set; }
        public DbSet<FollowingModel> Following { get; set; }
        public DbSet<ArticlesModel> Articles { get; set; }
        public DbSet<ArticlesTags> ArticleTags { get; set; }
        public DbSet<ArticlesFavoriets> Favoriets { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>().ToTable("Users").HasKey(u => u.Username);
            modelBuilder.Entity<FollowingModel>().ToTable("UserFollowing").HasKey(f => new {
                f.Username,
                f.FollowingName
            });
            modelBuilder.Entity<ArticlesModel>().ToTable("Articles").HasKey(a => a.Slug);
            modelBuilder.Entity<ArticlesTags>().ToTable("ArticlesTags").HasKey(t=>new
            {
                t.Slug,
                t.Tagname
            });
            modelBuilder.Entity<ArticlesFavoriets>().ToTable("ArticlesFavoriets").HasKey(f => new
            {
                f.Username,
                f.Slug
            });

            /*modelBuilder.Entity<UserModel>().ToTable("Users").HasMany(u=>u.Following).WithOne(f=>f.Users).HasForeignKey(f=>f.UserId);
            modelBuilder.Entity<UserModel>().HasKey(u => u.Id);
            modelBuilder.Entity<UserModel>().HasMany(u => u.MyArticles).WithOne(a => a.Author).HasForeignKey(a=>a.AuthorId);
            modelBuilder.Entity<UserModel>().Property(u=>u.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<FollowingModel>().Property(u=>u.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Tags>().Property(u => u.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<ArticlesModel>().ToTable("Articles").HasMany(t => t.TagList).WithMany(a => a.articles).UsingEntity<ArticlesTags>().ToTable("ArticlesTags");
            modelBuilder.Entity<ArticlesModel>().HasMany(f=>f.FollowedBy).WithMany(u=>u.FavorietArticles).UsingEntity<ArticlesFavoriets>();
            modelBuilder.Entity<ArticlesModel>().HasKey(a => a.Id);
            modelBuilder.Entity<ArticlesModel>().Property(u => u.Id).ValueGeneratedOnAdd();*/

        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("Data Source=mydb.db");
    }




}

