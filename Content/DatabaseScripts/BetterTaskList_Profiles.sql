USE [BetterTaskList]
GO

/****** Object:  Table [dbo].[BetterTaskList_Profiles]    Script Date: 04/28/2011 15:59:00 ******/
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
 CONSTRAINT [PK_BetterTaskList_Profile] PRIMARY KEY CLUSTERED 
(
	[ProfileId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[BetterTaskList_Profile]  WITH CHECK ADD  CONSTRAINT [FK_BetterTaskList_Profile_aspnet_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO

ALTER TABLE [dbo].[BetterTaskList_Profile] CHECK CONSTRAINT [FK_BetterTaskList_Profile_aspnet_Users]
GO

