-- Create a new database called 'FMASolutionsWebsiteDB' if it doesn't exist already
USE master
GO
IF NOT EXISTS (
    SELECT name
        FROM sys.databases
        WHERE name = N'FMASolutionsWebsiteDB'
)
CREATE DATABASE FMASolutionsWebsiteDB
GO

USE FMASolutionsWebsiteDB
GO