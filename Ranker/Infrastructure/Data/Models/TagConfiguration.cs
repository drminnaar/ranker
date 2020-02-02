using System;
using Ranker.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ranker.Infrastructure.Data.Models
{
    public sealed class TagConfiguration : IEntityTypeConfiguration<MovieTag>
    {
        public void Configure(EntityTypeBuilder<MovieTag> entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            entity.HasKey(e => e.TagId);
        }
    }
}
