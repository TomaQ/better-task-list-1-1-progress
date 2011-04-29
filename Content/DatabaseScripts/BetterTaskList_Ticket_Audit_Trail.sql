USE [BetterTaskList]
GO

/****** Object:  Table [dbo].[BetterTaskList_Ticket_Audit_Trail]    Script Date: 04/28/2011 15:59:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BetterTaskList_Ticket_Audit_Trail](
	[AuditTrailId] [bigint] IDENTITY(1,1) NOT NULL,
	[AuditEventType] [nvarchar](50) NOT NULL,
	[AuditEventCategory] [nvarchar](50) NOT NULL,
	[AuditEventTimeStamp] [smalldatetime] NOT NULL,
	[AuditEventDescription] [nvarchar](500) NOT NULL,
	[AuditEventForeingKey] [bigint] NOT NULL,
 CONSTRAINT [PK_BetterTaskList_Ticket_Audit_Trail] PRIMARY KEY CLUSTERED 
(
	[AuditTrailId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

