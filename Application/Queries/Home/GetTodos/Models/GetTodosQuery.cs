using Application.Dto.GetEntity;
using MediatR;

namespace Application.Queries.Home.GetTodos.Models;

public record GetTodosQuery : GetPagedEntityRequestBase, IRequest<GetTodosResponse> { }