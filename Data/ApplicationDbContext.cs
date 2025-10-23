using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Models;
using TaskManagerApi.Models.Enums;
using TaskStatusEnum = TaskManagerApi.Models.Enums.TaskStatus;

namespace TaskManagerApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Models.Task> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();
            });

            modelBuilder.Entity<Models.Task>(entity =>
            {
                entity.HasOne(t => t.User)
                      .WithMany(u => u.Tasks)
                      .HasForeignKey(t => t.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(t => t.Status)
                      .HasDefaultValue(TaskStatusEnum.ToDo);

                entity.Property(t => t.CreatedAt).HasDefaultValueSql("GETDATE()");
            });
        }
    }
}