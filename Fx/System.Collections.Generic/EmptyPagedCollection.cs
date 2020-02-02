namespace System.Collections.Generic
{
    public sealed class EmptyPagedCollection<T> : IPagedCollection<T>
    {
        private readonly IReadOnlyCollection<T> _list = Array.Empty<T>();

        public int CurrentPageNumber => 0;

        public int? NextPageNumber => null;

        public int? PreviousPageNumber => null;

        public int LastPageNumber => 0;

        public int ItemCount => 0;

        public int PageSize => 0;

        public int PageCount => 0;

        public bool HasPrevious => false;

        public bool HasNext => false;

        public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();
    }
}
