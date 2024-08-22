using AgentRest.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AgentRest.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<AgentModel> Agents { get; set; }
        public DbSet<TargetModel> Targets { get; set; }
        public DbSet<MissionModel> Missions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AgentModel>()
                .Property(a => a.Status)
                .HasConversion<string>()
                .IsRequired();

            modelBuilder.Entity<TargetModel>()
                .Property(a => a.Status)
                .HasConversion<string>()
                .IsRequired();


            modelBuilder.Entity<MissionModel>()
                .HasOne(m => m.Agent)
                .WithMany(a => a.Missions)
                .HasForeignKey(m => m.AgentId)
                .OnDelete(DeleteBehavior.Restrict); // מחיקה מקושרת

            modelBuilder.Entity<MissionModel>()
                .HasOne(m => m.Target)
                .WithMany()
                .HasForeignKey(m => m.TargetId)
                .OnDelete(DeleteBehavior.Restrict); // מחיקה מקושרת


            base.OnModelCreating(modelBuilder);
        }

    }
}
