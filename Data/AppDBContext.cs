using System;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) {
            
        }

        public DbSet<ToDoTask> ToDoItems { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                
                entity.HasIndex(u => u.Email).IsUnique(); 
                
                entity.Property(u => u.Email).IsRequired().HasMaxLength(150);
                entity.Property(u => u.PasswordHash).IsRequired();
            });

            modelBuilder.Entity<ToDoTask>(entity =>
            {
                entity.HasKey(t => t.Id);
                
                entity.Property(t => t.Title).IsRequired().HasMaxLength(200);
                entity.Property(t => t.Description).HasMaxLength(1000);
                
                entity.HasOne(t => t.User)
                    .WithMany(u => u.Tasks)
                    .HasForeignKey(t => t.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(t => t.Category)
                    .WithMany(c => c.Tasks)
                    .HasForeignKey(t => t.CategoryId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Name).IsRequired().HasMaxLength(50);
                
                entity.HasOne(c => c.User)
                    .WithMany(u => u.Categories)
                    .HasForeignKey(c => c.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}