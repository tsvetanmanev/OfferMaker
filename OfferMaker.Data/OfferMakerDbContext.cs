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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
