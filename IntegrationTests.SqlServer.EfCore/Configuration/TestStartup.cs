using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using TaskManagementTool.DataAccess;
using TaskManagementTool.DataAccess.Contracts;
using TaskManagementTool.DataAccess.Entities;
using TaskManagementTool.DataAccess.Repositories;
using TaskManagementTool.Host.Profiles;

namespace IntegrationTests.SqlServer.EfCore.Configuration
{
    public class TestStartup
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory+"/Configuration")
            .AddJsonFile("appsettings.test.json")
            .Build();

        public static readonly Dao Dao;

        static TestStartup()
        {
            #region Dao setup
            DbContextOptionsBuilder<Dao> builder =
                new DbContextOptionsBuilder<Dao>()
                    .UseSqlServer(Configuration.GetSection("ConnectionString").Value);

            DbContextOptions<Dao> options = builder.Options;

            Dao = new Dao(options);
            #endregion

            #region Mapper setup
            DefaultMappingProfile defaultProfile = new();

            void AddProfile(IMapperConfigurationExpression expression) => expression.AddProfile(defaultProfile);

            MapperConfiguration config = new(AddProfile);

            Mapper = config.CreateMapper();
            #endregion
        }

        public static IMapper Mapper { get; }

        public static ITodoRepository Repository => new TodoRepository(Dao);
    }
}
