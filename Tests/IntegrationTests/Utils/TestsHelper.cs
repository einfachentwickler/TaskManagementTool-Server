using Application.Commands.Auth.Login.Models;
using Application.Commands.Auth.Register.Models;
using Application.Commands.Home.CreateTodo.Models;
using IntegrationTests.Constants;
using System.Net.Http.Json;

namespace IntegrationTests.Utils;

public static class TestsHelper
{
    public static async Task<HttpResponseMessage> RegisterUserAsync(HttpClient client, string email, string password, string confirmPassword)
    {
        var registerDto = new UserRegisterCommand
        {
            Age = 34,
            Password = password,
            ConfirmPassword = confirmPassword,
            Email = email,
            FirstName = Guid.NewGuid().ToString(),
            LastName = Guid.NewGuid().ToString()
        };

        return await client.PostAsJsonAsync(UriConstants.AUTH_REGISTER_URI, registerDto);
    }

    public static async Task LoginAsync(HttpClient client, string email, string password)
    {
        var loginDto = new UserLoginCommand
        {
            Email = email,
            Password = password
        };

        var loginResponse = await client.PostAsJsonAsync(UriConstants.AUTH_LOGIN_URI, loginDto);

        string token = (await loginResponse.Content.ReadFromJsonAsync<UserLoginResponse>())!.Token;

        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
    }

    public static async Task<HttpResponseMessage> CreateTodoAsync(HttpClient client, string name = "Todo 1")
    {
        var createTodoDto = new CreateTodoDto
        {
            Name = name,
            Content = "Content 1",
            Importance = 5
        };

        return await client.PostAsJsonAsync(UriConstants.HOME_CREATE_TODO_URI, createTodoDto);
    }
}