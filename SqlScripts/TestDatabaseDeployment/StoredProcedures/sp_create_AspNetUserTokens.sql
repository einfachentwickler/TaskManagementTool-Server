USE [TaskManagementTool_Test]
GO

DROP PROCEDURE IF EXISTS sp_create_AspNetUserTokens;

GO
CREATE PROCEDURE sp_create_AspNetUserTokens AS
	BEGIN
		SET ANSI_NULLS ON

		SET QUOTED_IDENTIFIER ON

		CREATE TABLE [dbo].[AspNetUserTokens] (
			[UserId]        NVARCHAR (450) NOT NULL,
			[LoginProvider] NVARCHAR (450) NOT NULL,
			[Name]          NVARCHAR (450) NOT NULL,
			[Value]         NVARCHAR (MAX) NULL
		);
	END