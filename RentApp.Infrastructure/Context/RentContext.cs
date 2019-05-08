using Microsoft.EntityFrameworkCore;

namespace RentApp.Infrastructure.Context
{
  public class RentContext : DbContext
  {
    public DbSet<Flat> Flat { get; set; }
    public DbSet<User> User { get; set; }
    public DbSet<Image> Image { get; set; }
    public DbSet<Owner> Owner { get; set; }
    public DbSet<Tenant> Tenant { get; set; }
    public DbSet<Address> Address { get; set; }

    public RentContext(DbContextOptions options) : base(options)
    {
    }

    public RentContext()
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlite("DataSource=dbo.RentFlatApi.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {      
      //FLAT
      modelBuilder.Entity<Flat>()
        .HasMany(x => x.Images)
        .WithOne(y => y.Flat)
        .OnDelete(DeleteBehavior.Cascade);

      modelBuilder.Entity<Flat>()
        .HasOne(x => x.Owner)
        .WithMany(y => y.Flats)
        .OnDelete(DeleteBehavior.Cascade);

      modelBuilder.Entity<Flat>()
        .HasOne(x => x.Tenant)
        .WithOne(y => y.Flat)
        .HasForeignKey<Tenant>(z => z.Id);

      //ADDRESS - brak relacji - pola nie będące w innych klasach
      
      //IMAGE
      modelBuilder.Entity<Image>()
        .HasOne(x => x.Flat)
        .WithMany(y => y.Images)
        .OnDelete(DeleteBehavior.Cascade);
      
      //OWNER
      modelBuilder.Entity<Owner>()
        .HasMany(x => x.Flats)
        .WithOne(y => y.Owner)
        .OnDelete(DeleteBehavior.Cascade);
      
      //PERSON - tylko relacja hasOne address ???

      //TENANT
      modelBuilder.Entity<Tenant>()
        .HasOne(x => x.Flat)
        .WithOne(y => y.Tenant)
        .OnDelete(DeleteBehavior.Cascade);
      
      //USER - brak relacji
      

    }
  }
}
