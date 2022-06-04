using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using TaskManagementTool.DataAccess;
using TaskManagementTool.DataAccess.Contracts;
using TaskManagementTool.DataAccess.Entities;
using TaskManagementTool.DataAccess.Factories;
using TaskManagementTool.DataAccess.Repositories;
using TaskManagementTool.Host.Configuration.Profiles;

namespace IntegrationTests.SqlServer.EfCore.Configuration
{
    public class TestStartup
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory + "/Configuration")
            .AddJsonFile("appsettings.test.json")
            .Build();

        public static IDatabaseFactory DatabaseFactory { get; }

        static TestStartup()
        {
            #region Dao setup
            DbContextOptionsBuilder<TaskManagementToolDatabase> builder =
                new DbContextOptionsBuilder<TaskManagementToolDatabase>()
                    .UseSqlServer(Configuration.GetSection("ConnectionString").Value);

            DbContextOptions<TaskManagementToolDatabase> options = builder.Options;

            DatabaseFactory = new DatabaseFactory(options);
            #endregion

            #region Mapper setup
            DefaultMappingProfile defaultProfile = new();

            void AddProfile(IMapperConfigurationExpression expression) => expression.AddProfile(defaultProfile);

            MapperConfiguration config = new(AddProfile);

            Mapper = config.CreateMapper();
            #endregion

            #region User manager setup
            IUserStore<User> userStore = new UserStore<User>(DatabaseFactory.Create().DbContext);

            IPasswordHasher<User> hasher = new PasswordHasher<User>();

            UserValidator<User> validator = new();
            List<UserValidator<User>> validators = new() { validator };

            ILogger<UserManager<User>> logger = new Mock<ILogger<UserManager<User>>>().Object;

            UserManager = new UserManager<User>(
                userStore,
                null,
                hasher, validators,
                null,
                null,
                null,
                null,
                logger
            );

            // Set-up token providers.
            IUserTwoFactorTokenProvider<User> tokenProvider = new EmailTokenProvider<User>();
            UserManager.RegisterTokenProvider("Default", tokenProvider);
            IUserTwoFactorTokenProvider<User> phoneTokenProvider = new PhoneNumberTokenProvider<User>();
            UserManager.RegisterTokenProvider("PhoneTokenProvider", phoneTokenProvider);
            #endregion
        }

        public static IMapper Mapper { get; }

        public static UserManager<User> UserManager { get; }

        public static ITodoRepository Repository => new TodoRepository(DatabaseFactory);
    }
}
