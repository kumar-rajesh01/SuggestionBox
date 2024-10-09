USE [SuggestionBox]
GO
/****** Object:  Table [dbo].[User]    Script Date: 10/9/2024 1:23:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[Phone] [varchar](15) NULL,
	[Password] [varchar](50) NOT NULL,
	[Role] [varchar](15) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateUpdated] [datetime] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[User] ON 
GO
INSERT [dbo].[User] ([UserId], [Name], [Email], [Phone], [Password], [Role], [IsActive], [DateCreated], [DateUpdated]) VALUES (1, N'Rajesh', N'rajesh.kumar@ociustechnologies.com', N'1236458790', N'+oNtrirc+iM=', N'Admin', 1, CAST(N'2024-07-12T15:44:41.663' AS DateTime), CAST(N'2024-07-12T15:44:41.663' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[User] OFF
GO
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_DateUpdated]  DEFAULT (getdate()) FOR [DateUpdated]
GO
/****** Object:  Table [dbo].[Suggestion]    Script Date: 10/9/2024 1:23:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Suggestion](
	[SuggestionId] [bigint] IDENTITY(1,1) NOT NULL,
	[Text] [nvarchar](max) NOT NULL,
	[Ip] [varchar](50) NOT NULL,
	[File] [nvarchar](max) NULL,
	[Comment] [nvarchar](max) NULL,
	[IsReviewed] [bit] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateUpdated] [datetime] NOT NULL,
	[FileName] [nvarchar](500) NULL,
 CONSTRAINT [PK_Suggestion] PRIMARY KEY CLUSTERED 
(
	[SuggestionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Suggestion] ADD  CONSTRAINT [DF_Suggestion_IsReviewed]  DEFAULT ((0)) FOR [IsReviewed]
GO
ALTER TABLE [dbo].[Suggestion] ADD  CONSTRAINT [DF_Suggestion_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[Suggestion] ADD  CONSTRAINT [DF_Suggestion_DateUpdated]  DEFAULT (getdate()) FOR [DateUpdated]
GO
