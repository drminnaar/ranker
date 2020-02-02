using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace Ranker.Api.Models.Users
{
    [DataContract(Name = "UserList", Namespace = "")]
    [KnownType(typeof(ResourceBase))]
    public sealed class UserResourceList : ResourceBase
    {
        public UserResourceList(IUrlHelper urlHelper, IReadOnlyCollection<UserResource> users) : base(urlHelper)
        {
            Users = users ?? throw new ArgumentNullException(nameof(users));
        }

        [DataMember(Order = 1)]
        public IEnumerable<UserResource> Users { get; }
    }
}
