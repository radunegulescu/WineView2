﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WineView2.Models;

namespace WineView2.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Wine> Wines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Color>().HasData(
                new Color { Id = 1, Name = "Red"},
                new Color { Id = 2, Name = "Rose"},
                new Color { Id = 3, Name = "White"}
                );
            modelBuilder.Entity<Wine>().HasData(
                new Wine
                {
                    Id = 1,
                    Name = "Fortune of Time",
                    Price = 90,
                    Price5 = 85,
                    Price10 = 80,
                    ColorId = 1,
                    ImageUrl = ""
                },
                new Wine
                {
                    Id = 2,
                    Name = "Dark Skies",
                    Price = 30,
                    Price5 = 25,
                    Price10 = 20,
                    ColorId = 1,
                    ImageUrl = ""
                },
                new Wine
                {
                    Id = 3,
                    Name = "Vanish in the Sunset",
                    Price = 50,
                    Price5 = 40,
                    Price10 = 35,
                    ColorId = 1,
                    ImageUrl = ""
                },
                new Wine
                {
                    Id = 4,
                    Name = "Cotton Candy",
                    Price = 65,
                    Price5 = 60,
                    Price10 = 55,
                    ColorId = 2,
                    ImageUrl = ""
                },
                new Wine
                {
                    Id = 5,
                    Name = "Rock in the Ocean",
                    Price = 27,
                    Price5 = 25,
                    Price10 = 20,
                    ColorId = 2,
                    ImageUrl = ""
                },
                new Wine
                {
                    Id = 6,
                    Name = "Leaves and Wonders",
                    Price = 23,
                    Price5 = 22,
                    Price10 = 20,
                    ColorId = 3,
                    ImageUrl=""
                }
                );
        }
    }
}
