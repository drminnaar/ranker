namespace System.Collections.Generic
{
    public interface IPagedCollection<T> : IEnumerable<T>
    {
        int CurrentPageNumber { get; }
        int? NextPageNumber { get; }
        int? PreviousPageNumber { get; }
        int LastPageNumber { get; }
        int ItemCount { get; }
        int PageSize { get; }
        int PageCount { get; }
        bool HasPrevious { get; }
        bool HasNext { get; }
    }
}
