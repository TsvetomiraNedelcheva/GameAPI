using Data;
using Microsoft.EntityFrameworkCore;

namespace Mapping
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
          : base(options)
        {
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Game>().HasKey(x => x.Id);
            builder.Entity<Game>().Property(x => x.Name).IsRequired();
            builder.Entity<Game>().HasMany(x => x.Tags).WithMany(x => x.Games);
            builder.Entity<Game>().HasOne<Genre>().WithMany(x => x.Games);

            builder.Entity<Genre>().HasKey(x => x.Id);
            builder.Entity<Genre>().Property(x => x.Name).IsRequired();
            builder.Entity<Genre>().HasMany(x => x.Games).WithOne(x => x.Genre);

            builder.Entity<Tag>().HasKey(x => x.Id);
            builder.Entity<Tag>().Property(x => x.Name).IsRequired();
            builder.Entity<Tag>().HasMany(x => x.Games).WithMany(x => x.Tags);

            builder.Ignore<Money>();
            builder.Ignore<CurrencyCode>();


            var entityTypes = builder.Model.GetEntityTypes().ToList();

            var foreignKeys = entityTypes
                .SelectMany(e => e.GetForeignKeys().Where(f => f.DeleteBehavior == DeleteBehavior.Cascade));

            foreach (var foreignKey in foreignKeys)
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(builder);
        }
    }
}
