USE [Noc]
GO

/****** Object:  Table [HR].[EmployeeBankAccounts]    Script Date: 8/12/2026 2:33:13 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [HR].[EmployeeBankAccounts](
	[BankAccountID] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeID] [int] NOT NULL,
	[BankCode] [int] NULL,
	[BranchCode] [int] NULL,
	[CityCode] [int] NULL,
	[AccountNumber] [nvarchar](30) NOT NULL,
	[IBAN] [nvarchar](34) NULL,
	[BankName] [nvarchar](100) NULL,
	[BankAddress1] [nvarchar](100) NULL,
	[BankAddress2] [nvarchar](100) NULL,
	[CountryCode] [int] NULL,
	[IsActive] [bit] NOT NULL,
	[ValidFrom] [date] NOT NULL,
	[ValidTo] [date] NULL,
 CONSTRAINT [PK_EmployeeBankAccounts] PRIMARY KEY CLUSTERED 
(
	[BankAccountID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [HR].[EmployeeBankAccounts] ADD  CONSTRAINT [DF_EmployeeBankAccounts_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO

ALTER TABLE [HR].[EmployeeBankAccounts] ADD  CONSTRAINT [DF_EmployeeBankAccounts_ValidFrom]  DEFAULT (getdate()) FOR [ValidFrom]
GO

ALTER TABLE [HR].[EmployeeBankAccounts]  WITH CHECK ADD  CONSTRAINT [FK_EmployeeBankAccounts_Employees] FOREIGN KEY([EmployeeID])
REFERENCES [HR].[Employees] ([EmployeeID])
ON DELETE CASCADE
GO

ALTER TABLE [HR].[EmployeeBankAccounts] CHECK CONSTRAINT [FK_EmployeeBankAccounts_Employees]
GO

