namespace Application.Commands.Home.CreateTodo.Models;

public enum CreateTodoErrorCode
{
    InvalidRequest,
    InvalidName,
    InvalidContent,
    ContentTooLarge
}