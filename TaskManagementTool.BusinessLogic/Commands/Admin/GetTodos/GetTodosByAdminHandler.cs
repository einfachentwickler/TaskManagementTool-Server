using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementTool.BusinessLogic.Commands.Admin.GetTodos.Models;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.DataAccess.Contracts;
using TaskManagementTool.DataAccess.Entities;

namespace TaskManagementTool.BusinessLogic.Commands.Admin.GetTodos;

public class GetTodosByAdminHandler(ITodoRepository todoRepository, IMapper mapper) : IRequestHandler<GetTodosByAdminRequest, GetTodosByAdminResponse>
{
    public async Task<GetTodosByAdminResponse> Handle(GetTodosByAdminRequest request, CancellationToken cancellationToken)
    {
        IEnumerable<TodoEntry> todos = await todoRepository.GetAsync(request.PageSize, request.PageNumber);

        return new GetTodosByAdminResponse { Todos = mapper.Map<IEnumerable<TodoDto>>(todos) };
    }
}