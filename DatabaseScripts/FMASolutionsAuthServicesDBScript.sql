-- Create a new database called 'FMASolutionsAuthServicesDB' if it does not exist already
USE master
GO
IF NOT EXISTS (
    SELECT name
    FROM sys.databases
    WHERE name = N'FMASolutionsAuthServicesDB'
)
CREATE DATABASE FMASolutionsAuthServicesDB
GO
USE FMASolutionsAuthServicesDB
GO
/*
DROP TABLES IN REQUIRED ORDER IF NEEDED:
DROP TABLE UserAuth
DROP TABLE ApprovedApps
DROP TABLE Tokens
DROP TABLE AuthTypes
DROP TABLE UserRoles
DROP TABLE Users
*/
CREATE TABLE UserRoles
(
    UserRoleID INT IDENTITY(1,1) PRIMARY KEY,
    UserRoleName VARCHAR(50) NOT NULL UNIQUE
)
GO
CREATE TABLE AuthTypes
(
    AuthTypeID INT IDENTITY(1,1) PRIMARY KEY,
    AuthTypeName VARCHAR(50) NOT NULL UNIQUE
)
GO
CREATE TABLE ApprovedApps
(
    ApprovedAppID INT IDENTITY(1,1) PRIMARY KEY,
    AppName VARCHAR(50) NOT NULL UNIQUE,
    AppKey VARCHAR(32) NOT NULL,
    AppPassword VARCHAR(MAX) NOT NULL
)
GO
CREATE TABLE Users
(
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    AuthTypeID INT FOREIGN KEY REFERENCES AuthTypes(AuthTypeID),
    UserRoleID INT FOREIGN KEY REFERENCES UserRoles(UserRoleID),
    EmailAddress VARCHAR(200) NOT NULL UNIQUE,
    KnownAs VARCHAR(100) NOT NULL UNIQUE,
    Firstname VARCHAR(100) NULL,
    Surname VARCHAR(100) NULL,
    MobileNumber VARCHAR(30) NULL,
    AddressLine1 VARCHAR(100) NULL,
    AddressLine2 VARCHAR(100) NULL,
    AddressLine3 VARCHAR(100) NULL,
    City VARCHAR(100) NULL,
    Country VARCHAR(100) NULL,
    PostCode VARCHAR(100) NULL
)
GO
CREATE TABLE UserAuth
(
    UserAuthID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT FOREIGN KEY REFERENCES Users(UserID) UNIQUE,
    UserKey VARCHAR(32) NOT NULL,
    [Password] VARCHAR(MAX) NOT NULL
)
CREATE TABLE Tokens
(
    TokenID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT FOREIGN KEY REFERENCES Users(UserID),
    ApprovedAppID INT FOREIGN KEY REFERENCES ApprovedApps(ApprovedAppID),
    TokenKey VARCHAR(32) NOT NULL,
    TokenIssueDate DATETIME NOT NULL,
    TokenExpiryDate DATETIME NOT NULL
)
GO
INSERT INTO AuthTypes
    (AuthTypeName)
VALUES('FMASolutions'),
    ('Google'),
    ('GitHub')
INSERT INTO UserRoles
    (UserRoleName)
VALUES('FMASolutionsAdmin'),
    ('FMASolutionsStandard'),
    ('External'),
    ('ShopBro')
INSERT INTO USERS
    (AuthTypeID,UserRoleID,EmailAddress,KnownAs,Firstname,Surname,MobileNumber,AddressLine1,AddressLine2,AddressLine3,City,Country,PostCode)
VALUES( (Select AuthTypeID
        From AuthTypes
        Where AuthTypeName = 'FMASolutions')
    , (Select UserRoleID
        FROM UserRoles
        WHERE UserRoleName = 'FMASolutionsAdmin')
    , 'faisal@ahmedmail.info'
    , 'Fes'
    , 'Faisal'
    , 'Ahmed'
    , '07532282222'
    , 'Flat 1'
    , '123 Your Street'
    , 'Your Suburb'
    , 'Your City'
    , 'Your Country'
    , 'NG1 2AB'
)
INSERT INTO UserAuth
    (UserID, UserKey, [Password])
VALUES( (SELECT UserID
        FROM Users
        WHERE EmailAddress = 'faisal@ahmedmail.info')
    , '543F29EA6A46409E8914FF84A38E9B6D'
    , 'fwTOiwkLHDvI9IlQ+WCr4w=='
)
INSERT INTO ApprovedApps
    (AppName,AppKey,AppPassword)
VALUES('FMASolutions', '336BA3CAF5E843629A0167057C9732D7', 'I/8gDxN9tuSF1L0JLgGgNA==')
SELECT *
FROM ApprovedApps
SELECT *
FROM Tokens
SELECT *
FROM AuthTypes
SELECT *
FROM UserRoles
SELECT *
FROM Users
SELECT *
FROM UserAuth
