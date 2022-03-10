USE TaskManagementTool_Test;

DROP PROCEDURE IF EXISTS sp_create_todos;

GO
CREATE PROCEDURE sp_create_todos AS
	BEGIN
		SET ANSI_NULLS ON

		SET QUOTED_IDENTIFIER ON

		CREATE TABLE [dbo].[Todos] (
			[Id]          INT            IDENTITY (1, 1) NOT NULL,
			[Name]        NVARCHAR (MAX) NULL,
			[Content]     NVARCHAR (MAX) NULL,
			[IsCompleted] BIT            NOT NULL,
			[Importance]  INT            NOT NULL,
			[CreatorId]   NVARCHAR (450) NULL
		);

		CREATE NONCLUSTERED INDEX [IX_Todos_CreatorId]
			ON [dbo].[Todos]([CreatorId] ASC);

		ALTER TABLE [dbo].[Todos]
			ADD CONSTRAINT [PK_Todos] PRIMARY KEY CLUSTERED ([Id] ASC);

		ALTER TABLE [dbo].[Todos]
			ADD CONSTRAINT [FK_Todos_AspNetUsers_CreatorId] FOREIGN KEY ([CreatorId]) REFERENCES [dbo].[AspNetUsers] ([Id]);
END


