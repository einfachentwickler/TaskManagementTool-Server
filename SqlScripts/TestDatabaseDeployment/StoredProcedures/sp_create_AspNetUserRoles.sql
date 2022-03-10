USE [TaskManagementTool_Test]

DROP PROCEDURE IF EXISTS sp_create_AspNetUserRoles;

GO
CREATE PROCEDURE sp_create_AspNetUserRoles AS
BEGIN
	SET ANSI_NULLS ON

	SET QUOTED_IDENTIFIER ON

	CREATE TABLE [dbo].[AspNetUserRoles] (
		[UserId] NVARCHAR (450) NOT NULL,
		[RoleId] NVARCHAR (450) NOT NULL
	);

	CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_RoleId]
		ON [dbo].[AspNetUserRoles]([RoleId] ASC);

	ALTER TABLE [dbo].[AspNetUserRoles]
		ADD CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC);

	ALTER TABLE [dbo].[AspNetUserRoles]
		ADD CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE;

	ALTER TABLE [dbo].[AspNetUserRoles]
		ADD CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE;

END


