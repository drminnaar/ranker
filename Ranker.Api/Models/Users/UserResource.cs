using System.Runtime.Serialization;
using Microsoft.AspNetCore.Mvc;
using Ranker.Application.Users.Models;

namespace Ranker.Api.Models.Users
{
    [DataContract(Name = "User", Namespace = "")]
    [KnownType(typeof(ResourceBase))]
    public sealed class UserResource : ResourceBase
    {
        public UserResource(IUrlHelper urlHelper) : base(urlHelper)
        {
        }

        public UserResource(IUrlHelper urlHelper, UserDetail user) : base(urlHelper)
        {
            if (user is null)
                throw new System.ArgumentNullException(nameof(user));

            Age = user.Age;
            Email = user.Email;
            FirstName = user.FirstName;
            Gender = user.Gender;
            LastName = user.LastName;
            UserId = user.UserId;
        }

        [DataMember(Order = 1)]
        public long UserId { get; set; }

        [DataMember(Order = 2)]
        public long Age { get; set; }

        [DataMember(Order = 3)]
        public string FirstName { get; set; } = string.Empty;

        [DataMember(Order = 4)]
        public string LastName { get; set; } = string.Empty;

        [DataMember(Order = 5)]
        public string Gender { get; set; } = string.Empty;

        [DataMember(Order = 6)]
        public string Email { get; set; } = string.Empty;
    }
}
