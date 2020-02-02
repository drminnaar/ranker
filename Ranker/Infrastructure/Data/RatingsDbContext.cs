using System;
using Ranker.Application;
using Ranker.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Ranker.Infrastructure.Data
{
    public sealed class RatingsDbContext : DbContext, IRatingsDbContext
    {
        public RatingsDbContext(DbContextOptions<RatingsDbContext> options) : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; } = null!;
        public DbSet<Rating> Ratings { get; set; } = null!;
        public DbSet<MovieTag> Tags { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder is null)
                throw new ArgumentNullException(nameof(modelBuilder));

            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RatingsDbContext).Assembly);
        }
    }
}
