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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210214092642_CreateDefinition')
BEGIN
    CREATE TABLE [dbo].[Aircraft] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(30) NOT NULL,
        [State] int NOT NULL,
        [Description] nvarchar(max) NULL,
        CONSTRAINT [PK_Aircraft] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210214092642_CreateDefinition')
BEGIN
    CREATE TABLE [dbo].[Passenger] (
        [Id] int NOT NULL IDENTITY,
        [IdentificationDocument] bigint NOT NULL,
        [FirstName] varchar(30) NOT NULL,
        [LastName] varchar(30) NOT NULL,
        [Specialty] varchar(max) NOT NULL,
        CONSTRAINT [PK_Passenger] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210214092642_CreateDefinition')
BEGIN
    CREATE TABLE [dbo].[Permission] (
        [Id] int NOT NULL IDENTITY,
        [Name] varchar(50) NOT NULL,
        CONSTRAINT [PK_Permission] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210214092642_CreateDefinition')
BEGIN
    CREATE TABLE [dbo].[Role] (
        [Id] int NOT NULL IDENTITY,
        [Name] varchar(30) NOT NULL,
        CONSTRAINT [PK_Role] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210214092642_CreateDefinition')
BEGIN
    CREATE TABLE [dbo].[Rental] (
        [Id] int NOT NULL IDENTITY,
        [IdPassenger] int NOT NULL,
        [IdAircraft] int NOT NULL,
        [Location] varchar(max) NOT NULL,
        [ArrivalDate] datetime2 NOT NULL,
        [DepartureDate] datetime2 NOT NULL,
        CONSTRAINT [PK_Rental] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Rental_Aircraft_IdAircraft] FOREIGN KEY ([IdAircraft]) REFERENCES [dbo].[Aircraft] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Rental_Passenger_IdPassenger] FOREIGN KEY ([IdPassenger]) REFERENCES [dbo].[Passenger] ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210214092642_CreateDefinition')
BEGIN
    CREATE TABLE [dbo].[RolePermission] (
        [IdRole] int NOT NULL,
        [IdPermission] int NOT NULL,
        CONSTRAINT [PK_RolePermission] PRIMARY KEY ([IdRole], [IdPermission]),
        CONSTRAINT [FK_RolePermission_Permission_IdPermission] FOREIGN KEY ([IdPermission]) REFERENCES [dbo].[Permission] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_RolePermission_Role_IdRole] FOREIGN KEY ([IdRole]) REFERENCES [dbo].[Role] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210214092642_CreateDefinition')
BEGIN
    CREATE TABLE [dbo].[User] (
        [Id] int NOT NULL IDENTITY,
        [IdRole] int NOT NULL,
        [Username] nvarchar(20) NOT NULL,
        [Password] varchar(max) NOT NULL,
        [Email] varchar(50) NOT NULL,
        [FirstName] varchar(30) NOT NULL,
        [LastName] varchar(30) NOT NULL,
        CONSTRAINT [PK_User] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_User_Role_IdRole] FOREIGN KEY ([IdRole]) REFERENCES [dbo].[Role] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210214092642_CreateDefinition')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[dbo].[Permission]'))
        SET IDENTITY_INSERT [dbo].[Permission] ON;
    EXEC(N'INSERT INTO [dbo].[Permission] ([Id], [Name])
    VALUES (1, ''ROLES''),
    (2, ''USERS''),
    (3, ''AIRCRAFTS''),
    (4, ''PASSENGERS''),
    (5, ''RENTALS'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[dbo].[Permission]'))
        SET IDENTITY_INSERT [dbo].[Permission] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210214092642_CreateDefinition')
BEGIN
    CREATE UNIQUE INDEX [IX_Permission_Name] ON [dbo].[Permission] ([Name]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210214092642_CreateDefinition')
BEGIN
    CREATE INDEX [IX_Rental_IdAircraft] ON [dbo].[Rental] ([IdAircraft]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210214092642_CreateDefinition')
BEGIN
    CREATE INDEX [IX_Rental_IdPassenger] ON [dbo].[Rental] ([IdPassenger]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210214092642_CreateDefinition')
BEGIN
    CREATE UNIQUE INDEX [IX_Role_Name] ON [dbo].[Role] ([Name]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210214092642_CreateDefinition')
BEGIN
    CREATE INDEX [IX_RolePermission_IdPermission] ON [dbo].[RolePermission] ([IdPermission]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210214092642_CreateDefinition')
BEGIN
    CREATE INDEX [IX_User_IdRole] ON [dbo].[User] ([IdRole]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210214092642_CreateDefinition')
BEGIN
    CREATE UNIQUE INDEX [IX_User_Username_Email] ON [dbo].[User] ([Username], [Email]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210214092642_CreateDefinition')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20210214092642_CreateDefinition', N'5.0.3');
END;
GO

COMMIT;
GO

