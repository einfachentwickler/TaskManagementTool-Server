USE [TaskManagementTool_Test]
GO
DROP PROCEDURE IF EXISTS sp_create_AspNetRoleClaims

GO
CREATE PROCEDURE sp_create_AspNetRoleClaims AS
BEGIN
	SET ANSI_NULLS ON

	SET QUOTED_IDENTIFIER ON

	CREATE TABLE [dbo].[AspNetRoleClaims] (
		[Id]         INT            IDENTITY (1, 1) NOT NULL,
		[RoleId]     NVARCHAR (450) NOT NULL,
		[ClaimType]  NVARCHAR (MAX) NULL,
		[ClaimValue] NVARCHAR (MAX) NULL
	);

	CREATE NONCLUSTERED INDEX [IX_AspNetRoleClaims_RoleId]
		ON [dbo].[AspNetRoleClaims]([RoleId] ASC);

	ALTER TABLE [dbo].[AspNetRoleClaims]
		ADD CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED ([Id] ASC);

	ALTER TABLE [dbo].[AspNetRoleClaims]
		ADD CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE;
END


