using lib.Models;
using Microsoft.EntityFrameworkCore;

namespace lib.Data;

public class LibContext : DbContext
{
    public LibContext(DbContextOptions<LibContext> options) : base(options) { }

    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Author>()
            .HasMany(a => a.Books)
            .WithOne(a => a.Author)
            .HasForeignKey(b => b.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AuthorPublisher>()
            .HasKey(ap => ap.Id);

        modelBuilder.Entity<AuthorPublisher>()
            .HasOne(ap => ap.Author)
            .WithMany(a => a.AuthorPublishers)
            .HasForeignKey(ap => ap.AuthorId);


        modelBuilder.Entity<AuthorPublisher>()
            .HasOne(ap => ap.Publisher)
            .WithMany(p => p.AuthorPublishers)
            .HasForeignKey(ap => ap.PublisherId);
    }
}
