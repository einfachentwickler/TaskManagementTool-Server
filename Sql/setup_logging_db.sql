CREATE DATABASE TaskManagementToolLogs;
GO

USE TaskManagementToolLogs;
GO

IF EXISTS (SELECT [name] FROM master.sys.server_principals WHERE [name] = 'TaskManagementToolLogsLogin')
BEGIN
	DROP LOGIN [TaskManagementToolLogsLogin];
END;

CREATE LOGIN TaskManagementToolLogsLogin WITH PASSWORD = '123456789Qq';
			
DROP USER IF EXISTS [TaskManagementToolLogsUser];
CREATE USER [TaskManagementToolLogsUser] FOR LOGIN TaskManagementToolLogsLogin;
	
DROP TABLE IF EXISTS [ct_logs];
CREATE TABLE [ct_logs](
	[id] int NOT NULL PRIMARY KEY IDENTITY(1,1),
    [created_on] datetime NOT NULL,
    [level] nvarchar(10),
    [message] nvarchar(max),
    [stack_trace] nvarchar(max),
    [exception] nvarchar(max),
    [logger] nvarchar(255),
    [url] nvarchar(255)
);