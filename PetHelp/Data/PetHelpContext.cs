using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PetHelp.Models;

namespace PetHelp.Data
{
    public class PetHelpContext : DbContext
    {
        public PetHelpContext (DbContextOptions<PetHelpContext> options)
            : base(options)
        {
        }

        public DbSet<PetHelp.Models.User> User { get; set; } = default!;
        public DbSet<PetHelp.Models.Pet> Pet { get; set; } = default!;
        public DbSet<PetHelp.Models.Ad> Ad { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Ads)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Pets)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<Ad>()
                .HasOne(a => a.Pet)
                .WithOne(p => p.Ad)
                .HasForeignKey<Pet>(p => p.AdId);
        }
    }
}
