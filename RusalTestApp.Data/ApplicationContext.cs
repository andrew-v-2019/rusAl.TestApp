using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RusAlTestApp.Data.Models;

namespace RusAlTestApp.Data
{
    public sealed class ApplicationContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<Drink> Drinks { get; set; }
        public DbSet<Color> Colors { get; set; }


        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RegistrationDrink>()
                .HasKey(t => new {t.DrinkId, t.RegistrationId});


            modelBuilder.Entity<RegistrationDrink>()
                .HasOne(sc => sc.Registration)
                .WithMany(s => s.RegistrationDrinks)
                .HasForeignKey(sc => sc.RegistrationId);


            modelBuilder.Entity<IdentityUser>().HasData(
                CreateDefaultUser()
            );

            modelBuilder.Entity<Drink>().HasData(
                CreateDrinks()
            );

            modelBuilder.Entity<Color>().HasData(
                CreateColors()
            );
        }

        private static IdentityUser CreateDefaultUser()
        {
            return new IdentityUser
            {
                UserName = "admin",
                PasswordHash = EncodePassword("admin"),
                Email = "admin",
                EmailConfirmed = true,
                LockoutEnabled = false,
                TwoFactorEnabled = false,
                Id = Guid.NewGuid().ToString(),
            };
        }

        private static IEnumerable<Color> CreateColors()
        {
            var colors = new[] {"Синий", "Желтый", "Красный"};
            var result = new List<Color>();
            for (var i = 0; i < colors.Length; i++)
            {
                result.Add(new Color()
                {
                    ColorId = i + 1,
                    Name = colors[i]
                });
            }

            return result;
        }

        private static IEnumerable<Drink> CreateDrinks()
        {
            var colors = new[] {"Чай", "Кофе", "Сок", "Вода"};
            var result = new List<Drink>();
            for (var i = 0; i < colors.Length; i++)
            {
                result.Add(new Drink
                {
                    DrinkId = i + 1,
                    Name = colors[i]
                });
            }

            return result;
        }

        public static string EncodePassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            using (var bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }

            var dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }
    }


}
