using API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace API.Context
{
    public class MyContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        public MyContext(DbContextOptions<MyContext> options) : base(options) 
        { 
        

        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Profiling> Profilings { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<University> Universities { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<AccountRole> AccountRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasOne(a => a.Account)
                .WithOne(b => b.Employee)
                .HasForeignKey<Account>(b => b.NIK);
            modelBuilder.Entity<Account>()
                .HasOne(a => a.Profiling)
                .WithOne(b => b.Account)
                .HasForeignKey<Profiling>(b => b.NIK);
            modelBuilder.Entity<Education>()
                .HasMany(a => a.Profilings)
                .WithOne(b => b.Education);
            modelBuilder.Entity<University>()
                .HasMany(a => a.Educations)
                .WithOne(b => b.University);



            modelBuilder.Entity<AccountRole>()
                .HasKey(bc => new { bc.NIK, bc.IdRole });
            modelBuilder.Entity<AccountRole>()
                .HasOne(bc => bc.Account)
                .WithMany(b => b.AccountRoles)
                .HasForeignKey(bc => bc.NIK);
            modelBuilder.Entity<AccountRole>()
                .HasOne(bc => bc.Role)
                .WithMany(c => c.AccountRoles)
                .HasForeignKey(bc => bc.IdRole);
        }
    }
}
