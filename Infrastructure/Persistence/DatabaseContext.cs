using Domain.Entities;
using Domain.Entities.Clients;
using Domain.Entities.Contracts;
using Domain.Entities.Softwares;
using Domain.Entities.Subscriptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence;

public class DatabaseContext(DbContextOptions opt, IConfiguration configuration) : DbContext(opt)
{
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Token> Tokens { get; set; }
    public virtual DbSet<SubscriptionStatusType> SubscriptionStatusTypes { get; set; }
    public virtual DbSet<Subscription> Subscriptions { get; set; }
    public virtual DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }
    public virtual DbSet<SoftwareDiscount> SoftwareDiscounts { get; set; }
    public virtual DbSet<Software> Softwares { get; set; }
    public virtual DbSet<Discount> Discounts { get; set; }
    public virtual DbSet<ContractStatusType> ContractStatusTypes { get; set; }
    public virtual DbSet<Contract> Contracts { get; set; }
    public virtual DbSet<ContractPayment> ContractPayments { get; set; }
    public virtual DbSet<IndividualClient> IndividualClients { get; set; }
    public virtual DbSet<Client> Clients { get; set; }
    public virtual DbSet<CompanyClient> CompanyClients { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        var schema = configuration["Database:Schema"];
        if (!string.IsNullOrWhiteSpace(schema))
        {
            modelBuilder.HasDefaultSchema(schema);
        }
        
        //USERS --- TOKENS
        modelBuilder.Entity<User>(user =>
        {
            user.ToTable("Users");
            user.HasKey(x => x.Id);
            user.Property(x => x.Id).ValueGeneratedOnAdd();
            user.Property(x => x.Login).HasMaxLength(50).IsRequired();
            user.HasIndex(x => x.Login).IsUnique();
            user.Property(x => x.PasswordHash).HasMaxLength(255).IsRequired();
            user.Property(x => x.Role).HasConversion<string>().HasMaxLength(20).IsRequired();

            user.HasOne(e => e.Token)
                .WithOne(e => e.User)
                .HasForeignKey<Token>(e => e.UserId);
        });

        modelBuilder.Entity<Token>(token =>
        {
            token.ToTable("Tokens");
            token.HasKey(e => e.UserId);
            token.Property(e => e.RefreshToken).HasMaxLength(100).IsRequired();
            token.HasIndex(e => e.RefreshToken).IsUnique();
        });
        
        // Clients
        modelBuilder.Entity<Client>(client =>
        {
            client.ToTable("Clients");
            client.HasKey(x => x.Id);
            client.Property(x => x.Id).ValueGeneratedOnAdd();
            client.Property(x => x.Address).HasMaxLength(150).IsRequired();
            client.Property(x => x.Email).HasMaxLength(100).IsRequired();
            client.Property(x => x.Phone).HasColumnType("char(9)").IsRequired();
            
            client.HasOne(e => e.CompanyClient)
                .WithOne(e => e.Client)
                .HasForeignKey<CompanyClient>(e => e.ClientId)
                .OnDelete(DeleteBehavior.Cascade); // Gdy usuniesz klienta, usunie się też firma
            
            client.HasOne(e => e.IndividualClient)
                .WithOne(e => e.Client)
                .HasForeignKey<IndividualClient>(e => e.ClientId)
                .OnDelete(DeleteBehavior.Cascade); // Gdy usuniesz klienta, usunie się też firma
            
            client.HasMany(e => e.Contracts)
                .WithOne(e => e.Client)
                .HasForeignKey(e => e.ClientId);
            
            client.HasMany(e => e.Subscriptions)
                .WithOne(e => e.Client)
                .HasForeignKey(e => e.ClientId);
        });
        
        modelBuilder.Entity<CompanyClient>(company =>
        {
            company.ToTable("CompanyClients");
            company.HasKey(c => c.ClientId); // ClientId jako PK i FK
            company.Property(c => c.Name).HasMaxLength(100).IsRequired();
            company.Property(c => c.Krs).HasColumnType("char(10)").IsRequired();
            company.HasIndex(c => c.Krs).IsUnique();
        });
        
        modelBuilder.Entity<IndividualClient>(individual =>
        {
            individual.ToTable("IndividualClients");
            individual.HasKey(i => i.ClientId); // ClientId jako PK i FK
            individual.Property(i => i.FirstName).HasMaxLength(50).IsRequired();
            individual.Property(i => i.LastName).HasMaxLength(100).IsRequired();
            individual.Property(i => i.Pesel).HasColumnType("char(11)").IsRequired();
            individual.HasIndex(i => i.Pesel).IsUnique();
        });
        
        // Contracts
        
        modelBuilder.Entity<Contract>(contract =>
        {
            contract.ToTable("Contracts");
            contract.HasKey(c => c.Id);
            contract.Property(c => c.Id).ValueGeneratedOnAdd();
            contract.Property(c => c.SoftwareVersion).HasMaxLength(20).IsRequired();
            contract.Property(c => c.StartDate).HasColumnType("date").IsRequired();
            contract.Property(c => c.EndDate).HasColumnType("date").IsRequired();
            contract.Property(c => c.Price).HasColumnType("decimal(18,2)").IsRequired();
            
            // Nie trzeba bo jest wyżej Klient -> Kontrakty
            // contract.HasOne(c => c.Client)
            //     .WithMany(c => c.Contracts)
            //     .HasForeignKey(c => c.ClientId);

            contract.HasOne(c => c.ContractStatusType)
                .WithMany(s => s.Contracts)
                .HasForeignKey(c => c.StatusId);
            
            contract.HasOne(c => c.Software)
                .WithMany(c => c.Contracts) 
                .HasForeignKey(c => c.SoftwareId);
        });
        
        modelBuilder.Entity<ContractStatusType>(status =>
        {
            status.ToTable("ContractStatusTypes");
            status.HasKey(s => s.Id);
            status.Property(s => s.Id).ValueGeneratedOnAdd();
            status.Property(s => s.Name).HasMaxLength(20).IsRequired();
        });

        modelBuilder.Entity<ContractPayment>(payment =>
        {
            payment.ToTable("ContractPayments");
            payment.HasKey(p => p.Id);
            payment.Property(p => p.Id).ValueGeneratedOnAdd();
            payment.Property(p => p.Amount).HasColumnType("decimal(18,2)").IsRequired();
            payment.Property(p => p.PaymentDate).HasColumnType("date").IsRequired();

            payment.HasOne(p => p.Contract)
                .WithMany(c => c.ContractPayments)
                .HasForeignKey(p => p.ContractId);
        });
        
        // Subscriptions
        
        modelBuilder.Entity<Subscription>(subscription =>
        {
            subscription.ToTable("Subscriptions");
            subscription.HasKey(s => s.Id);
            subscription.Property(s => s.Id).ValueGeneratedOnAdd();
            subscription.Property(s => s.Name).HasMaxLength(100).IsRequired();
            subscription.Property(s => s.PricePerPeriod).HasColumnType("decimal(18,2)").IsRequired();

            subscription.HasOne(s => s.SubscriptionStatusType)
                .WithMany(st => st.Subscriptions)
                .HasForeignKey(s => s.StatusId);
                
            subscription.HasOne(s => s.Software)
                .WithMany()
                .HasForeignKey(s => s.SoftwareId);
        });

        modelBuilder.Entity<SubscriptionStatusType>(status =>
        {
            status.ToTable("SubscriptionStatusTypes");
            status.HasKey(s => s.Id);
            status.Property(s => s.Id).ValueGeneratedOnAdd();
            status.Property(s => s.Name).HasMaxLength(20).IsRequired();
        });

        modelBuilder.Entity<SubscriptionPayment>(payment =>
        {
            payment.ToTable("SubscriptionPayments");
            payment.HasKey(p => p.Id);
            payment.Property(p => p.Id).ValueGeneratedOnAdd();
            payment.Property(p => p.Amount).HasColumnType("decimal(18,2)").IsRequired();
            payment.Property(p => p.PaymentDate).HasColumnType("date").IsRequired();
            payment.Property(p => p.PeriodStart).HasColumnType("date").IsRequired();
            payment.Property(p => p.PeriodEnd).HasColumnType("date").IsRequired();

            payment.HasOne(p => p.Subscription)
                .WithMany(s => s.SubscriptionPayments)
                .HasForeignKey(p => p.SubscriptionId);
        });
        
        // Software & Discount
        
        modelBuilder.Entity<Software>(software =>
        {
            software.ToTable("Softwares");
            software.HasKey(s => s.Id);
            software.Property(s => s.Id).ValueGeneratedOnAdd();
            software.Property(s => s.Name).HasMaxLength(100).IsRequired();
            software.Property(s => s.Description).HasMaxLength(300).IsRequired();
            software.Property(s => s.CurrentVersion).HasMaxLength(20).IsRequired();
            software.Property(s => s.Category).HasMaxLength(50).IsRequired();
        });

        modelBuilder.Entity<Discount>(discount =>
        {
            discount.ToTable("Discounts");
            discount.HasKey(d => d.Id);
            discount.Property(d => d.Id).ValueGeneratedOnAdd();
            discount.Property(d => d.Name).HasMaxLength(50).IsRequired();
            discount.Property(d => d.Value).HasColumnType("decimal(5,2)").IsRequired();
            discount.Property(d => d.DateFrom).HasColumnType("date").IsRequired();
            discount.Property(d => d.DateTo).HasColumnType("date").IsRequired();
        });

        modelBuilder.Entity<SoftwareDiscount>(sd =>
        {
            sd.ToTable("Softwares_Discounts");
            sd.HasKey(e => new { e.SoftwareId, e.DiscountId });

            sd.HasOne(e => e.Software)
                .WithMany(s => s.SoftwareDiscounts)
                .HasForeignKey(e => e.SoftwareId);

            sd.HasOne(e => e.Discount)
                .WithMany(d => d.SoftwareDiscounts)
                .HasForeignKey(e => e.DiscountId);
        });
    }
}
















