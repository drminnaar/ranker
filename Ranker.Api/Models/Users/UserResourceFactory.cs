using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Ranker.Application.Users.Models;

namespace Ranker.Api.Models.Users
{
    public sealed class UserResourceFactory
    {
        private readonly IUrlHelper _urlHelper;

        public UserResourceFactory(IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper ?? throw new ArgumentNullException(nameof(urlHelper));
        }

        public UserResource CreateUserResource(UserDetail user)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user));

            return (UserResource)new UserResource(_urlHelper, user)
                .AddDelete("delete-user", "DeleteUser", new { userId = user.UserId })
                .AddGet("self", "GetUserById", new { userId = user.UserId })
                .AddGet("users", "GetUserList", new PagedQueryParams())
                .AddOptions("GetUserOptions")
                .AddPatch("patch-user", "PatchUser", new { userId = user.UserId })
                .AddPost("create-user", "CreateUser")
                .AddPut("update-user", "UpdateUser", new { userId = user.UserId })
                .AddGet("ratings", "GetUserRatingList", new { userId = user.UserId });
        }

        public ResourceBase CreateUserResourceList(IPagedCollection<UserDetail> users, UserQuery query)
        {
            var userResources = users
                .Select(user => CreateUserResource(user))
                .ToList();

            var routeName = "GetUserList";

            return new UserResourceList(_urlHelper, userResources)
                .AddCurrentPage(routeName, users, query)
                .AddNextPage(routeName, users, query)
                .AddPreviousPage(routeName, users, query)
                .AddFirstPage(routeName, users, query)
                .AddLastPage(routeName, users, query);
        }
    }
}
