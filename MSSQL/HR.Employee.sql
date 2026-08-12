USE [Noc]
GO

/****** Object:  Table [HR].[Employees]    Script Date: 8/12/2026 1:20:17 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [HR].[Employees](
	[EmployeeID] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeNumber] [int] NOT NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[LastName] [nvarchar](100) NOT NULL,
	[MotherName] [nvarchar](100) NULL,
	[Gender] [char](1) NULL,
	[DateOfBirth] [date] NOT NULL,
	[BirthPlace] [nvarchar](100) NULL,
	[IsBornAbroad] [bit] NOT NULL DEFAULT ((0)),
	[NationalityCode] [int] NULL,
	[HasDoubleNationality] [bit] NOT NULL DEFAULT ((0)),
	[MaritalStatus] [char](1) NULL,
	[SocialCategory] [char](1) NULL,
	[EntryDate] [datetime] NOT NULL DEFAULT (getdate()),
	[CreatedBy] [nvarchar](128) NOT NULL DEFAULT (suser_sname()),
	[LastModifiedDate] [datetime] NULL,
	[LastModifiedBy] [nvarchar](128) NULL,
	[IsActive] [bit] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_Employees] PRIMARY KEY CLUSTERED 
(
	[EmployeeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Employees_EmployeeNumber] UNIQUE NONCLUSTERED 
(
	[EmployeeNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [HR].[Employees]  WITH CHECK ADD  CONSTRAINT [CK_Employees_Gender] CHECK (([Gender] IS NULL OR [Gender] IN ('F', 'M')))
GO

ALTER TABLE [HR].[Employees] CHECK CONSTRAINT [CK_Employees_Gender]
GO

ALTER TABLE [HR].[Employees]  WITH CHECK ADD  CONSTRAINT [CK_Employees_MaritalStatus] CHECK ([MaritalStatus] IS NULL OR [MaritalStatus] IN ('S', 'M', 'D', 'W'));
GO

ALTER TABLE [HR].[Employees] CHECK CONSTRAINT [CK_Employees_MaritalStatus]
GO

