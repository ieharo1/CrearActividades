namespace EnterpriseMediaVault.Application.Common;

public sealed class PagedResult<T>
{
    public IReadOnlyCollection<T> Items { get; init; } = Array.Empty<T>();
    public long Total { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
}
