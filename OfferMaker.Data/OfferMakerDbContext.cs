namespace OfferMaker.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using OfferMaker.Data.Models;

    public class OfferMakerDbContext : IdentityDbContext<User>
    {
        public OfferMakerDbContext(DbContextOptions<OfferMakerDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Opportunity> Opportunities { get; set; }

        public DbSet<Proposal> Offers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<UserOpportunity>()
                .HasKey(uo => new { uo.UserId, uo.OpportunityId });

            builder
                .Entity<UserOpportunity>()
                .HasOne(uo => uo.User)
                .WithMany(u => u.Opportunities)
                .HasForeignKey(uo => uo.UserId);

            builder
                .Entity<UserOpportunity>()
                .HasOne(uo => uo.Opportunity)
                .WithMany(o => o.Members)
                .HasForeignKey(uo => uo.OpportunityId);

            builder
                .Entity<Account>()
                .HasOne(a => a.Manager)
                .WithMany(u => u.Accounts)
                .HasForeignKey(a => a.ManagerId);

            builder
                .Entity<Opportunity>()
                .HasOne(o => o.Account)
                .WithMany(a => a.Opportunities)
                .HasForeignKey(o => o.AccountId);

            builder
                .Entity<Proposal>()
                .HasOne(p => p.Opportunity)
                .WithMany(o => o.Proposals)
                .HasForeignKey(o => o.OpportunityId);

            base.OnModelCreating(builder);
        }
    }
}
