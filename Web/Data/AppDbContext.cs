using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Web.Models;

namespace Web.Data
{
    public class AppDbContext:IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext>options):base(options) 
        {
            
        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleTag> ArticleTags { get; set; }
        public DbSet<Tag> Tags { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");

        }
    }
}
