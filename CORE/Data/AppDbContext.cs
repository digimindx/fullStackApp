using CORE.Entities.Financial;
using CORE.Entities.HR;
using Microsoft.EntityFrameworkCore;

namespace CORE.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    #region HR

    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<EmployeeAccount> EmployeeAccounts => Set<EmployeeAccount>();
    public DbSet<EmployeeAddress> EmployeeAddresses => Set<EmployeeAddress>();
    public DbSet<EmployeeBankAccount> EmployeeBankAccounts => Set<EmployeeBankAccount>();

    #endregion

    #region Financial

    public DbSet<FinancialAccount> FinancialAccounts => Set<FinancialAccount>();

    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ─── HR Schema ───────────────────────────────────────────────

        modelBuilder.HasDefaultSchema("HR");

        // Employee
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("Employees");

            entity.HasKey(e => e.EmployeeID);

            entity.HasIndex(e => e.EmployeeNumber).IsUnique();

            entity.Property(e => e.EmployeeNumber).IsRequired();

            entity.Property(e => e.FullName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.MotherName).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(1);
            entity.Property(e => e.DateOfBirth).HasColumnType("date");
            entity.Property(e => e.BirthPlace).HasMaxLength(100);
            entity.Property(e => e.NationalityCode).IsRequired(false);
            entity.Property(e => e.MaritalStatus).HasMaxLength(1);
            entity.Property(e => e.SocialCategory).HasMaxLength(1);
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).IsRequired().HasMaxLength(128);
            entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.LastModifiedBy).HasMaxLength(128);



            // Gender constraint
            entity.ToTable(t => t.HasCheckConstraint("CK_Gender", "Gender IN ('F', 'M')"));

            // Marital status constraint
            entity.ToTable(t => t.HasCheckConstraint("CK_MaritalStatus", "MaritalStatus IN ('S', 'M', 'D', 'W')"));
        });

        // EmployeeAccount
        modelBuilder.Entity<EmployeeAccount>(entity =>
        {
            entity.ToTable("EmployeeAccounts");

            entity.HasKey(e => e.AccountID);

            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasIndex(e => e.Username).IsUnique();

            entity.Property(e => e.EmployeeID).IsRequired();
            entity.Property(e => e.Gender).HasMaxLength(1);
            entity.Property(e => e.DateOfBirth).HasColumnType("date");
            entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
            entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(1000);
            entity.Property(e => e.PasswordSalt).IsRequired().HasMaxLength(1000);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.LastLoginDate).HasColumnType("datetime");
            entity.Property(e => e.LockedUntil).HasColumnType("datetime");
            entity.Property(e => e.RefreshToken).HasMaxLength(500);
            entity.Property(e => e.RefreshTokenExpiryTime).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).IsRequired().HasMaxLength(128);
            entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.LastModifiedBy).HasMaxLength(128);

            // FK to Employee
            entity.HasOne<Employee>()
                  .WithMany()
                  .HasForeignKey(e => e.EmployeeID)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // EmployeeAddress
        modelBuilder.Entity<EmployeeAddress>(entity =>
        {
            entity.ToTable("EmployeeAddresses");

            entity.HasKey(e => e.AddressID);

            entity.Property(e => e.EmployeeID).IsRequired();
            entity.Property(e => e.AddressType).IsRequired().HasMaxLength(1);
            entity.Property(e => e.CityCode).IsRequired(false);
            entity.Property(e => e.CountryCode).IsRequired(false);
            entity.Property(e => e.AddressText).IsRequired().HasMaxLength(200);
            entity.Property(e => e.NearestPoliceStation).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.MobileNumber).HasMaxLength(20);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.ValidFrom).HasColumnType("date");
            entity.Property(e => e.ValidTo).HasColumnType("date");
            entity.Property(e => e.CreatedBy).IsRequired().HasMaxLength(128);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.LastModifiedBy).HasMaxLength(128);
            entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

            // AddressType constraint
            entity.ToTable(t => t.HasCheckConstraint("CK_AddressType", "AddressType IN ('C', 'P')"));
        });

        // EmployeeBankAccount
        modelBuilder.Entity<EmployeeBankAccount>(entity =>
        {
            entity.ToTable("EmployeeBankAccounts");

            entity.HasKey(e => e.BankAccountID);

            entity.Property(e => e.EmployeeID).IsRequired();
            entity.Property(e => e.BankCode).IsRequired(false);
            entity.Property(e => e.BranchCode).IsRequired(false);
            entity.Property(e => e.CityCode).IsRequired(false);
            entity.Property(e => e.AccountNumber).IsRequired().HasMaxLength(30);
            entity.Property(e => e.IBAN).HasMaxLength(34);
            entity.Property(e => e.BankName).HasMaxLength(100);
            entity.Property(e => e.BankAddress1).HasMaxLength(100);
            entity.Property(e => e.BankAddress2).HasMaxLength(100);
            entity.Property(e => e.CountryCode).IsRequired(false);
            entity.Property(e => e.ValidFrom).HasColumnType("date");
            entity.Property(e => e.ValidTo).HasColumnType("date");

            // FK to Employee
            entity.HasOne<Employee>()
                  .WithMany()
                  .HasForeignKey(e => e.EmployeeID)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ─── Financial Schema ────────────────────────────────────────

        modelBuilder.HasDefaultSchema("dbo");

        // FinancialAccount
        modelBuilder.Entity<FinancialAccount>(entity =>
        {
            entity.ToTable("FinancialAccounts");

            entity.HasKey(e => e.AccountID);

            entity.Property(e => e.EmployeeID).IsRequired();
            entity.Property(e => e.AccountNumber).IsRequired().HasMaxLength(50);
            entity.Property(e => e.AccountName).HasMaxLength(100);
            entity.Property(e => e.Balance).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Currency).HasMaxLength(10);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

            // FK to Employee
            entity.HasOne<Employee>()
                  .WithMany()
                  .HasForeignKey(e => e.EmployeeID)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
