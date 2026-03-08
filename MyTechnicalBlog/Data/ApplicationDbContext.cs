using Microsoft.EntityFrameworkCore;
using MyTechnicalBlog.Models;

namespace MyTechnicalBlog.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // Теперь контроллер TagsController найдет свой DbSet<Tag> Tags
        public DbSet<User> Users { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Настройка связей (теперь используем английские свойства)
            modelBuilder.Entity<Article>().HasMany(s => s.Tags).WithMany(t => t.Articles);
            modelBuilder.Entity<User>().HasMany(p => p.Roles).WithMany(r => r.Users);

            // 2. Добавляем Роли
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Администратор" },
                new Role { Id = 2, Name = "Модератор" },
                new Role { Id = 3, Name = "Пользователь" }
            );

            // 3. Добавляем Пользователей
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Username = "AdminUser" },
                new User { Id = 2, Username = "ModeratorUser" },
                new User { Id = 3, Username = "RegularUser" }
            );

            // 4. Связываем пользователей с ролями
            // EF Core использует английские названия для теневой таблицы: RoleUser, UsersId, RolesId
            modelBuilder.Entity("RoleUser").HasData(
                new { UsersId = 1, RolesId = 1 }, // AdminUser -> Администратор
                new { UsersId = 2, RolesId = 2 }, // ModeratorUser -> Модератор
                new { UsersId = 3, RolesId = 3 }  // RegularUser -> Пользователь
            );
        }
    }
}