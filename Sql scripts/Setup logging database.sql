CREATE DATABASE TaskManagementToolLogs;
USE TaskManagementToolLogs;

DROP TABLE IF EXISTS [Logs];
CREATE TABLE Logs(
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	[Datetime] DATETIME NOT NULL,
	Log_Level VARCHAR(15)  NOT NULL,
	[Message] VARCHAR(MAX) NOT NULL
);