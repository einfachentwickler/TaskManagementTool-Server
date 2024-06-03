using IntegrationTests.Constants;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;
using TaskManagementTool.BusinessLogic.Commands.Auth.Login.Models;
using TaskManagementTool.BusinessLogic.Commands.Auth.Register.Models;
using TaskManagementTool.BusinessLogic.ViewModels.ToDoModels;

namespace IntegrationTests.Utils;
public static class TestsHelper
{
    public static async Task<HttpResponseMessage> RegisterUserAsync(HttpClient client, string email, string password, string confirmPassword)
    {
        UserRegisterRequest registerDto = new()
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
        UserLoginRequest loginDto = new()
        {
            Email = email,
            Password = password
        };

        HttpResponseMessage loginResponse = await client.PostAsJsonAsync(UriConstants.AUTH_LOGIN_URI, loginDto);

        string token = (await loginResponse.Content.ReadFromJsonAsync<UserLoginResponse>())!.Message;

        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
    }

    public static async Task<HttpResponseMessage> CreateTodoAsync(HttpClient client, string name = "Todo 1")
    {
        CreateTodoDto createTodoDto = new()
        {
            Name = name,
            Content = "Content 1",
            Importance = 5
        };

        return await client.PostAsJsonAsync(UriConstants.HOME_CREATE_TODO_URI, createTodoDto);
    }
}