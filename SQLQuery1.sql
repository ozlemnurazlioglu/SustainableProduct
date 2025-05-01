CREATE TABLE [dbo].[Products]
(
    [ProductID] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    [ProductName] NVARCHAR(100) NOT NULL,
    [Description] NVARCHAR(MAX) NULL,
    [Price] DECIMAL(10,2) NULL,
    [Brand] NVARCHAR(100) NULL,
    [Category] NVARCHAR(100) NULL,
    [SustainabilityTags] NVARCHAR(255) NULL,
    [EstimatedCarbonFootprint] DECIMAL(10,2) NULL,
    [Image] VARBINARY(MAX) NULL
);
