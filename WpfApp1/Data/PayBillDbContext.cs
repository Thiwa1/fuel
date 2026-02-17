using System;
using Microsoft.EntityFrameworkCore;
using WpfApp1.Models;

namespace WpfApp1.Data
{
    public class PayBillDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Paysheet> Paysheets { get; set; }

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
                // Placeholder connection string. Update with actual credentials.
                optionsBuilder.UseMySql("server=localhost;database=pay_bill;user=root;password=password",
                    new MySqlServerVersion(new Version(8, 0, 21)));
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

            // Payments Foreign Keys (already defined via attributes, but reinforcing is good or just rely on attributes)
            // Attributes are usually enough for simple FKs.
        }
    }
}
