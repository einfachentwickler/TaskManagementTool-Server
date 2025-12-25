namespace Application.Dto;

public abstract record GetPagedEntityBase
{
    public int PageSize { get; init; }
    public int PageNumber { get; init; }
}