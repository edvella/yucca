CREATE TABLE [dbo].[Suppliers]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [AddressLine1] NVARCHAR(50) NULL, 
    [AddressLine2] NVARCHAR(50) NULL, 
    [City] NVARCHAR(20) NULL, 
    [State] NVARCHAR(20) NULL, 
    [CountryIsoCode] NCHAR(2) NULL, 
    [PostCode] NCHAR(10) NULL, 
    [ContactPhone] NVARCHAR(20) NULL, 
    [Email] NVARCHAR(255) NULL, 
    [Website] NVARCHAR(255) NULL, 
    [TaxNumber] NVARCHAR(20) NULL, 
    [ModifiedOn] DATETIME NOT NULL, 
    [ModifiedBy] NVARCHAR(20) NULL
)
