using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Ranker.Domain.Models;

namespace Ranker.Infrastructure.Data
{
    public sealed class Seeder
    {
        private readonly RatingsDbContext _context;

        public Seeder(RatingsDbContext context)
        {
            _context = context;
        }

        public Seeder IncludeMovies(string moviesFilePath)
        {
            if (_context.Ratings.Any())
                return this;

            Movies = GetData<Movie>(moviesFilePath);

            return this;
        }

        public Seeder IncludeTags(string tagsFilePath)
        {
            if (_context.Tags.Any())
                return this;

            Tags = GetData<MovieTag>(tagsFilePath);

            return this;
        }

        public Seeder IncludeUsers(string usersFilePath)
        {
            if (_context.Users.Any())
                return this;

            Users = GetData<User>(usersFilePath);

            return this;
        }

        public Seeder IncludeRatings(string ratingsFilePath)
        {
            if (_context.Ratings.Any())
                return this;

            Ratings = GetData<Rating>(ratingsFilePath);

            return this;
        }

        public void Seed()
        {
            SeedUsers();
            SeedMovies();
            SeedRatings();
            SeedTags();
            _context.SaveChanges();
        }

        private IReadOnlyCollection<Rating> Ratings { get; set; } = Rating.ToReadOnlyCollection();
        private IReadOnlyCollection<Movie> Movies { get; set; } = Movie.ToReadOnlyCollection();
        private IReadOnlyCollection<User> Users { get; set; } = User.ToReadOnlyCollection();
        private IReadOnlyCollection<MovieTag> Tags { get; set; } = MovieTag.ToReadOnlyCollection();

        private void SeedMovies()
        {
            if (!Movies.Any() || _context.Movies.Any())
                return;

            _context.Movies.AddRange(Movies);
        }

        private void SeedUsers()
        {
            if (!Users.Any() || _context.Users.Any())
                return;

            _context.Users.AddRange(Users);
        }

        private void SeedRatings()
        {
            if (!Ratings.Any() || _context.Ratings.Any())
                return;

            _context.Ratings.AddRange(Ratings);
        }

        private void SeedTags()
        {
            if (!Tags.Any() || _context.Tags.Any())
                return;

            _context.Tags.AddRange(Tags);
        }

        private static IReadOnlyCollection<T> GetData<T>(string entityFilePath)
        {
            if (string.IsNullOrWhiteSpace(entityFilePath))
            {
                var message = $"The specified parameter '{entityFilePath}'" +
                  " may not be null, empty, or whitespace.";

                throw new ArgumentException(message, nameof(entityFilePath));
            }

            if (!File.Exists(entityFilePath))
            {
                throw new FileNotFoundException(
                   $"The path '{entityFilePath}' could not be found.",
                   entityFilePath);
            }

            var content = File.ReadAllText(entityFilePath);

            if (string.IsNullOrWhiteSpace(content))
            {
                var message = $"The specified file '{entityFilePath}'" +
                   " contains no data.";

                throw new InvalidDataException(message);
            }

            var entities = JsonSerializer.Deserialize<T[]>(content, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            if (entities == null || !entities.Any())
            {
                var message = $"Expected list of {typeof(T).Name}'s but" +
                   $" received none for specified file '{entityFilePath}'";

                throw new InvalidDataException(message);
            }

            return entities;
        }
    }
}
