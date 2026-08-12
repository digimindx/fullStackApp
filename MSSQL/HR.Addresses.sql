USE [Noc]
GO

/****** Object:  Table [HR].[EmployeeAddresses]    Script Date: 8/12/2026 2:06:19 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [HR].[EmployeeAddresses](
	[AddressID] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeID] [int] NOT NULL,
	[AddressType] [char](1) NOT NULL,
	[CityCode] [int] NULL,
	[CountryCode] [int] NULL,
	[AddressText] [nvarchar](200) NULL,
	[NearestPoliceStation] [nvarchar](100) NULL,
	[PhoneNumber] [nvarchar](20) NULL,
	[MobileNumber] [nvarchar](20) NULL,
	[Email] [nvarchar](100) NULL,
	[IsPrimary] [bit] NOT NULL,
	[ValidFrom] [date] NOT NULL,
	[ValidTo] [date] NULL,
 CONSTRAINT [PK_EmployeeAddresses] PRIMARY KEY CLUSTERED 
(
	[AddressID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [HR].[EmployeeAddresses] ADD  CONSTRAINT [DF_EmployeeAddresses_IsPrimary]  DEFAULT ((0)) FOR [IsPrimary]
GO

ALTER TABLE [HR].[EmployeeAddresses] ADD  CONSTRAINT [DF_EmployeeAddresses_ValidFrom]  DEFAULT (getdate()) FOR [ValidFrom]
GO

ALTER TABLE [HR].[EmployeeAddresses]  WITH CHECK ADD  CONSTRAINT [FK_EmployeeAddresses_Employees] FOREIGN KEY([EmployeeID])
REFERENCES [HR].[Employees] ([EmployeeID])
ON DELETE CASCADE
GO

ALTER TABLE [HR].[EmployeeAddresses] CHECK CONSTRAINT [FK_EmployeeAddresses_Employees]
GO

ALTER TABLE [HR].[EmployeeAddresses]  WITH CHECK ADD  CONSTRAINT [CK_EmployeeAddresses_AddressType] CHECK  (([AddressType]='C' OR [AddressType]='P'))
GO

ALTER TABLE [HR].[EmployeeAddresses] CHECK CONSTRAINT [CK_EmployeeAddresses_AddressType]
GO

ALTER TABLE [HR].[EmployeeAddresses]  WITH CHECK ADD  CONSTRAINT [CK_EmployeeAddresses_DateRange] CHECK  (([ValidTo] IS NULL OR [ValidTo]>=[ValidFrom]))
GO

ALTER TABLE [HR].[EmployeeAddresses] CHECK CONSTRAINT [CK_EmployeeAddresses_DateRange]
GO

