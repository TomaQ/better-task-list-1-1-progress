USE [BetterTaskList_Progress]
GO

/****** Object:  Table [dbo].[BetterTaskList_Stream_Comment]    Script Date: 05/20/2011 10:52:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BetterTaskList_Stream_Comment](
	[StreamId] [bigint] NOT NULL,
	[StreamCommentId] [bigint] IDENTITY(1,1) NOT NULL,
	[StreamCommentDetails] [nvarchar](500) NOT NULL,
	[StreamCommentParentId] [bigint] NOT NULL,
	[StreamCommentTimeStamp] [smalldatetime] NOT NULL,
	[StreamCommentLikesCount] [int] NOT NULL,
	[StreamCommentSubmitterUserId] [uniqueidentifier] NOT NULL,
	[StreamCommentSubmitterFullName] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_BetterTaskList_Stream_Comment] PRIMARY KEY CLUSTERED 
(
	[StreamCommentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[BetterTaskList_Stream_Comment] ADD  CONSTRAINT [DF_BetterTaskList_Stream_Comment_StreamCommentParentId]  DEFAULT ((0)) FOR [StreamCommentParentId]
GO

ALTER TABLE [dbo].[BetterTaskList_Stream_Comment] ADD  CONSTRAINT [DF_BetterTaskList_Stream_Comment_StreamCommentLikesCount]  DEFAULT ((0)) FOR [StreamCommentLikesCount]
GO

