USE [BetterTaskList]
GO

/****** Object:  Table [dbo].[BetterTaskList_Profile]    Script Date: 05/09/2011 17:15:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BetterTaskList_Profile](
	[UserId] [uniqueidentifier] NOT NULL,
	[ProfileId] [bigint] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](60) NULL,
	[LastName] [nvarchar](60) NULL,
	[FullName] [nvarchar](256) NULL,
	[TimeZone] [nvarchar](50) NULL,
	[PictureName] [nvarchar](50) NULL,
	[CellPhoneNumber] [nvarchar](50) NULL,
	[WorkPhoneNumber] [nvarchar](50) NULL,
 CONSTRAINT [PK_BetterTaskList_Profiles] PRIMARY KEY CLUSTERED 
(
	[ProfileId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

