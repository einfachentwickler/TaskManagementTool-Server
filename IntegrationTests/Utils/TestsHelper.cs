﻿using IntegrationTests.Constants;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;
using TaskManagementTool.BusinessLogic.ViewModels;
using TaskManagementTool.BusinessLogic.ViewModels.AuthModels;
using TaskManagementTool.BusinessLogic.ViewModels.ToDoModels;

namespace IntegrationTests.Utils;
public static class TestsHelper
{
    public static async Task<HttpResponseMessage> RegisterUserAsync(HttpClient client, string email, string password, string confirmPassword)
    {
        RegisterDto registerDto = new()
        {
            Age = 34,
            Password = password,
            ConfirmPassword = confirmPassword,
            Email = email,
            FirstName = Guid.NewGuid().ToString(),
            LastName = Guid.NewGuid().ToString()
        };

        return await client.PostAsJsonAsync(UriConstants.REGISTER_URI, registerDto);
    }

    public static async Task LoginAsync(HttpClient client, string email, string password)
    {
        LoginDto loginDto = new()
        {
            Email = email,
            Password = password
        };

        HttpResponseMessage loginResponse = await client.PostAsJsonAsync(UriConstants.LOGIN_URI, loginDto);

        string token = (await loginResponse.Content.ReadFromJsonAsync<UserManagerResponse>())!.Message;

        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
    }

    public static async Task<HttpResponseMessage> CreateTodoAsync(HttpClient client)
    {
        CreateTodoDto createTodoDto = new()
        {
            Name = "Todo 1",
            Content = "Content 1",
            Importance = 5
        };

        return await client.PostAsJsonAsync(UriConstants.CREATE_TODO_URI, createTodoDto);
    }
}