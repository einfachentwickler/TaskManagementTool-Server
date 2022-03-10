USE [TaskManagementTool_Test]

DROP PROCEDURE IF EXISTS sp_create_AspNetUsers;

GO
CREATE PROCEDURE sp_create_AspNetUsers AS
	BEGIN
		SET ANSI_NULLS ON

		SET QUOTED_IDENTIFIER ON

		CREATE TABLE [dbo].[AspNetUsers] (
			[Id]                   NVARCHAR (450)     NOT NULL,
			[FirstName]            NVARCHAR (MAX)     NULL,
			[LastName]             NVARCHAR (MAX)     NULL,
			[Age]                  INT                NOT NULL,
			[IsBlocked]            BIT                NOT NULL,
			[Role]                 NVARCHAR (MAX)     NULL,
			[UserName]             NVARCHAR (256)     NULL,
			[NormalizedUserName]   NVARCHAR (256)     NULL,
			[Email]                NVARCHAR (256)     NULL,
			[NormalizedEmail]      NVARCHAR (256)     NULL,
			[EmailConfirmed]       BIT                NOT NULL,
			[PasswordHash]         NVARCHAR (MAX)     NULL,
			[SecurityStamp]        NVARCHAR (MAX)     NULL,
			[ConcurrencyStamp]     NVARCHAR (MAX)     NULL,
			[PhoneNumber]          NVARCHAR (MAX)     NULL,
			[PhoneNumberConfirmed] BIT                NOT NULL,
			[TwoFactorEnabled]     BIT                NOT NULL,
			[LockoutEnd]           DATETIMEOFFSET (7) NULL,
			[LockoutEnabled]       BIT                NOT NULL,
			[AccessFailedCount]    INT                NOT NULL
		);


		CREATE NONCLUSTERED INDEX [EmailIndex]
			ON [dbo].[AspNetUsers]([NormalizedEmail] ASC);


		CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex]
			ON [dbo].[AspNetUsers]([NormalizedUserName] ASC) WHERE ([NormalizedUserName] IS NOT NULL);

		ALTER TABLE [dbo].[AspNetUsers]
			ADD CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED ([Id] ASC);
	END