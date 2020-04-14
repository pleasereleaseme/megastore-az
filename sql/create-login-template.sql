-- ======================================================================================
-- Create SQL Login template for megastore.
-- Use Ctrl+Shift+M in SSMS to show Template Parameters dialog
-- ======================================================================================

Use master
GO

CREATE LOGIN sales_user_<environment_name, sysname, environment_name>
	WITH PASSWORD = '<password, sysname, Change_Password>' 
GO
