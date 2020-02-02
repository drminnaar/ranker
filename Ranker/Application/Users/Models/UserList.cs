using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Ranker.Application.Users.Models
{
    [CollectionDataContract(Name = "Users", Namespace = "")]
    public sealed class UserList : IEnumerable<UserDetail>
    {
        private readonly List<UserDetail> _users = new List<UserDetail>();

        public UserList()
        {
        }

        public UserList(IEnumerable<UserDetail> collection)
        {
            _users = new List<UserDetail>(collection);
        }

        internal void Add(UserDetail item)
        {
            _users.Add(item);
        }

        public IEnumerator<UserDetail> GetEnumerator() => _users.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _users.GetEnumerator();
    }
}
