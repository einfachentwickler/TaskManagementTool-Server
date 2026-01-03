using MediatR;

namespace Application.Commands.Auth.Logout.Models;

public record LogoutCommand(string RefreshToken) : IRequest;