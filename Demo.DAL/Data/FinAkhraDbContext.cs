using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Demo.DAL.Data
{
    public class FinAkhraDbContext : DbContext
    {
        public FinAkhraDbContext(DbContextOptions<FinAkhraDbContext> options) : base(options)
        {
        }

        // DbSets for all entities
        public DbSet<Member> Members { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<LedgerAccount> LedgerAccounts { get; set; }
        public DbSet<JournalEntry> JournalEntries { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Fee> Fees { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<MedicaidRequest> MedicaidRequests { get; set; }
        public DbSet<CashBook> CashBooks { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<Permission> Permissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<Fee>()
                .HasOne(f => f.Member)
                .WithMany(m => m.Fees)
                .HasForeignKey(f => f.MemberId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MedicaidRequest>()
                .HasOne(mr => mr.Member)
                .WithMany(m => m.MedicaidRequests)
                .HasForeignKey(mr => mr.MemberId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<JournalEntry>()
                .HasOne(je => je.CreatedByUser)
                .WithMany(u => u.JournalEntries)
                .HasForeignKey(je => je.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.JournalEntry)
                .WithMany(je => je.Transactions)
                .HasForeignKey(t => t.JournalId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.LedgerAccount)
                .WithMany(la => la.Transactions)
                .HasForeignKey(t => t.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Voucher>()
                .HasOne(v => v.Member)
                .WithMany(m => m.Vouchers)
                .HasForeignKey(v => v.MemberId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Voucher>()
                .HasOne(v => v.LedgerAccount)
                .WithMany(la => la.Vouchers)
                .HasForeignKey(v => v.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Voucher>()
                .HasOne(v => v.JournalEntry)
                .WithMany(je => je.Vouchers)
                .HasForeignKey(v => v.JournalId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure constraints
            modelBuilder.Entity<Fee>()
                .HasCheckConstraint("CK_Fee_Month", "[FeeMonth] >= 1 AND [FeeMonth] <= 12");

            modelBuilder.Entity<LedgerAccount>()
                .HasCheckConstraint("CK_LedgerAccount_Type", 
                    "[AccountType] IN ('Assets', 'Liabilities', 'Income', 'Expense')");

            modelBuilder.Entity<MedicaidRequest>()
                .HasCheckConstraint("CK_MedicaidRequest_Status", 
                    "[Status] IN ('Pending', 'Approved', 'Rejected')");

            modelBuilder.Entity<Voucher>()
                .HasCheckConstraint("CK_Voucher_Type", 
                    "[VoucherType] IN ('Receipt', 'Payment')");

            // Configure decimal precision
            modelBuilder.Entity<CashBook>()
                .Property(c => c.Debit)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<CashBook>()
                .Property(c => c.Credit)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<CashBook>()
                .Property(c => c.Balance)
                .HasColumnType("decimal(10,2)");

            // Seed initial data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed default user
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    UserName = "admin",
                    PasswordHash = "AQAAAAEAACcQAAAAEK8VoBxKWAW8EjQZhL+FjQWrHJV9FjQWrH", // admin123 (should be properly hashed)
                    Role = "Administrator",
                    IsActive = true
                }
            );

            // Seed basic ledger accounts
            modelBuilder.Entity<LedgerAccount>().HasData(
                new LedgerAccount { AccountId = 1, AccountName = "Cash", AccountType = "Assets", IsCashAccount = true },
                new LedgerAccount { AccountId = 2, AccountName = "Bank", AccountType = "Assets", IsCashAccount = true },
                new LedgerAccount { AccountId = 3, AccountName = "Member Fees", AccountType = "Income", IsCashAccount = false },
                new LedgerAccount { AccountId = 4, AccountName = "Event Expenses", AccountType = "Expense", IsCashAccount = false },
                new LedgerAccount { AccountId = 5, AccountName = "Medicaid Fund", AccountType = "Liabilities", IsCashAccount = false }
            );

            // Seed permissions
            modelBuilder.Entity<Permission>().HasData(
                new Permission { Id = 1, Role = "Administrator", Page = "Members", CanView = true, CanEdit = true, CanDelete = true },
                new Permission { Id = 2, Role = "Administrator", Page = "Fees", CanView = true, CanEdit = true, CanDelete = true },
                new Permission { Id = 3, Role = "Administrator", Page = "Events", CanView = true, CanEdit = true, CanDelete = true },
                new Permission { Id = 4, Role = "Administrator", Page = "Accounting", CanView = true, CanEdit = true, CanDelete = true },
                new Permission { Id = 5, Role = "User", Page = "Members", CanView = true, CanEdit = false, CanDelete = false },
                new Permission { Id = 6, Role = "User", Page = "Fees", CanView = true, CanEdit = false, CanDelete = false }
            );
        }
    }
}