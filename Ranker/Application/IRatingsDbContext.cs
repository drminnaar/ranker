using System.Threading;
using System.Threading.Tasks;
using Ranker.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Ranker.Application
{
    public interface IRatingsDbContext
    {
        DbSet<Movie> Movies { get; }
        DbSet<Rating> Ratings { get; }
        DbSet<MovieTag> Tags { get; }
        DbSet<User> Users { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}
