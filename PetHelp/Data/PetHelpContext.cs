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

        public DbSet<PetHelp.Models.Owner> Owners { get; set; } = default!;
        public DbSet<PetHelp.Models.Applicant> Applicants { get; set; } = default!;
        public DbSet<PetHelp.Models.Pet> Pets { get; set; } = default!;
        public DbSet<PetHelp.Models.Ad> Ads { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Owner>()
                .HasMany(u => u.Ads)
                .WithOne(a => a.Owner)
                .HasForeignKey(a => a.OwnerId);

            modelBuilder.Entity<Owner>()
                .HasMany(u => u.Pets)
                .WithOne(p => p.Owner)
                .HasForeignKey(p => p.OwnerId);

            modelBuilder.Entity<Ad>()
                .HasOne(a => a.Pet)
                .WithOne(p => p.Ad)
                .HasForeignKey<Pet>(p => p.AdId);

            modelBuilder.Entity<Applicant>()
                .HasMany(u => u.Ads)
                .WithMany(a => a.Applicants);
        }
    }
}
