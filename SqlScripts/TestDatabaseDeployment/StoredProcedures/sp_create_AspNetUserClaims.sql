USE [TaskManagementTool_Test]

DROP PROCEDURE IF EXISTS sp_create_AspNetUserClaims;

GO
CREATE PROCEDURE sp_create_AspNetUserClaims AS
BEGIN
	SET ANSI_NULLS ON

	SET QUOTED_IDENTIFIER ON

	CREATE TABLE [dbo].[AspNetUserClaims] (
		[Id]         INT            IDENTITY (1, 1) NOT NULL,
		[UserId]     NVARCHAR (450) NOT NULL,
		[ClaimType]  NVARCHAR (MAX) NULL,
		[ClaimValue] NVARCHAR (MAX) NULL
	);

	CREATE NONCLUSTERED INDEX [IX_AspNetUserClaims_UserId]
		ON [dbo].[AspNetUserClaims]([UserId] ASC);

	ALTER TABLE [dbo].[AspNetUserClaims]
		ADD CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED ([Id] ASC);

	ALTER TABLE [dbo].[AspNetUserClaims]
		ADD CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE;
END

