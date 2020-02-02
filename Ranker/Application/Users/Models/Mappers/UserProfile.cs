using System.Collections.Generic;
using AutoMapper;
using Ranker.Domain.Models;

namespace Ranker.Application.Users.Models.Mappers
{
    public sealed class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDetail>();
            CreateMap<User, UserForPatch>();
            CreateMap<IPagedCollection<User>, List<UserDetail>>();
            CreateMap<UserForCreate, User>();
        }
    }
}
