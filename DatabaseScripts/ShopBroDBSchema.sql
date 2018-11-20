-- Create a new database called 'ShopBroDB' if it doesn't exist already
USE master
GO
IF NOT EXISTS (
    SELECT name
FROM sys.databases
WHERE name = N'ShopBroDB'
)
CREATE DATABASE ShopBroDB
GO
USE ShopBroDB
GO
/*
DROP TABLES IN REQUIRED ORDER IF NEEDED:
*/
DROP TABLE InvoiceItems
DROP TABLE InvoiceHeaders
DROP TABLE DeliveryNoteItems
DROP TABLE DeliveryNotes
DROP TABLE OrderItems
DROP TABLE OrderHeaders
DROP TABLE OrderStatus
DROP TABLE InvoiceStatus
DROP TABLE CustomerAddresses
DROP TABLE Customers
DROP TABLE CustomerTypes
DROP TABLE AddressLocations
DROP TABLE CityAreas
DROP TABLE Cities
DROP TABLE Countries
DROP TABLE Items
DROP TABLE SubGroups
DROP TABLE ProductGroups
DROP TABLE AuditLogs
DROP TABLE AuditLogTypes
DROP PROCEDURE dbo.DeliverExistingItems 


CREATE TABLE ProductGroups(
    ProductGroupID INT IDENTITY(1,1) PRIMARY KEY,
    ProductGroupCode VARCHAR(7) NOT NULL UNIQUE,    
    ProductGroupName VARCHAR(50) NOT NULL,
    ProductGroupDescription VARCHAR(250) NOT NULL
)
GO
CREATE TABLE SubGroups(
    SubGroupID INT IDENTITY(1,1) PRIMARY KEY,
    SubGroupCode VARCHAR(7) NOT NULL UNIQUE,
    ProductGroupID INT FOREIGN KEY REFERENCES ProductGroups(ProductGroupID),    
    SubGroupName VARCHAR(100) NOT NULL,    
    SubGroupDescription VARCHAR(250) NOT NULL
)
CREATE TABLE Items(
    ItemID INT IDENTITY(1,1) PRIMARY KEY,
    ItemCode VARCHAR(7) NOT NULL UNIQUE,    
    SubGroupID INT FOREIGN KEY REFERENCES SubGroups(SubGroupID),    
    ItemName VARCHAR(100) NOT NULL,    
    ItemDescription VARCHAR(250) NOT NULL,
    ItemUnitPrice DECIMAL(12,4) NOT NULL,
    ItemUnitPriceWithMaxDiscount DECIMAL(12,4) NOT NULL,
    ItemAvailableQty INT NOT NULL,
    ItemReorderQtyReminder INT NOT NULL,
    ItemImageFilename VARCHAR(500) NOT NULL
)
GO
CREATE TABLE Countries(
    CountryID INT IDENTITY(1,1) PRIMARY KEY,
    CountryCode VARCHAR(7) NOT NULL UNIQUE,
    CountryName VARCHAR(100) NOT NULL
)
GO
CREATE TABLE Cities(
    CityID INT IDENTITY(1,1) PRIMARY KEY,
    CityCode VARCHAR(7) NOT NULL UNIQUE,
    CountryID INT FOREIGN KEY REFERENCES Countries(CountryID),
    CityName VARCHAR(100) NOT NULL
)
GO
CREATE TABLE CityAreas(
    CityAreaID INT IDENTITY(1,1) PRIMARY KEY,
    CityAreaCode VARCHAR(7) NOT NULL UNIQUE,
    CityID INT FOREIGN KEY REFERENCES Cities(CityID),
    CityAreaName VARCHAR(100) NOT NULL
)
GO
CREATE TABLE AddressLocations(
    AddressLocationID INT IDENTITY(1,1) PRIMARY KEY,    
    AddressLine1 VARCHAR(100) NOT NULL,
    AddressLine2 VARCHAR(100) NULL,
    CityAreaID INT FOREIGN KEY REFERENCES CityAreas(CityAreaID),
    PostCode VARCHAR(10) NULL
)
GO
CREATE TABLE CustomerTypes(
    CustomerTypeID INT IDENTITY(1,1) PRIMARY KEY,
    CustomerTypeCode VARCHAR(7) NOT NULL UNIQUE,
    CustomerTypeName VARCHAR(100) NOT NULL
)
GO
CREATE TABLE Customers(
    CustomerID INT IDENTITY(1,1) PRIMARY KEY,
    CustomerTypeID INT FOREIGN KEY REFERENCES CustomerTypes(CustomerTypeID),
    CustomerCode VARCHAR(7) NOT NULL UNIQUE,
    CustomerName VARCHAR(250) NOT NULL,
    CustomerContactNumber VARCHAR(30) NOT NULL,
    CustomerEmailAddress VARCHAR(250) NULL    
)
GO
CREATE TABLE CustomerAddresses(
    CustomerAddressID INT IDENTITY(1,1) PRIMARY KEY,    
    CustomerID INT FOREIGN KEY REFERENCES Customers(CustomerID),
    AddressLocationID INT FOREIGN KEY REFERENCES AddressLocations(AddressLocationID),
    IsDefaultAddress BIT NOT NULL,
    CustomerAddressDescription VARCHAR(100) NOT NULL,
)
GO
CREATE TABLE OrderStatus(
    OrderStatusID INT IDENTITY(1,1) PRIMARY KEY,
    OrderstatusValue VARCHAR(20) NOT NULL
)
GO
CREATE TABLE InvoiceStatus(
    InvoiceStatusID INT IDENTITY(1,1) PRIMARY KEY,
    InvoiceStatusValue VARCHAR(20) NOT NULL
)
GO
CREATE TABLE OrderHeaders(
    OrderHeaderID INT IDENTITY(1,1) PRIMARY KEY,
    CustomerID INT FOREIGN KEY REFERENCES Customers(CustomerID),
    CustomerAddressID INT FOREIGN KEY REFERENCES CustomerAddresses(CustomerAddressID),
    OrderStatusID INT FOREIGN KEY REFERENCES OrderStatus(OrderStatusID),
    OrderDate DATETIME NOT NULL,
    DeliveryDate DATETIME NOT NULL
)
GO
CREATE TABLE  OrderItems(
    OrderItemID INT IDENTITY(1,1) PRIMARY KEY,
    OrderHeaderID INT FOREIGN KEY REFERENCES OrderHeaders(OrderHeaderID),
    ItemID INT FOREIGN KEY REFERENCES Items(ItemID),
    OrderItemStatusID INT FOREIGN KEY REFERENCES OrderStatus(OrderStatusID),
    OrderItemUnitPrice DECIMAL(12,4) NOT NULL,
    OrderItemUnitPriceAfterDiscount DECIMAL(12,4) NOT NULL,
    OrderItemQty INT NOT NULL,
    OrderItemDescription VARCHAR(100) NOT NULL    
)
GO
CREATE TABLE InvoiceHeaders(
    InvoiceHeaderID INT IDENTITY(1,1) PRIMARY KEY,
    OrderHeaderID INT FOREIGN KEY REFERENCES OrderHeaders(OrderHeaderID),
    InvoiceStatusID INT FOREIGN KEY REFERENCES InvoiceStatus(InvoiceStatusID),
    InvoiceDate DATETIME NOT NULL
)
GO
CREATE TABLE InvoiceItems(
    InvoiceItemID INT IDENTITY(1,1) PRIMARY KEY,
    InvoiceHeaderID INT FOREIGN KEY REFERENCES InvoiceHeaders(InvoiceHeaderID),
    OrderItemID INT FOREIGN KEY REFERENCES OrderItems(OrderItemID),
    InvoiceItemStatusID INT FOREIGN KEY REFERENCES InvoiceStatus(InvoiceStatusID),    
    InvoiceItemQty INT NOT NULL    
)
GO
CREATE TABLE DeliveryNotes(
    DeliveryNoteID INT IDENTITY(1,1) PRIMARY KEY,
    OrderHeaderID INT FOREIGN KEY REFERENCES OrderHeaders(OrderHeaderID),
    DeliveryDate DATETIME NOT NULL    
)
CREATE TABLE DeliveryNoteItems(
    DeliveryNoteItemID INT IDENTITY(1,1) PRIMARY KEY,
    DeliveryNoteID INT FOREIGN KEY REFERENCES DeliveryNotes(DeliveryNoteID),
    OrderItemID INT FOREIGN KEY REFERENCES OrderItems(OrderItemID)
)
CREATE TABLE AuditLogTypes(
    AuditLogTypeID INT IDENTITY(1,1) PRIMARY KEY,
    AuditLogTypeName VARCHAR(100) NOT NULL
)
GO
CREATE TABLE AuditLogs(
    AuditLogID INT IDENTITY(1,1) PRIMARY KEY,
    AuditLogTypeID INT FOREIGN KEY REFERENCES AuditLogTypes(AuditLogTypeID),
    AuditLogIdentifier INT NOT NULL,
    AuditLogIdentifierCode VARCHAR(5) NOT NULL,
    AuditLogOldValue VARCHAR(500),
    AuditLogNewValue VARCHAR(500),
    AuditLogDateChanged DATETIME NOT NULL,
    AuditLogChangedByID INT NOT NULL
)
GO

