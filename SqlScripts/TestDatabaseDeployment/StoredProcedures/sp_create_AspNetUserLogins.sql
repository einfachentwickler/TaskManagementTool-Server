USE [TaskManagementTool_Test]

DROP PROCEDURE IF EXISTS sp_create_AspNetUserLogins;

GO
CREATE PROCEDURE sp_create_AspNetUserLogins AS
BEGIN
	SET ANSI_NULLS ON

	SET QUOTED_IDENTIFIER ON

	CREATE TABLE [dbo].[AspNetUserLogins] (
		[LoginProvider]       NVARCHAR (450) NOT NULL,
		[ProviderKey]         NVARCHAR (450) NOT NULL,
		[ProviderDisplayName] NVARCHAR (MAX) NULL,
		[UserId]              NVARCHAR (450) NOT NULL
	);


	CREATE NONCLUSTERED INDEX [IX_AspNetUserLogins_UserId]
		ON [dbo].[AspNetUserLogins]([UserId] ASC);

	ALTER TABLE [dbo].[AspNetUserLogins]
		ADD CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED ([LoginProvider] ASC, [ProviderKey] ASC);

	ALTER TABLE [dbo].[AspNetUserLogins]
		ADD CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE;
END


