using System.Transactions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Models;

namespace OnlineLibrary.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Book> Books { get; set; }
    public DbSet<BookTransaction> BookTransactions { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Use seed method here
        SeedUsersRoles seedUsersRoles = new();
        builder.Entity<IdentityRole>().HasData(seedUsersRoles.Roles);
        builder.Entity<IdentityUser>().HasData(seedUsersRoles.Users);
        builder.Entity<IdentityUserRole<string>>().HasData(seedUsersRoles.UserRoles);

        builder.Entity<Book>().ToTable("Book");
        builder.Entity<BookTransaction>().ToTable("BookTransaction");

        builder.Seed();
    }
}
