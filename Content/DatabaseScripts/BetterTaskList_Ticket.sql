USE [BetterTaskList]
GO

/****** Object:  Table [dbo].[BetterTaskList_Ticket]    Script Date: 05/11/2011 07:34:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BetterTaskList_Ticket](
	[TicketId] [bigint] IDENTITY(1,1) NOT NULL,
	[TicketTags] [nvarchar](500) NULL,
	[TicketStatus] [nvarchar](20) NULL,
	[TicketPriority] [nvarchar](7) NULL,
	[TicketSubject] [nvarchar](250) NULL,
	[TicketDueDate] [smalldatetime] NULL,
	[TicketDescription] [ntext] NULL,
	[TicketLastUpdated] [smalldatetime] NULL,
	[TicketCreatorUserId] [uniqueidentifier] NOT NULL,
	[TicketStartTimeStamp] [smalldatetime] NULL,
	[TicketOwnersEmailList] [nvarchar](1500) NULL,
	[TicketFinishTimeStamp] [smalldatetime] NULL,
	[TicketResolutionDetails] [ntext] NULL,
	[TicketResolvedByUserId] [uniqueidentifier] NULL,
	[TicketEmailNotificationList] [nvarchar](1500) NULL,
 CONSTRAINT [PK_BetterTaskList_Tickets] PRIMARY KEY CLUSTERED 
(
	[TicketId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

