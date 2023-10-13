
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 09/30/2023 13:41:08
-- Generated from EDMX file: D:\5032Project\FIT5032_EasyX\Models\BookingModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [aspnet-FIT5032_EasyX-20230930113409];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_AspNetUserRoles_AspNetRoles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserRoles] DROP CONSTRAINT [FK_AspNetUserRoles_AspNetRoles];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetUserRoles_AspNetUsers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserRoles] DROP CONSTRAINT [FK_AspNetUserRoles_AspNetUsers];
GO
IF OBJECT_ID(N'[dbo].[FK_DoctorBookings]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BookingsSet] DROP CONSTRAINT [FK_DoctorBookings];
GO
IF OBJECT_ID(N'[dbo].[FK_PatientBookings]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BookingsSet] DROP CONSTRAINT [FK_PatientBookings];
GO
IF OBJECT_ID(N'[dbo].[FK_Doctor_inherits_AspNetUsers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUsers_Doctor] DROP CONSTRAINT [FK_Doctor_inherits_AspNetUsers];
GO
IF OBJECT_ID(N'[dbo].[FK_Patient_inherits_AspNetUsers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUsers_Patient] DROP CONSTRAINT [FK_Patient_inherits_AspNetUsers];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[AspNetRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetRoles];
GO
IF OBJECT_ID(N'[dbo].[AspNetUsers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUsers];
GO
IF OBJECT_ID(N'[dbo].[BookingsSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BookingsSet];
GO
IF OBJECT_ID(N'[dbo].[AspNetUsers_Doctor]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUsers_Doctor];
GO
IF OBJECT_ID(N'[dbo].[AspNetUsers_Patient]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUsers_Patient];
GO
IF OBJECT_ID(N'[dbo].[AspNetUserRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUserRoles];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'AspNetRoles'
CREATE TABLE [dbo].[AspNetRoles] (
    [Id] nvarchar(128)  NOT NULL,
    [Name] nvarchar(256)  NOT NULL
);
GO

-- Creating table 'AspNetUsers'
CREATE TABLE [dbo].[AspNetUsers] (
    [Id] nvarchar(128)  NOT NULL,
    [Email] nvarchar(256)  NULL,
    [EmailConfirmed] bit  NOT NULL,
    [PasswordHash] nvarchar(max)  NULL,
    [SecurityStamp] nvarchar(max)  NULL,
    [PhoneNumber] nvarchar(max)  NULL,
    [PhoneNumberConfirmed] bit  NOT NULL,
    [TwoFactorEnabled] bit  NOT NULL,
    [LockoutEndDateUtc] datetime  NULL,
    [LockoutEnabled] bit  NOT NULL,
    [AccessFailedCount] int  NOT NULL,
    [UserName] nvarchar(256)  NOT NULL
);
GO

-- Creating table 'BookingsSet'
CREATE TABLE [dbo].[BookingsSet] (
    [Boking_Id] int IDENTITY(1,1) NOT NULL,
    [Booking_Date] datetime  NOT NULL,
    [Booking_Content] nvarchar(max)  NOT NULL,
    [Booking_IsConfirm] bit  NOT NULL,
    [DoctorId] nvarchar(128)  NOT NULL,
    [PatientId] nvarchar(128)  NOT NULL,
    [Rating] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'AspNetUsers_Doctor'
CREATE TABLE [dbo].[AspNetUsers_Doctor] (
    [Title] nvarchar(max)  NOT NULL,
    [First_Name] nvarchar(max)  NOT NULL,
    [Last_Name] nvarchar(max)  NOT NULL,
    [Major] nvarchar(max)  NOT NULL,
    [Address] nvarchar(max)  NOT NULL,
    [Is_Ready_For_Visitor] nvarchar(max)  NOT NULL,
    [Id] nvarchar(128)  NOT NULL
);
GO

-- Creating table 'AspNetUsers_Patient'
CREATE TABLE [dbo].[AspNetUsers_Patient] (
    [Title] nvarchar(max)  NOT NULL,
    [First_Name] nvarchar(max)  NOT NULL,
    [Last_Name] nvarchar(max)  NOT NULL,
    [Date_Of_Birth] nvarchar(max)  NOT NULL,
    [Address] nvarchar(max)  NOT NULL,
    [Id] nvarchar(128)  NOT NULL
);
GO

-- Creating table 'AspNetUserRoles'
CREATE TABLE [dbo].[AspNetUserRoles] (
    [AspNetRoles_Id] nvarchar(128)  NOT NULL,
    [AspNetUsers_Id] nvarchar(128)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'AspNetRoles'
ALTER TABLE [dbo].[AspNetRoles]
ADD CONSTRAINT [PK_AspNetRoles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetUsers'
ALTER TABLE [dbo].[AspNetUsers]
ADD CONSTRAINT [PK_AspNetUsers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Boking_Id] in table 'BookingsSet'
ALTER TABLE [dbo].[BookingsSet]
ADD CONSTRAINT [PK_BookingsSet]
    PRIMARY KEY CLUSTERED ([Boking_Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetUsers_Doctor'
ALTER TABLE [dbo].[AspNetUsers_Doctor]
ADD CONSTRAINT [PK_AspNetUsers_Doctor]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetUsers_Patient'
ALTER TABLE [dbo].[AspNetUsers_Patient]
ADD CONSTRAINT [PK_AspNetUsers_Patient]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [AspNetRoles_Id], [AspNetUsers_Id] in table 'AspNetUserRoles'
ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [PK_AspNetUserRoles]
    PRIMARY KEY CLUSTERED ([AspNetRoles_Id], [AspNetUsers_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [AspNetRoles_Id] in table 'AspNetUserRoles'
ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [FK_AspNetUserRoles_AspNetRoles]
    FOREIGN KEY ([AspNetRoles_Id])
    REFERENCES [dbo].[AspNetRoles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [AspNetUsers_Id] in table 'AspNetUserRoles'
ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [FK_AspNetUserRoles_AspNetUsers]
    FOREIGN KEY ([AspNetUsers_Id])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetUserRoles_AspNetUsers'
CREATE INDEX [IX_FK_AspNetUserRoles_AspNetUsers]
ON [dbo].[AspNetUserRoles]
    ([AspNetUsers_Id]);
GO

-- Creating foreign key on [DoctorId] in table 'BookingsSet'
ALTER TABLE [dbo].[BookingsSet]
ADD CONSTRAINT [FK_DoctorBookings]
    FOREIGN KEY ([DoctorId])
    REFERENCES [dbo].[AspNetUsers_Doctor]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DoctorBookings'
CREATE INDEX [IX_FK_DoctorBookings]
ON [dbo].[BookingsSet]
    ([DoctorId]);
GO

-- Creating foreign key on [PatientId] in table 'BookingsSet'
ALTER TABLE [dbo].[BookingsSet]
ADD CONSTRAINT [FK_PatientBookings]
    FOREIGN KEY ([PatientId])
    REFERENCES [dbo].[AspNetUsers_Patient]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PatientBookings'
CREATE INDEX [IX_FK_PatientBookings]
ON [dbo].[BookingsSet]
    ([PatientId]);
GO

-- Creating foreign key on [Id] in table 'AspNetUsers_Doctor'
ALTER TABLE [dbo].[AspNetUsers_Doctor]
ADD CONSTRAINT [FK_Doctor_inherits_AspNetUsers]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'AspNetUsers_Patient'
ALTER TABLE [dbo].[AspNetUsers_Patient]
ADD CONSTRAINT [FK_Patient_inherits_AspNetUsers]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------