using Microsoft.EntityFrameworkCore;
using PetHelp.Domain.DomainModels;
using PetHelp.Models;

namespace PetHelp.Data
{
    public class PetHelpContext(DbContextOptions<PetHelpContext> options) : DbContext(options)
    {
        public DbSet<Owner> Owners { get; set; } = default!;
        public DbSet<Applicant> Applicants { get; set; } = default!;
        public DbSet<Pet> Pets { get; set; } = default!;
        public DbSet<Ad> Ads { get; set; } = default!;
        public DbSet<Admin> Admins { get; set; } = default!;

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

            modelBuilder.Entity<Pet>()
                .Property(p => p.Sex)
                .HasConversion<string>();
        }
    }
}
