-- ======================================================================================
-- Create Configure database template for megastore.
-- Use Ctrl+Shift+M in SSMS to show Template Parameters dialog
-- ======================================================================================
SET ANSI_NULLS ON
GO
 
SET QUOTED_IDENTIFIER ON
GO

CREATE USER sales_user
	FOR LOGIN sales_user_<environment_name, sysname, environment_name>
	WITH DEFAULT_SCHEMA = dbo
GO

-- Add user to the database owner role
EXEC sp_addrolemember N'db_owner', N'sales_user'
GO
 
CREATE TABLE [dbo].[Sale](
	[SaleID] [bigint] IDENTITY(1001,1) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[Description] [varchar](100) NOT NULL
) ON [PRIMARY]
GO