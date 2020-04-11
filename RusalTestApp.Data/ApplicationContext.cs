using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
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

        public DbSet<RegistrationDrink> RegistrationDrinks { get; set; }
        public DbSet<RegistrationColor> RegistrationColors { get; set; }


        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RegistrationDrink>()
                .HasKey(t => new { t.DrinkId, t.RegistrationId });


            modelBuilder.Entity<RegistrationDrink>()
                .HasOne(sc => sc.Registration)
                .WithMany(s => s.RegistrationDrinks)
                .HasForeignKey(sc => sc.RegistrationId);

            modelBuilder.Entity<RegistrationColor>()
                .HasKey(t => new { t.ColorId, t.RegistrationId });


            modelBuilder.Entity<RegistrationColor>()
                .HasOne(sc => sc.Registration)
                .WithMany(s => s.RegistrationColors)
                .HasForeignKey(sc => sc.RegistrationId);


            modelBuilder.Entity<Drink>().HasData(
                CreateDrinks()
            );

            modelBuilder.Entity<Color>().HasData(
                CreateColors()
            );
        }

        private static IEnumerable<Color> CreateColors()
        {
            var colors = new[] { "Синий", "Желтый", "Красный" };
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
            var colors = new[] { "Чай", "Кофе", "Сок", "Вода" };
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
    }
}
