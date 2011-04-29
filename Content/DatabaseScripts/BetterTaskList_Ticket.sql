USE [BetterTaskList]
GO

/****** Object:  Table [dbo].[BetterTaskList_Ticket]    Script Date: 04/28/2011 16:05:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BetterTaskList_Ticket](
	[TicketId] [bigint] IDENTITY(1,1) NOT NULL,
	[TicketTags] [nvarchar](500) NOT NULL,
	[TicketStatus] [nvarchar](20) NOT NULL,
	[TicketPriority] [nvarchar](7) NOT NULL,
	[TicketSubject] [nvarchar](250) NOT NULL,
	[TicketDueDate] [smalldatetime] NOT NULL,
	[TicketDescription] [ntext] NOT NULL,
	[TicketLastUpdated] [smalldatetime] NOT NULL,
	[TicketCreatorUserId] [uniqueidentifier] NOT NULL,
	[TicketStartTimeStamp] [smalldatetime] NOT NULL,
	[TicketOwnersEmailList] [nvarchar](1500) NOT NULL,
	[TicketFinishTimeStamp] [smalldatetime] NOT NULL,
	[TicketResolutionDetails] [ntext] NOT NULL,
	[TicketEmailNotificationList] [nvarchar](1500) NOT NULL,
 CONSTRAINT [PK_BetterTaskList_Tickets] PRIMARY KEY CLUSTERED 
(
	[TicketId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

