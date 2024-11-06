using projekt.src.Models.Users;
using Microsoft.EntityFrameworkCore;
using projekt.src.Models.Store;
using projekt.src.Models.Reviews;
using projekt.src.Models.SavedAnnouncements;
using projekt.src.Models.ShoppingCart;
using projekt.src.Models.Orders;

namespace projekt.src.Data;

public class ApiDbContext : DbContext 
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options) 
        : base(options) {}

    public DbSet<User> Users { get; set; }
    public DbSet<Announcement> Announcements { get; set; }
    public DbSet<Reviews> Reviews { get; set; }
    public DbSet<SavedAnnouncements> SavedAnnouncements { get; set; }
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
    public DbSet<Orders> Orders { get; set; }
    public DbSet<OrderedItems> OrderedItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(builder =>
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasConversion(id => id.Value, value => new UserId(value));

            builder.HasIndex(x=>x.Email).IsUnique();
            builder.Property(x=>x.Email).HasConversion(email => email.Value, value => new Email(value));

            builder.Property(x=>x.Name).HasConversion(name => name.Value, value => new Name(value))
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x=>x.LastName).HasConversion(ln=>ln.Value, value=>new LastName(value))
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x=>x.Password).HasConversion(p => p.Value, value => new Password(value))
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x=>x.Address).HasConversion(a => a.Value, value => new Address(value));

            builder.Property(x=>x.Location).HasConversion(Location => Location.Value, value => new Address(value));

            builder.Property(x=>x.PostCode).HasConversion(PostCode => PostCode.Value, value => new PostCode(value));

            builder.Property(x=>x.Phone).HasConversion(Phone =>Phone.Value, value => new Phone(value));
        
            builder.Property(x=>x.Role).HasConversion(Role => Role.Value, value => new AccountType(value))
                .IsRequired();

            builder.Property(x=>x.CreatedAt).IsRequired();
        });

        modelBuilder.Entity<Announcement>(builder =>
        {
            builder.HasKey(x=>x.Id);

            builder.Property(x=>x.OwnerId)
                .HasConversion(id=>id.Value, value => new UserId(value))
                .IsRequired();
            
            builder.OwnsOne(x=>x.Item, ib=>
            {
                ib.WithOwner(); //owned by Announcement

                ib.Property(i=>i.Title)
                    .IsRequired()
                    .HasMaxLength(500);
                
                ib.Property(i=>i.Description)
                    .HasMaxLength(5000);

                ib.Property(i=>i.Amount)
                    .HasConversion(am=>am.Value, value => new ItemAmount(value))
                    .IsRequired();
                    
                ib.Property(i=>i.Categories)
                    .HasConversion(
                        categories => string.Join(',', categories ?? new List<string>()),
                        value => value.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
                    );
                
                ib.Property(i=>i.Cost)
                    .HasConversion(c=>c.Value, value => new Cost(value))
                    .IsRequired();
                
                ib.Property(i => i.ColorsSizesAmounts)
                    .HasConversion(
                        colorsSizesAmounts => colorsSizesAmounts != null 
                            ? string.Join(';', colorsSizesAmounts.Select(kvp => 
                                $"{kvp.Key}:{string.Join(',', kvp.Value.Select(size => $"{size.Key}:{size.Value}"))}"))
                            : string.Empty,
                        value => value.Split(';', StringSplitOptions.RemoveEmptyEntries)
                            .Select(s => s.Split(new[] { ':' }, 2))
                            .ToDictionary(
                                s => s[0],
                                s => s[1].Split(',', StringSplitOptions.RemoveEmptyEntries)
                                    .Select(sz => sz.Split(new[] { ':' }, 2))
                                    .ToDictionary(sz => sz[0], sz => int.Parse(sz[1]))
                            )
                );

                 ib.Property(i => i.ColorsAmount)
                    .HasConversion(
                        ca => ca != null 
                            ? string.Join(';', ca.Select(kvp => $"{kvp.Key}:{kvp.Value}")) 
                            : string.Empty,
                        value => value.Split(';', StringSplitOptions.RemoveEmptyEntries)
                                    .Select(s => s.Split(new[] { ':' }, 2))
                                    .ToDictionary(s => s[0], s => int.Parse(s[1]))
                    );

                ib.Property(i=>i.Model_Brand);

            });

            builder.Property(x=>x.CreatedAt)
                .IsRequired();
        });

        modelBuilder.Entity<Reviews>(builder =>
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.AnnouncementId);

            builder.Property(x => x.UserId)
                .HasConversion(x=>x.Value, value => new UserId(value));

            builder.Property(x => x.OwnerId)
                .IsRequired()
                .HasConversion(id => id.Value, value => new UserId(value));
            
            builder.Property(x => x.Comment);

            builder.Property(x => x.Rating)
                .IsRequired()
                .HasConversion(r=>r.Value, value => new Rating(value));

            builder.Property(x => x.CreatedAt);

        });

        modelBuilder.Entity<SavedAnnouncements>(builder =>
        {
            builder.HasKey(x => new {x.UserId, x.AnnouncementId});

            builder.Property(x => x.UserId)
                .HasConversion(id => id.Value, value => new UserId(value))
                .IsRequired();

            builder.Property(x => x.AnnouncementId)
                .IsRequired();
        });

        modelBuilder.Entity<ShoppingCart>(builder =>
        {
            builder.HasKey(x=>x.Id);

            builder.Property(x=>x.OwnerId)
                .HasConversion(x=>x.Value, value => new UserId(value))
                .IsRequired();

            builder.Property(x=>x.UpdatedAt)
                .IsRequired();

            builder.HasMany(x=>x.Items)
                .WithOne()
                .HasForeignKey(y=>y.ShoppingCartId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x=>x.OwnerId);
        });

         modelBuilder.Entity<ShoppingCartItem>(builder =>
        {
            builder.Property(x => x.Id)
                .IsRequired();

            builder.Property(x => x.Quantity)
                .HasConversion(v => v.Value, value => new Quantity(value))
                .IsRequired();

            builder.Property(x => x.ShoppingCartId)
                .IsRequired();

            builder.Property(x=>x.AnnouncementId)
                .IsRequired();

            builder.Property(x => x.SelectedColor);

            builder.Property(x => x.SelectedSize);
        });

         modelBuilder.Entity<Orders>(builder =>
        {
            builder.HasKey(x=>x.Id);

            builder.Property(x=>x.OrderingPerson)
                .HasConversion(x=>x.Value, value => new UserId(value))
                .IsRequired();

            builder.Property(x=>x.OrderedAt)
                .IsRequired();

            builder.Property(x=>x.DeliveryMethod)
                .HasConversion(x=>x.Value, value=>new DeliveryMethod(value))
                .IsRequired();
            
            builder.HasMany(x=>x.Items)
                .WithOne()
                .HasForeignKey(x=>x.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.OwnsMany(x => x.OwnersId, ownerBuilder =>
                {
                    ownerBuilder.WithOwner(); 

                    ownerBuilder.Property(o => o.Value) 
                        .HasColumnName("OwnersId"); 
                });
            
        });

        modelBuilder.Entity<OrderedItems>(builder =>
        {
            builder.Property(x => x.Id)
                .IsRequired();

            builder.Property(x => x.OrderId)
                .IsRequired();

            builder.Property(x => x.Quantity)
                .HasConversion(v => v.Value, value => new Quantity(value))
                .IsRequired();

            builder.Property(x => x.ShoppingCartId)
                .IsRequired();

            builder.Property(x=>x.AnnouncementId)
                .IsRequired();

            builder.Property(x => x.SelectedColor);

            builder.Property(x => x.SelectedSize);

            builder.Property(x=>x.OrderStatus)
                .HasConversion(x=>x.Value, value => new OrderStatus(value))
                .IsRequired();
        });

    }
}