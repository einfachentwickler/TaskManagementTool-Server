USE [TaskManagementTool_Test]
GO
DROP PROCEDURE IF EXISTS sp_create_AspNetRoles

GO
CREATE PROCEDURE sp_create_AspNetRoles AS
BEGIN
	SET ANSI_NULLS ON

	SET QUOTED_IDENTIFIER ON

	CREATE TABLE [dbo].[AspNetRoles] (
		[Id]               NVARCHAR (450) NOT NULL,
		[Name]             NVARCHAR (256) NULL,
		[NormalizedName]   NVARCHAR (256) NULL,
		[ConcurrencyStamp] NVARCHAR (MAX) NULL
	);

	CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex]
		ON [dbo].[AspNetRoles]([NormalizedName] ASC) WHERE ([NormalizedName] IS NOT NULL);

	ALTER TABLE [dbo].[AspNetRoles]
		ADD CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED ([Id] ASC);
END


