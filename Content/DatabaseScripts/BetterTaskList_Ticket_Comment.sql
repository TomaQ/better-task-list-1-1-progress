USE [BetterTaskList]
GO

/****** Object:  Table [dbo].[BetterTaskList_Ticket_Comment]    Script Date: 05/04/2011 09:41:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BetterTaskList_Ticket_Comment](
	[TicketId] [bigint] NOT NULL,
	[TicketCommentId] [bigint] IDENTITY(1,1) NOT NULL,
	[TicketCommentDetails] [nvarchar](1500) NOT NULL,
	[TicketCommentTimeStamp] [smalldatetime] NOT NULL,
	[TicketCommentSubmitterUserId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_BetterTaskList_Ticket_Comment] PRIMARY KEY CLUSTERED 
(
	[TicketCommentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

