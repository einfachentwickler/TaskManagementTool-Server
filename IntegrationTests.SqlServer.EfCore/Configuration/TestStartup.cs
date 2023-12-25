using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using TaskManagementTool.BusinessLogic.MappingProfiles;
using TaskManagementTool.DataAccess;
using TaskManagementTool.DataAccess.Contracts;
using TaskManagementTool.DataAccess.Entities;
using TaskManagementTool.DataAccess.Factories;
using TaskManagementTool.DataAccess.Repositories;

namespace IntegrationTests.SqlServer.EfCore.Configuration;

public static class TestStartup
{
    public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory + "/Configuration")
        .AddJsonFile("appsettings.test.json")
        .Build();

    public static IDatabaseFactory DatabaseFactory { get; }

    static TestStartup()
    {
        var builder = new DbContextOptionsBuilder<TaskManagementToolDatabase>()
            .UseSqlServer(Configuration.GetSection("ConnectionString").Value);

        DatabaseFactory = new DatabaseFactory(builder.Options);

        DefaultMappingProfile defaultProfile = new();

        void AddProfile(IMapperConfigurationExpression expression) => expression.AddProfile(defaultProfile);

        MapperConfiguration config = new(AddProfile);

        Mapper = config.CreateMapper();

        IUserStore<User> userStore = new UserStore<User>(DatabaseFactory.Create().DbContext);

        IPasswordHasher<User> hasher = new PasswordHasher<User>();

        List<UserValidator<User>> validators = [new UserValidator<User>()];

        ILogger<UserManager<User>> logger = new Mock<ILogger<UserManager<User>>>().Object;

        UserManager = new UserManager<User>(
            userStore,
            null,
            hasher,
            validators,
            null,
            null,
            null,
            null,
            logger
        );

        //Set up token providers.
        UserManager.RegisterTokenProvider("Default", new EmailTokenProvider<User>());
        UserManager.RegisterTokenProvider("PhoneTokenProvider", new PhoneNumberTokenProvider<User>());
    }

    public static IMapper Mapper { get; }

    public static UserManager<User> UserManager { get; }

    public static ITodoRepository Repository => new TodoRepository(DatabaseFactory);
}