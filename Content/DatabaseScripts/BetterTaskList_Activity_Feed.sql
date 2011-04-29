USE [BetterTaskList]
GO

/****** Object:  Table [dbo].[BetterTaskList_Activity_Feed]    Script Date: 04/28/2011 15:58:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BetterTaskList_Activity_Feed](
	[FeedId] [bigint] IDENTITY(1,1) NOT NULL,
	[FeedActionDescription] [nvarchar](500) NOT NULL,
	[FeedActionCreatorUserId] [uniqueidentifier] NOT NULL,
	[FeedActionTimeStamp] [smalldatetime] NOT NULL,
	[FeedActionDetails] [nvarchar](1000) NOT NULL,
	[FeedMoreUrl] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_BetterTaskList_Activity_Feed] PRIMARY KEY CLUSTERED 
(
	[FeedId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

