using System;
using Ranker.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ranker.Infrastructure.Data.Models
{
    public sealed class RatingConfiguration : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            entity.HasKey(e => e.RatingId);
        }
    }
}
