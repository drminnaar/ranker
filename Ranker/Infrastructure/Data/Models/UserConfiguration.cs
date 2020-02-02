using System;
using Ranker.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ranker.Infrastructure.Data.Models
{
    public sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            entity.HasKey(e => e.UserId);
        }
    }
}
