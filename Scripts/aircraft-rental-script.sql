IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210220193602_CreateDefinition')
BEGIN
    CREATE TABLE [dbo].[Aircraft] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(100) NOT NULL,
        [State] int NOT NULL,
        [Description] nvarchar(max) NULL,
        CONSTRAINT [PK_Aircraft] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210220193602_CreateDefinition')
BEGIN
    CREATE TABLE [dbo].[Passenger] (
        [Id] int NOT NULL IDENTITY,
        [IdentificationDocument] bigint NOT NULL,
        [FirstName] varchar(50) NOT NULL,
        [LastName] varchar(50) NOT NULL,
        [Specialty] varchar(max) NOT NULL,
        CONSTRAINT [PK_Passenger] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210220193602_CreateDefinition')
BEGIN
    CREATE TABLE [dbo].[Permission] (
        [Id] int NOT NULL IDENTITY,
        [Name] varchar(50) NOT NULL,
        CONSTRAINT [PK_Permission] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210220193602_CreateDefinition')
BEGIN
    CREATE TABLE [dbo].[Role] (
        [Id] int NOT NULL IDENTITY,
        [Name] varchar(30) NOT NULL,
        CONSTRAINT [PK_Role] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210220193602_CreateDefinition')
BEGIN
    CREATE TABLE [dbo].[Rental] (
        [Id] int NOT NULL IDENTITY,
        [PassengerId] int NOT NULL,
        [AircraftId] int NOT NULL,
        [Location] varchar(max) NOT NULL,
        [ArrivalDate] datetime2 NOT NULL,
        [DepartureDate] datetime2 NOT NULL,
        CONSTRAINT [PK_Rental] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Rental_Aircraft_AircraftId] FOREIGN KEY ([AircraftId]) REFERENCES [dbo].[Aircraft] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Rental_Passenger_PassengerId] FOREIGN KEY ([PassengerId]) REFERENCES [dbo].[Passenger] ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210220193602_CreateDefinition')
BEGIN
    CREATE TABLE [dbo].[RolePermission] (
        [RoleId] int NOT NULL,
        [PermissionId] int NOT NULL,
        CONSTRAINT [PK_RolePermission] PRIMARY KEY ([RoleId], [PermissionId]),
        CONSTRAINT [FK_RolePermission_Permission_PermissionId] FOREIGN KEY ([PermissionId]) REFERENCES [dbo].[Permission] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_RolePermission_Role_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Role] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210220193602_CreateDefinition')
BEGIN
    CREATE TABLE [dbo].[User] (
        [Id] int NOT NULL IDENTITY,
        [RoleId] int NOT NULL,
        [Username] nvarchar(100) NOT NULL,
        [Password] varchar(max) NOT NULL,
        [Email] varchar(100) NOT NULL,
        [FirstName] varchar(50) NOT NULL,
        [LastName] varchar(50) NOT NULL,
        CONSTRAINT [PK_User] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_User_Role_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Role] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210220193602_CreateDefinition')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[dbo].[Permission]'))
        SET IDENTITY_INSERT [dbo].[Permission] ON;
    EXEC(N'INSERT INTO [dbo].[Permission] ([Id], [Name])
    VALUES (1, ''CanRoles''),
    (2, ''CanUsers''),
    (3, ''CanAircrafts''),
    (4, ''CanPassengers''),
    (5, ''CanRentals'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[dbo].[Permission]'))
        SET IDENTITY_INSERT [dbo].[Permission] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210220193602_CreateDefinition')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[dbo].[Role]'))
        SET IDENTITY_INSERT [dbo].[Role] ON;
    EXEC(N'INSERT INTO [dbo].[Role] ([Id], [Name])
    VALUES (1, ''AdminUser''),
    (2, ''CommonUser'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[dbo].[Role]'))
        SET IDENTITY_INSERT [dbo].[Role] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210220193602_CreateDefinition')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'PermissionId', N'RoleId') AND [object_id] = OBJECT_ID(N'[dbo].[RolePermission]'))
        SET IDENTITY_INSERT [dbo].[RolePermission] ON;
    EXEC(N'INSERT INTO [dbo].[RolePermission] ([PermissionId], [RoleId])
    VALUES (1, 1),
    (2, 1),
    (3, 1),
    (4, 1),
    (5, 1),
    (4, 2),
    (5, 2)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'PermissionId', N'RoleId') AND [object_id] = OBJECT_ID(N'[dbo].[RolePermission]'))
        SET IDENTITY_INSERT [dbo].[RolePermission] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210220193602_CreateDefinition')
BEGIN
    CREATE UNIQUE INDEX [IX_Permission_Name] ON [dbo].[Permission] ([Name]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210220193602_CreateDefinition')
BEGIN
    CREATE INDEX [IX_Rental_AircraftId] ON [dbo].[Rental] ([AircraftId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210220193602_CreateDefinition')
BEGIN
    CREATE INDEX [IX_Rental_PassengerId] ON [dbo].[Rental] ([PassengerId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210220193602_CreateDefinition')
BEGIN
    CREATE UNIQUE INDEX [IX_Role_Name] ON [dbo].[Role] ([Name]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210220193602_CreateDefinition')
BEGIN
    CREATE INDEX [IX_RolePermission_PermissionId] ON [dbo].[RolePermission] ([PermissionId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210220193602_CreateDefinition')
BEGIN
    CREATE INDEX [IX_User_RoleId] ON [dbo].[User] ([RoleId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210220193602_CreateDefinition')
BEGIN
    CREATE UNIQUE INDEX [IX_User_Username_Email] ON [dbo].[User] ([Username], [Email]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210220193602_CreateDefinition')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20210220193602_CreateDefinition', N'5.0.3');
END;
GO

COMMIT;
GO