CREATE PROCEDURE dbo.DeliverExistingItems 
@OrderHeaderID INT
AS

IF EXISTS(SELECT 1 FROM OrderItems WHERE OrderHeaderID = @OrderHeaderID and OrderItemStatusID = 1)
BEGIN
	INSERT INTO DeliveryNotes(OrderHeaderID,DeliveryDate)
	VALUES(@OrderHeaderID,GetDate())

	DECLARE @CurrentDeliveryNoteID INT
	SET @CurrentDeliveryNoteID = (SELECT TOP 1 DeliveryNoteID FROM DeliveryNotes WHERE OrderHeaderID = @OrderHeaderID ORDER BY DeliveryNoteID DESC)

	INSERT INTO DeliveryNoteItems(DeliveryNoteID, OrderItemID)
	SELECT @CurrentDeliveryNoteID, OrderItemID
	FROM OrderItems
	WHERE OrderHeaderID = @OrderHeaderID
	AND OrderItemStatusID = 1

	UPDATE OrderItems
	SET OrderItemStatusID = 2
	WHERE OrderHeaderID = @OrderHeaderID
	AND OrderItemStatusID = 1

	UPDATE OrderHeaders
	SET OrderStatusID = 2
	WHERE OrderHeaderID = @OrderHeaderID

	SELECT @CurrentDeliveryNoteID
END
ELSE
BEGIN
	SELECT 0
END
