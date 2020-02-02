using System;
using System.Collections.Generic;

namespace Ranker.Domain.Models
{
    public class User
    {
        public long UserId { get; set; }
        public long Age { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; }= string.Empty;
        public string Gender { get; set; }= string.Empty;
        public string Email { get; set; }= string.Empty;
        public virtual ICollection<Rating> Ratings { get; } = new HashSet<Rating>();
        public virtual ICollection<MovieTag> Tags { get; } = new HashSet<MovieTag>();

        public static User Default() => new User();
        public static IReadOnlyCollection<User> ToReadOnlyCollection() => Array.Empty<User>();
    }
}
