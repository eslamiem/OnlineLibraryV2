using System.Transactions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Models;

namespace OnlineLibrary.Data;

public class ApplicationDbContext : IdentityDbContext<CustomUser, CustomRole, string>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Book> Books { get; set; }
    public DbSet<BorrowTransaction> BorrowTransactions { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Use seed method here
        SeedUsersRoles seedUsersRoles = new();
        builder.Entity<CustomRole>().HasData(seedUsersRoles.Roles);
        builder.Entity<CustomUser>().HasData(seedUsersRoles.Users);
        builder.Entity<IdentityUserRole<string>>().HasData(seedUsersRoles.UserRoles);

        builder.Entity<Book>().ToTable("Book");
        builder.Entity<BorrowTransaction>().ToTable("BorrowTransaction");

        builder.Seed();
    }
}
