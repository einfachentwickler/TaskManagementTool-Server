USE [TaskManagementTool_Test]
GO

/****** Object:  Table [dbo].[Todos]    Script Date: 28.09.2022 15:16:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Todos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Content] [nvarchar](max) NULL,
	[IsCompleted] [bit] NOT NULL,
	[Importance] [int] NOT NULL,
	[CreatorId] [nvarchar](450) NULL,
 CONSTRAINT [PK_Todos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Todos]  WITH CHECK ADD  CONSTRAINT [FK_Todos_AspNetUsers_CreatorId] FOREIGN KEY([CreatorId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO

ALTER TABLE [dbo].[Todos] CHECK CONSTRAINT [FK_Todos_AspNetUsers_CreatorId]
GO


