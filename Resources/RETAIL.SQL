USE [RETAIL]
;

IF SCHEMA_ID(N'STORE') IS NULL EXECUTE(N'CREATE SCHEMA [STORE]');
;

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('[STORE].[FK_OrderDetail_Order]') AND OBJECTPROPERTY(id, 'IsForeignKey') = 1)
ALTER TABLE [STORE].[OrderDetail] DROP CONSTRAINT [FK_OrderDetail_Order]
;

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('[STORE].[FK_OrderDetail_Product]') AND OBJECTPROPERTY(id, 'IsForeignKey') = 1)
ALTER TABLE [STORE].[OrderDetail] DROP CONSTRAINT [FK_OrderDetail_Product]
;

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('[STORE].[FK_Order_Customer]') AND OBJECTPROPERTY(id, 'IsForeignKey') = 1)
ALTER TABLE [STORE].[Order] DROP CONSTRAINT [FK_Order_Customer]
;

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('[STORE].[FK_Order_Shipper]') AND OBJECTPROPERTY(id, 'IsForeignKey') = 1)
ALTER TABLE [STORE].[Order] DROP CONSTRAINT [FK_Order_Shipper]
;

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('[STORE].[FK_Product_Category]') AND OBJECTPROPERTY(id, 'IsForeignKey') = 1)
ALTER TABLE [STORE].[Product] DROP CONSTRAINT [FK_Product_Category]
;

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('[STORE].[FK_Product_Supplier]') AND OBJECTPROPERTY(id, 'IsForeignKey') = 1)
ALTER TABLE [STORE].[Product] DROP CONSTRAINT [FK_Product_Supplier]
;



IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('[STORE].[Shipper]') AND  OBJECTPROPERTY(id, 'IsUserTable') = 1)
DROP TABLE [STORE].[Shipper]
;

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('[STORE].[Supplier]') AND  OBJECTPROPERTY(id, 'IsUserTable') = 1)
DROP TABLE [STORE].[Supplier]
;

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('[STORE].[Category]') AND  OBJECTPROPERTY(id, 'IsUserTable') = 1)
DROP TABLE [STORE].[Category]
;

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('[STORE].[OrderDetail]') AND  OBJECTPROPERTY(id, 'IsUserTable') = 1)
DROP TABLE [STORE].[OrderDetail]
;

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('[STORE].[Order]') AND  OBJECTPROPERTY(id, 'IsUserTable') = 1)
DROP TABLE [STORE].[Order]
;

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('[STORE].[Product]') AND  OBJECTPROPERTY(id, 'IsUserTable') = 1)
DROP TABLE [STORE].[Product]
;

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('[STORE].[Customer]') AND  OBJECTPROPERTY(id, 'IsUserTable') = 1)
DROP TABLE [STORE].[Customer]
;


CREATE TABLE [STORE].[Shipper] ( 
	[Id] bigint identity(1,1)  NOT NULL,
	[CompanyName] nvarchar(max) NOT NULL,
	[ContactName] nvarchar(max) NOT NULL,
	[Phone] nvarchar(1024) NOT NULL
)
;

CREATE TABLE [STORE].[Supplier] ( 
	[Id] bigint identity(1,1)  NOT NULL,
	[CompanyName] nvarchar(max) NOT NULL,
	[ContactName] nvarchar(1024) NULL,
	[ContactTitle] nvarchar(1024) NULL,
	[Address] nvarchar(max) NULL
)
;

CREATE TABLE [STORE].[Category] ( 
	[Id] bigint identity(1,1)  NOT NULL,
	[Name] nvarchar(256) NOT NULL,
	[Description] nvarchar(max) NULL
)
;

CREATE TABLE [STORE].[OrderDetail] ( 
	[Id] bigint identity(1,1)  NOT NULL,
	[OrderId] bigint NOT NULL,
	[ProductId] bigint NOT NULL,
	[Quantity] int DEFAULT 0 NOT NULL,
	[Discount] float DEFAULT 0 NOT NULL
)
;

CREATE TABLE [STORE].[Order] ( 
	[Id] bigint identity(1,1)  NOT NULL,
	[CustomerId] bigint NOT NULL,
	[ShipperId] bigint NOT NULL,
	[OrderDate] datetime NOT NULL,
	[TotalCharge] float DEFAULT 0 NOT NULL,
	[Freight] float DEFAULT 0 NOT NULL,
	[ShipDate] datetime NOT NULL,
	[ShipAddress] nvarchar(max) NOT NULL
)
;

CREATE TABLE [STORE].[Product] ( 
	[Id] bigint identity(1,1)  NOT NULL,
	[CategoryId] bigint NOT NULL,
	[SupplierId] bigint NOT NULL,
	[Name] nvarchar(1024) NOT NULL,
	[Description] nvarchar(max) NULL,
	[UnitPrice] float DEFAULT 0 NOT NULL,
	[UnitsInStock] int DEFAULT 0 NOT NULL
)
;

CREATE TABLE [STORE].[Customer] ( 
	[Id] bigint identity(1,1)  NOT NULL,
	[Name] nvarchar(256) NOT NULL,
	[Address] nvarchar(max) NULL,
	[Phone] nvarchar(256) NULL
)
;


ALTER TABLE [STORE].[Shipper] ADD CONSTRAINT [PK_Shipper] 
	PRIMARY KEY CLUSTERED ([Id])
;

ALTER TABLE [STORE].[Supplier] ADD CONSTRAINT [PK_Supplier] 
	PRIMARY KEY CLUSTERED ([Id])
;

ALTER TABLE [STORE].[Category] ADD CONSTRAINT [PK_Category] 
	PRIMARY KEY CLUSTERED ([Id])
;

ALTER TABLE [STORE].[OrderDetail] ADD CONSTRAINT [PK_OrderItem] 
	PRIMARY KEY CLUSTERED ([Id])
;

ALTER TABLE [STORE].[Order] ADD CONSTRAINT [PK_Order] 
	PRIMARY KEY CLUSTERED ([Id])
;

ALTER TABLE [STORE].[Product] ADD CONSTRAINT [PK_Product] 
	PRIMARY KEY CLUSTERED ([Id])
;

ALTER TABLE [STORE].[Customer] ADD CONSTRAINT [PK_Customer] 
	PRIMARY KEY CLUSTERED ([Id])
;



ALTER TABLE [STORE].[OrderDetail] ADD CONSTRAINT [FK_OrderDetail_Order] 
	FOREIGN KEY ([OrderId]) REFERENCES [STORE].[Order] ([Id])
;

ALTER TABLE [STORE].[OrderDetail] ADD CONSTRAINT [FK_OrderDetail_Product] 
	FOREIGN KEY ([ProductId]) REFERENCES [STORE].[Product] ([Id])
;

ALTER TABLE [STORE].[Order] ADD CONSTRAINT [FK_Order_Customer] 
	FOREIGN KEY ([CustomerId]) REFERENCES [STORE].[Customer] ([Id])
;

ALTER TABLE [STORE].[Order] ADD CONSTRAINT [FK_Order_Shipper] 
	FOREIGN KEY ([ShipperId]) REFERENCES [STORE].[Shipper] ([Id])
;

ALTER TABLE [STORE].[Product] ADD CONSTRAINT [FK_Product_Category] 
	FOREIGN KEY ([CategoryId]) REFERENCES [STORE].[Category] ([Id])
;

ALTER TABLE [STORE].[Product] ADD CONSTRAINT [FK_Product_Supplier] 
	FOREIGN KEY ([SupplierId]) REFERENCES [STORE].[Supplier] ([Id])
;
