using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WpfApp1.Models;

namespace WpfApp1.Data
{
    public class PayBillDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Paysheet> Paysheets { get; set; }
        public DbSet<Login> Logins { get; set; }

        public PayBillDbContext()
        {
        }

        public PayBillDbContext(DbContextOptions<PayBillDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

                var configuration = builder.Build();
                var connectionString = configuration.GetConnectionString("DefaultConnection");

                if (!string.IsNullOrEmpty(connectionString))
                {
                    optionsBuilder.UseMySql(connectionString,
                        new MySqlServerVersion(new Version(8, 0, 21)));
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Employees
            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.EmployeeNumber)
                .IsUnique();

            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.NicNo)
                .IsUnique();

            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.AccountNumber)
                .IsUnique();

            // Schedules
            modelBuilder.Entity<Schedule>()
                .HasIndex(s => s.Name)
                .IsUnique();

            // Logins
            modelBuilder.Entity<Login>()
                .HasIndex(l => l.Username)
                .IsUnique();

            // Seed Admin User
            modelBuilder.Entity<Login>().HasData(
                new Login { Id = 1, Username = "Admin", Password = "123456", Role = "admin" }
            );

            // Payments Foreign Keys (already defined via attributes, but reinforcing is good or just rely on attributes)
            // Attributes are usually enough for simple FKs.
        }
    }
}
