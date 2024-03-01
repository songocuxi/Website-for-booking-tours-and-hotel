create database dbTravelAndHotel
USE [dbTravelAndHotel]
GO
/****** Object:  Table [dbo].[Account]    Script Date: 13/03/2023 10:46:14 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[AccountID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](max) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[Salt] [nvarchar](max) NOT NULL,
	[Fullname] [nvarchar](max) NOT NULL,
	[Phone] [nvarchar](10) NULL,
	[Avatar] [nvarchar](max) NULL,
	[RoleID] [int] NOT NULL,
 CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED 
(
	[AccountID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Booking]    Script Date: 13/03/2023 10:46:14 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Booking](
	[TourID] [int] NOT NULL,
	[CustomerID] [int] NOT NULL,
	[Date] [date] NOT NULL,
	[Pay] [bit] NOT NULL,
	[BookingID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_Booking] PRIMARY KEY CLUSTERED 
(
	[BookingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Hotel]    Script Date: 13/03/2023 10:46:14 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hotel](
	[HotelID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Phone] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Hotel] PRIMARY KEY CLUSTERED 
(
	[HotelID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HotelBooking]    Script Date: 13/03/2023 10:46:14 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HotelBooking](
	[HotelID] [int] NOT NULL,
	[TourID] [int] NOT NULL,
	[Date] [date] NOT NULL,
	[HBID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_HotelBooking] PRIMARY KEY CLUSTERED 
(
	[HBID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Place]    Script Date: 13/03/2023 10:46:14 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Place](
	[PlaceID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Address] [nvarchar](max) NULL,
	[Image] [nvarchar](max) NULL,
 CONSTRAINT [PK_Place] PRIMARY KEY CLUSTERED 
(
	[PlaceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 13/03/2023 10:46:14 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[RoleID] [int] IDENTITY(1,1) NOT NULL,
	[Role] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tour]    Script Date: 13/03/2023 10:46:14 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tour](
	[TourID] [int] IDENTITY(1,1) NOT NULL,
	[StartTime] [date] NOT NULL,
	[EndTime] [date] NOT NULL,
	[Cost] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[PlaceID] [int] NOT NULL,
 CONSTRAINT [PK_Tour] PRIMARY KEY CLUSTERED 
(
	[TourID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Account] ON 
GO
INSERT [dbo].[Account] ([AccountID], [Username], [Password], [Salt], [Fullname], [Phone], [Avatar], [RoleID]) VALUES (1, N'Admin', N'123456', N'123', N'Admin', N'0123456789', NULL, 1)
GO
SET IDENTITY_INSERT [dbo].[Account] OFF
GO
SET IDENTITY_INSERT [dbo].[Booking] ON 
GO
INSERT [dbo].[Booking] ([TourID], [CustomerID], [Date], [Pay], [BookingID]) VALUES (3, 1, CAST(N'2023-03-12' AS Date), 1, 1)
GO
SET IDENTITY_INSERT [dbo].[Booking] OFF
GO
SET IDENTITY_INSERT [dbo].[Hotel] ON 
GO
INSERT [dbo].[Hotel] ([HotelID], [Name], [Phone]) VALUES (1, N'Mường Thanh', N'0123456789')
GO
SET IDENTITY_INSERT [dbo].[Hotel] OFF
GO
SET IDENTITY_INSERT [dbo].[HotelBooking] ON 
GO
INSERT [dbo].[HotelBooking] ([HotelID], [TourID], [Date], [HBID]) VALUES (1, 3, CAST(N'2023-03-12' AS Date), 1)
GO
SET IDENTITY_INSERT [dbo].[HotelBooking] OFF
GO
SET IDENTITY_INSERT [dbo].[Place] ON 
GO
INSERT [dbo].[Place] ([PlaceID], [Name], [Address], [Image]) VALUES (1, N'Chợ nổi', N'Cần ', NULL)
GO
SET IDENTITY_INSERT [dbo].[Place] OFF
GO
SET IDENTITY_INSERT [dbo].[Role] ON 
GO
INSERT [dbo].[Role] ([RoleID], [Role], [Description]) VALUES (1, N'Admin', N'Quản trị viên')
GO
INSERT [dbo].[Role] ([RoleID], [Role], [Description]) VALUES (2, N'Customer', N'Khách hàng yêu quý')
GO
SET IDENTITY_INSERT [dbo].[Role] OFF
GO
SET IDENTITY_INSERT [dbo].[Tour] ON 
GO
INSERT [dbo].[Tour] ([TourID], [StartTime], [EndTime], [Cost], [Description], [PlaceID]) VALUES (3, CAST(N'2023-03-12' AS Date), CAST(N'2023-03-12' AS Date), N'20000', N'Cần Thơ', 1)
GO
SET IDENTITY_INSERT [dbo].[Tour] OFF
GO
ALTER TABLE [dbo].[Account]  WITH CHECK ADD  CONSTRAINT [FK_Account_Role] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Role] ([RoleID])
GO
ALTER TABLE [dbo].[Account] CHECK CONSTRAINT [FK_Account_Role]
GO
ALTER TABLE [dbo].[Booking]  WITH CHECK ADD  CONSTRAINT [FK_Booking_Account] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Account] ([AccountID])
GO
ALTER TABLE [dbo].[Booking] CHECK CONSTRAINT [FK_Booking_Account]
GO
ALTER TABLE [dbo].[Booking]  WITH CHECK ADD  CONSTRAINT [FK_Booking_Tour] FOREIGN KEY([TourID])
REFERENCES [dbo].[Tour] ([TourID])
GO
ALTER TABLE [dbo].[Booking] CHECK CONSTRAINT [FK_Booking_Tour]
GO
ALTER TABLE [dbo].[HotelBooking]  WITH CHECK ADD  CONSTRAINT [FK_HotelBooking_Hotel] FOREIGN KEY([HotelID])
REFERENCES [dbo].[Hotel] ([HotelID])
GO
ALTER TABLE [dbo].[HotelBooking] CHECK CONSTRAINT [FK_HotelBooking_Hotel]
GO
ALTER TABLE [dbo].[HotelBooking]  WITH CHECK ADD  CONSTRAINT [FK_HotelBooking_Tour] FOREIGN KEY([TourID])
REFERENCES [dbo].[Tour] ([TourID])
GO
ALTER TABLE [dbo].[HotelBooking] CHECK CONSTRAINT [FK_HotelBooking_Tour]
GO
ALTER TABLE [dbo].[Tour]  WITH CHECK ADD  CONSTRAINT [FK_Tour_Place] FOREIGN KEY([PlaceID])
REFERENCES [dbo].[Place] ([PlaceID])
GO
ALTER TABLE [dbo].[Tour] CHECK CONSTRAINT [FK_Tour_Place]
GO
