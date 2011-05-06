USE [BetterTaskList]
GO

/****** Object:  Table [dbo].[BetterTaskList_Ticket_Comment]    Script Date: 05/06/2011 11:46:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BetterTaskList_Ticket_Comment](
	[TicketId] [bigint] NOT NULL,
	[TicketCommentId] [bigint] IDENTITY(1,1) NOT NULL,
	[TicketCommentKarma] [int] NOT NULL,
	[TicketCommentDetails] [nvarchar](1500) NOT NULL,
	[TicketCommentParentId] [bigint] NOT NULL,
	[TicketCommentTimeStamp] [smalldatetime] NOT NULL,
	[TicketCommentSubmitterUserId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_BetterTaskList_Ticket_Comment] PRIMARY KEY CLUSTERED 
(
	[TicketCommentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[BetterTaskList_Ticket_Comment] ADD  CONSTRAINT [DF_BetterTaskList_Ticket_Comment_TicketCommentKarma]  DEFAULT ((0)) FOR [TicketCommentKarma]
GO

ALTER TABLE [dbo].[BetterTaskList_Ticket_Comment] ADD  CONSTRAINT [DF_BetterTaskList_Ticket_Comment_TicketCommentParentId]  DEFAULT ((0)) FOR [TicketCommentParentId]
GO

