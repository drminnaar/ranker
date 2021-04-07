using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Ranker.Application.Users.Filters;
using Ranker.Application.Users.Models;
using Ranker.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Ranker.Application.Users
{
    public sealed class UserService : IUserService
    {
        private readonly IRatingsDbContext _context;
        private readonly IUserFilterBuilder _filter;
        private readonly IEntityOrderBuilder<User> _order;
        private readonly IMapper _mapper;

        public UserService(
            IRatingsDbContext context,
            IUserFilterBuilder filter,
            IEntityOrderBuilder<User> order,
            IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _filter = filter ?? throw new ArgumentNullException(nameof(filter));
            _order = order ?? throw new ArgumentNullException(nameof(order));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public Task<UserDetail> CreateUser(UserForCreate user)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user));

            return CreateUser();

            async Task<UserDetail> CreateUser()
            {
                var userForCreate = _mapper.Map<User>(user);
                _context.Users.Add(userForCreate);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return await GetUser(userForCreate.UserId).ConfigureAwait(false);
            }
        }

        public async Task DeleteUser(long userId)
        {
            var userFromDb = await _context
                    .Users
                    .FirstOrDefaultAsync(user => user.UserId == userId)
                    .ConfigureAwait(false);

            if (userFromDb == null)
                throw new EntityNotFoundException($"A user having id '{userId}' could not be found");

            _context.Users.Remove(userFromDb);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<UserDetail> GetUser(long userId)
        {
            var userFromDb = await _context
                .Users
                .Where(user => user.UserId == userId)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            return _mapper.Map<UserDetail>(userFromDb);
        }

        public async Task<UserForPatch?> GetUserForPatch(long userId)
        {
            var userFromDb = await _context
                .Users
                .TagWithQueryName(nameof(GetUserForPatch))
                .AsNoTracking()
                .FirstOrDefaultAsync(user => user.UserId == userId)
                .ConfigureAwait(false);

            return userFromDb == null ? null : _mapper.Map<UserForPatch>(userFromDb);
        }

        public Task<IPagedCollection<UserDetail>> GetUserList(UserQuery query)
        {
            if (query is null)
                throw new ArgumentNullException(nameof(query));

            return GetUserList();

            async Task<IPagedCollection<UserDetail>> GetUserList()
            {
                var filter = _filter
                    .WhereAge(query.MinimumAge, query.MaximumAge)
                    .WhereEmailEquals(query.Email)
                    .WhereFirstNameEquals(query.FirstName)
                    .WhereGenderEquals(query.Gender)
                    .WhereLastNameEquals(query.LastName)
                    .Filter;

                var usersFromDb = await _context
                    .Users
                    .Where(filter)
                    .OrderBy(query.Order, _order)
                    .ToPagedCollectionAsync(query.Page, query.Limit)
                    .ConfigureAwait(false);

                var users = _mapper.Map<IReadOnlyList<UserDetail>>(usersFromDb);

                return new PagedCollection<UserDetail>(
                    users,
                    usersFromDb.ItemCount,
                    usersFromDb.CurrentPageNumber,
                    usersFromDb.PageSize);
            }
        }

        public Task PatchUser(long userId, UserForPatch userForPatch)
        {
            if (userForPatch is null)
                throw new ArgumentNullException(nameof(userForPatch));

            return PatchUser();

            async Task PatchUser()
            {
                var userFromDb = await _context
                    .Users
                    .FirstOrDefaultAsync(user => user.UserId == userId)
                    .ConfigureAwait(false);

                if (userFromDb == null)
                    throw new EntityNotFoundException($"A user having id '{userId}' could not be found");

                userFromDb.Age = userForPatch.Age;
                userFromDb.Email = userForPatch.Email;
                userFromDb.FirstName = userForPatch.FirstName;
                userFromDb.Gender = userForPatch.Gender;
                userFromDb.LastName = userForPatch.LastName;

                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public Task UpdateUser(long userId, UserForUpdate userForUpdate)
        {
            if (userForUpdate is null)
                throw new ArgumentNullException(nameof(userForUpdate));

            return UpdateUser();

            async Task UpdateUser()
            {
                var userFromDb = await _context
                    .Users
                    .FirstOrDefaultAsync(user => user.UserId == userId)
                    .ConfigureAwait(false);

                if (userFromDb == null)
                    throw new EntityNotFoundException($"A user having id '{userId}' could not be found");

                userFromDb.Age = userForUpdate.Age;
                userFromDb.Email = userForUpdate.Email;
                userFromDb.FirstName = userForUpdate.FirstName;
                userFromDb.Gender = userForUpdate.Gender;
                userFromDb.LastName = userForUpdate.LastName;

                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}
