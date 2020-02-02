using System.Collections.Generic;
using System.Threading.Tasks;
using Ranker.Application.Users.Models;

namespace Ranker.Application.Users
{
    public interface IUserService
    {
        Task<UserDetail> CreateUser(UserForCreate user);
        Task DeleteUser(long userId);
        Task<UserDetail> GetUser(long userId);
        Task<UserForPatch?> GetUserForPatch(long userId);
        Task<IPagedCollection<UserDetail>> GetUserList(UserQuery query);
        Task PatchUser(long userId, UserForPatch userForPatch);
        Task UpdateUser(long userId, UserForUpdate userForUpdate);
    }
}