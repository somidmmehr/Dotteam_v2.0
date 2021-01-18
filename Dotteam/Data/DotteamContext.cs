using Microsoft.EntityFrameworkCore;
using Dotteam.Models;

namespace Dotteam.Data
{
    public class DotteamContext : DbContext
    {
        public DotteamContext (DbContextOptions<DotteamContext> options)
            : base(options)
        {
        }

        public DbSet<Models.ProjectModel> ProjectModel { get; set; }

        public DbSet<Dotteam.Models.IssueModel> IssueModel { get; set; }

        public DbSet<Dotteam.Models.CommentModel> CommentModel { get; set; }

        public DbSet<Dotteam.Models.PresentaionModel> PresentaionModel { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CommentModel>(entity =>
            {
                entity
                    .HasMany(e => e.Replies)
                    .WithOne(e => e.ParentComment) //Each comment from Replies points back to its parent
                    .HasForeignKey(e => e.ReplyToCommentId);
            });

        }

        public DbSet<Dotteam.Models.ProjectTechModel> ProjectTechModel { get; set; }
    }
}
