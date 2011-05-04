USE [BetterTaskList]
GO

/****** Object:  Table [dbo].[BetterTaskList_Profile]    Script Date: 05/03/2011 19:38:49 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BetterTaskList_Profile](
	[UserId] [uniqueidentifier] NOT NULL,
	[ProfileId] [bigint] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](60) NOT NULL,
	[LastName] [nvarchar](60) NOT NULL,
	[FullName] [nvarchar](256) NOT NULL,
	[TimeZone] [nvarchar](50) NOT NULL,
	[CellPhoneNumber] [nvarchar](50) NULL,
	[WorkPhoneNumber] [nvarchar](50) NULL,
 CONSTRAINT [PK_BetterTaskList_Profiles] PRIMARY KEY CLUSTERED 
(
	[ProfileId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[BetterTaskList_Profile]  WITH CHECK ADD  CONSTRAINT [FK_BetterTaskList_Profiles_aspnet_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO

ALTER TABLE [dbo].[BetterTaskList_Profile] CHECK CONSTRAINT [FK_BetterTaskList_Profiles_aspnet_Users]
GO

