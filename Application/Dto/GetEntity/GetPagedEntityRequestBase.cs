namespace Application.Dto.GetEntity;

public abstract record GetPagedEntityRequestBase
{
    public int PageSize { get; init; }
    public int PageNumber { get; init; }
}