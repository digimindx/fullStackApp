USE [Noc]
GO

/****** Object:  Table [HR].[EmployeeAccounts]    Script Date: 8/12/2026 2:43:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [HR].[EmployeeAccounts](
	[AccountID] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeID] [int] NOT NULL,
	[Gender] [nchar](1) NULL,
	[DateOfBirth] [date] NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[PasswordHash] [nvarchar](1000) NOT NULL,
	[PasswordSalt] [nvarchar](1000) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastLoginDate] [datetime] NULL,
	[IsActive] [bit] NOT NULL,
	[IsLocked] [bit] NOT NULL,
	[LockedUntil] [datetime] NULL,
	[FailedLoginAttempts] [int] NOT NULL,
	[RefreshToken] [nvarchar](500) NULL,
	[RefreshTokenExpiryTime] [datetime] NULL,
	[CreatedBy] [nvarchar](128) NOT NULL,
	[LastModifiedDate] [datetime] NULL,
	[LastModifiedBy] [nvarchar](128) NULL,
 CONSTRAINT [PK_EmployeeAccounts] PRIMARY KEY CLUSTERED 
(
	[AccountID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_EmployeeAccounts_Email] UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_EmployeeAccounts_Username] UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [HR].[EmployeeAccounts] ADD  CONSTRAINT [DF_EmployeeAccounts_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [HR].[EmployeeAccounts] ADD  CONSTRAINT [DF_EmployeeAccounts_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO

ALTER TABLE [HR].[EmployeeAccounts] ADD  CONSTRAINT [DF_EmployeeAccounts_IsLocked]  DEFAULT ((0)) FOR [IsLocked]
GO

ALTER TABLE [HR].[EmployeeAccounts] ADD  CONSTRAINT [DF_EmployeeAccounts_FailedLoginAttempts]  DEFAULT ((0)) FOR [FailedLoginAttempts]
GO

ALTER TABLE [HR].[EmployeeAccounts] ADD  CONSTRAINT [DF_EmployeeAccounts_CreatedBy]  DEFAULT (suser_sname()) FOR [CreatedBy]
GO

ALTER TABLE [HR].[EmployeeAccounts]  WITH CHECK ADD  CONSTRAINT [FK_EmployeeAccounts_Employees] FOREIGN KEY([EmployeeID])
REFERENCES [HR].[Employees] ([EmployeeID])
ON DELETE CASCADE
GO

ALTER TABLE [HR].[EmployeeAccounts] CHECK CONSTRAINT [FK_EmployeeAccounts_Employees]
GO

