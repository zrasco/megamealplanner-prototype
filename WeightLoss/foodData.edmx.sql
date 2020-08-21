
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 06/23/2015 23:49:46
-- Generated from EDMX file: D:\Data center\Development\ASP .NET\WeightLoss\WeightLoss\foodData.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [weightloss_website];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Ingredients_In_Meals_Ingredients]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Ingredients_In_Meals] DROP CONSTRAINT [FK_Ingredients_In_Meals_Ingredients];
GO
IF OBJECT_ID(N'[dbo].[FK_Meals_Ingredients_In_Meals]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Ingredients_In_Meals] DROP CONSTRAINT [FK_Meals_Ingredients_In_Meals];
GO
IF OBJECT_ID(N'[dbo].[FK_Users_MealPlanList]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MealPlanList] DROP CONSTRAINT [FK_Users_MealPlanList];
GO
IF OBJECT_ID(N'[dbo].[FK_Users_Meals]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Meals] DROP CONSTRAINT [FK_Users_Meals];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Ingredients]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Ingredients];
GO
IF OBJECT_ID(N'[dbo].[Ingredients_In_Meals]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Ingredients_In_Meals];
GO
IF OBJECT_ID(N'[dbo].[MealPlanList]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MealPlanList];
GO
IF OBJECT_ID(N'[dbo].[Meals]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Meals];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Ingredients'
CREATE TABLE [dbo].[Ingredients] (
    [IngredientId] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(50)  NOT NULL,
    [Quantity_Nbr] decimal(18,1)  NOT NULL,
    [Quantity_Measurement] varchar(20)  NOT NULL,
    [Calories] smallint  NULL,
    [Category] varchar(30)  NULL
);
GO

-- Creating table 'Ingredients_In_Meals'
CREATE TABLE [dbo].[Ingredients_In_Meals] (
    [InstanceId] int IDENTITY(1,1) NOT NULL,
    [MealId] int  NOT NULL,
    [IngredientId] int  NOT NULL,
    [Quantity] smallint  NOT NULL
);
GO

-- Creating table 'MealPlanLists'
CREATE TABLE [dbo].[MealPlanLists] (
    [InstanceId] int IDENTITY(1,1) NOT NULL,
    [UserId] int  NOT NULL,
    [MealId] int  NOT NULL,
    [MealDate] datetime  NOT NULL
);
GO

-- Creating table 'Meals'
CREATE TABLE [dbo].[Meals] (
    [MealId] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(256)  NULL,
    [UserId] int  NULL,
    [Cuisine] varchar(30)  NULL,
    [Breakfast] bit  NOT NULL,
    [Lunch] bit  NOT NULL,
    [Dinner] bit  NOT NULL,
    [Snack] bit  NOT NULL,
    [Rating] tinyint  NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [UserId] int IDENTITY(1,1) NOT NULL,
    [UserName] nvarchar(32)  NOT NULL,
    [LastLogin] datetime  NULL,
    [DateJoined] datetime  NOT NULL,
    [FirstName] nvarchar(50)  NULL,
    [LastName] nvarchar(50)  NULL,
    [Email] nvarchar(100)  NULL,
    [DOB] datetime  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [IngredientId] in table 'Ingredients'
ALTER TABLE [dbo].[Ingredients]
ADD CONSTRAINT [PK_Ingredients]
    PRIMARY KEY CLUSTERED ([IngredientId] ASC);
GO

-- Creating primary key on [InstanceId] in table 'Ingredients_In_Meals'
ALTER TABLE [dbo].[Ingredients_In_Meals]
ADD CONSTRAINT [PK_Ingredients_In_Meals]
    PRIMARY KEY CLUSTERED ([InstanceId] ASC);
GO

-- Creating primary key on [InstanceId] in table 'MealPlanLists'
ALTER TABLE [dbo].[MealPlanLists]
ADD CONSTRAINT [PK_MealPlanLists]
    PRIMARY KEY CLUSTERED ([InstanceId] ASC);
GO

-- Creating primary key on [MealId] in table 'Meals'
ALTER TABLE [dbo].[Meals]
ADD CONSTRAINT [PK_Meals]
    PRIMARY KEY CLUSTERED ([MealId] ASC);
GO

-- Creating primary key on [UserId] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([UserId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [IngredientId] in table 'Ingredients_In_Meals'
ALTER TABLE [dbo].[Ingredients_In_Meals]
ADD CONSTRAINT [FK_Ingredients_In_Meals_Ingredients]
    FOREIGN KEY ([IngredientId])
    REFERENCES [dbo].[Ingredients]
        ([IngredientId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Ingredients_In_Meals_Ingredients'
CREATE INDEX [IX_FK_Ingredients_In_Meals_Ingredients]
ON [dbo].[Ingredients_In_Meals]
    ([IngredientId]);
GO

-- Creating foreign key on [MealId] in table 'Ingredients_In_Meals'
ALTER TABLE [dbo].[Ingredients_In_Meals]
ADD CONSTRAINT [FK_Meals_Ingredients_In_Meals]
    FOREIGN KEY ([MealId])
    REFERENCES [dbo].[Meals]
        ([MealId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Meals_Ingredients_In_Meals'
CREATE INDEX [IX_FK_Meals_Ingredients_In_Meals]
ON [dbo].[Ingredients_In_Meals]
    ([MealId]);
GO

-- Creating foreign key on [UserId] in table 'MealPlanLists'
ALTER TABLE [dbo].[MealPlanLists]
ADD CONSTRAINT [FK_Users_MealPlanList]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Users]
        ([UserId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Users_MealPlanList'
CREATE INDEX [IX_FK_Users_MealPlanList]
ON [dbo].[MealPlanLists]
    ([UserId]);
GO

-- Creating foreign key on [UserId] in table 'Meals'
ALTER TABLE [dbo].[Meals]
ADD CONSTRAINT [FK_Users_Meals]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Users]
        ([UserId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Users_Meals'
CREATE INDEX [IX_FK_Users_Meals]
ON [dbo].[Meals]
    ([UserId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------