using lib.Models;
using Microsoft.EntityFrameworkCore;

namespace lib.Data;

public class LibContext : DbContext
{
    public LibContext(DbContextOptions<LibContext> options) : base(options) { }

    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //        => optionsBuilder.UseNpgsql("Ho:30st=localhost;Port=5432;Database=lib;Username=algrvvv;Password=");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Author>()
            .HasMany(a => a.Books)
            .WithOne(a => a.Author)
            .HasForeignKey(b => b.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
