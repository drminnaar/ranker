using System;
using Ranker.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ranker.Infrastructure.Data.Models
{
    public sealed class MovieConfiguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            entity.HasKey(e => e.MovieId);
        }
    }
}
