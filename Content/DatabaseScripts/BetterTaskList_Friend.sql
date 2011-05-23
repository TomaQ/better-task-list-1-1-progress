USE [BetterTaskList]
GO

/****** Object:  Table [dbo].[BetterTaskList_Friend]    Script Date: 05/22/2011 22:08:19 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BetterTaskList_Friend](
	[UserId] [uniqueidentifier] NOT NULL,
	[AreFriends] [bit] NOT NULL,
	[FriendshipId] [bigint] IDENTITY(1,1) NOT NULL,
	[FriendUserId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_BetterTaskList_Friend] PRIMARY KEY CLUSTERED 
(
	[FriendshipId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[BetterTaskList_Friend] ADD  CONSTRAINT [DF_BetterTaskList_Friend_AreFriends]  DEFAULT ((0)) FOR [AreFriends]
GO

