USE [BetterTaskList_Progress]
GO

/****** Object:  Table [dbo].[BetterTaskList_CoWorker]    Script Date: 05/23/2011 07:28:44 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BetterTaskList_CoWorker](
	[UserId] [uniqueidentifier] NOT NULL,
	[AreFriends] [bit] NOT NULL,
	[CoWorkerId] [bigint] IDENTITY(1,1) NOT NULL,
	[CoWorkerUserId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_BetterTaskList_CoWorker] PRIMARY KEY CLUSTERED 
(
	[CoWorkerId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

