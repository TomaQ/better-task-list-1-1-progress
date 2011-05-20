USE [BetterTaskList_Progress]
GO

/****** Object:  Table [dbo].[BetterTaskList_Stream]    Script Date: 05/20/2011 09:07:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BetterTaskList_Stream](
	[StreamId] [bigint] IDENTITY(1,1) NOT NULL,
	[StreamType] [nvarchar](25) NOT NULL,
	[StreamDetails] [nvarchar](500) NOT NULL,
	[StreamLikesCount] [int] NOT NULL,
	[StreamCreatorUserId] [uniqueidentifier] NOT NULL,
	[StreamCommentCount] [int] NOT NULL,
	[StreamCreatorFullName] [nvarchar](250) NOT NULL,
	[StreamCreatedTimeStamp] [smalldatetime] NOT NULL,
	[StreamLastUpdatedTimeStamp] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_BetterTaskList_Stream] PRIMARY KEY CLUSTERED 
(
	[StreamId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[BetterTaskList_Stream] ADD  CONSTRAINT [DF_BetterTaskList_Stream_StreamLikesCount]  DEFAULT ((0)) FOR [StreamLikesCount]
GO

ALTER TABLE [dbo].[BetterTaskList_Stream] ADD  CONSTRAINT [DF_BetterTaskList_Stream_StreamCommentCount]  DEFAULT ((0)) FOR [StreamCommentCount]
GO

