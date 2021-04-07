using System;
using Ranker.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ranker.Infrastructure.Data.Models
{
    public sealed class TagConfiguration : IEntityTypeConfiguration<MovieTag>
    {
        public void Configure(EntityTypeBuilder<MovieTag> builder)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            builder.HasKey(e => e.TagId);
        }
    }
}
