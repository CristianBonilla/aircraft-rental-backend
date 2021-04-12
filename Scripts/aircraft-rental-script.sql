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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210412102701_CreateDefinition')
BEGIN
    CREATE TABLE [dbo].[Aircraft] (
        [Id] uniqueidentifier NOT NULL DEFAULT (NEWID()),
        [Name] nvarchar(100) NOT NULL,
        [State] int NOT NULL,
        [Description] nvarchar(max) NULL,
        CONSTRAINT [PK_Aircraft] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210412102701_CreateDefinition')
BEGIN
    CREATE TABLE [dbo].[Passenger] (
        [Id] uniqueidentifier NOT NULL DEFAULT (NEWID()),
        [IdentificationDocument] bigint NOT NULL,
        [FirstName] varchar(50) NOT NULL,
        [LastName] varchar(50) NOT NULL,
        [Specialty] varchar(max) NOT NULL,
        CONSTRAINT [PK_Passenger] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210412102701_CreateDefinition')
BEGIN
    CREATE TABLE [dbo].[Permission] (
        [Id] uniqueidentifier NOT NULL DEFAULT (NEWID()),
        [Order] int NOT NULL,
        [Name] varchar(50) NOT NULL,
        [DisplayName] varchar(50) NOT NULL,
        CONSTRAINT [PK_Permission] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210412102701_CreateDefinition')
BEGIN
    CREATE TABLE [dbo].[Role] (
        [Id] uniqueidentifier NOT NULL DEFAULT (NEWID()),
        [Name] varchar(30) NOT NULL,
        [DisplayName] varchar(50) NOT NULL,
        CONSTRAINT [PK_Role] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210412102701_CreateDefinition')
BEGIN
    CREATE TABLE [dbo].[Rental] (
        [Id] uniqueidentifier NOT NULL DEFAULT (NEWID()),
        [PassengerId] uniqueidentifier NOT NULL,
        [AircraftId] uniqueidentifier NOT NULL,
        [Location] varchar(max) NOT NULL,
        [ArrivalDate] datetime2 NOT NULL,
        [DepartureDate] datetime2 NOT NULL,
        CONSTRAINT [PK_Rental] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Rental_Aircraft_AircraftId] FOREIGN KEY ([AircraftId]) REFERENCES [dbo].[Aircraft] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Rental_Passenger_PassengerId] FOREIGN KEY ([PassengerId]) REFERENCES [dbo].[Passenger] ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210412102701_CreateDefinition')
BEGIN
    CREATE TABLE [dbo].[RolePermission] (
        [RoleId] uniqueidentifier NOT NULL,
        [PermissionId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_RolePermission] PRIMARY KEY ([RoleId], [PermissionId]),
        CONSTRAINT [FK_RolePermission_Permission_PermissionId] FOREIGN KEY ([PermissionId]) REFERENCES [dbo].[Permission] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_RolePermission_Role_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Role] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210412102701_CreateDefinition')
BEGIN
    CREATE TABLE [dbo].[User] (
        [Id] uniqueidentifier NOT NULL DEFAULT (NEWID()),
        [RoleId] uniqueidentifier NOT NULL,
        [IdentificationDocument] bigint NOT NULL,
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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210412102701_CreateDefinition')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'DisplayName', N'Name', N'Order') AND [object_id] = OBJECT_ID(N'[dbo].[Permission]'))
        SET IDENTITY_INSERT [dbo].[Permission] ON;
    EXEC(N'INSERT INTO [dbo].[Permission] ([Id], [DisplayName], [Name], [Order])
    VALUES (''c5e3a53f-ce37-4512-91f3-a6d823dabe06'', ''Roles'', ''CanRoles'', 1),
    (''b8c5caa1-4a44-4783-af7e-eb29617a5a70'', ''Usuarios'', ''CanUsers'', 2),
    (''186df72b-0328-4539-8015-2965eb13ccec'', ''Alquileres'', ''CanRentals'', 3),
    (''44eb6612-536e-46d2-96ef-a752691f2296'', ''Aeronaves'', ''CanAircrafts'', 4),
    (''352dec26-951c-4236-afb5-b059f014e819'', ''Pasajeros'', ''CanPassengers'', 5)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'DisplayName', N'Name', N'Order') AND [object_id] = OBJECT_ID(N'[dbo].[Permission]'))
        SET IDENTITY_INSERT [dbo].[Permission] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210412102701_CreateDefinition')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'DisplayName', N'Name') AND [object_id] = OBJECT_ID(N'[dbo].[Role]'))
        SET IDENTITY_INSERT [dbo].[Role] ON;
    EXEC(N'INSERT INTO [dbo].[Role] ([Id], [DisplayName], [Name])
    VALUES (''6bbe4b56-3f81-4957-a8f1-33c9112db4a2'', ''Administrador'', ''AdminUser''),
    (''22b20e06-f147-41d6-8333-7c921242ad27'', ''Usuario Común'', ''CommonUser''),
    (''aedb18fc-7b6c-488c-80bf-8bc2b36febe3'', ''Pasajero'', ''PassengerUser''),
    (''da9fbf03-d19b-4586-a28b-7b8deaa7a5b6'', ''Piloto'', ''PilotUser'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'DisplayName', N'Name') AND [object_id] = OBJECT_ID(N'[dbo].[Role]'))
        SET IDENTITY_INSERT [dbo].[Role] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210412102701_CreateDefinition')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'PermissionId', N'RoleId') AND [object_id] = OBJECT_ID(N'[dbo].[RolePermission]'))
        SET IDENTITY_INSERT [dbo].[RolePermission] ON;
    EXEC(N'INSERT INTO [dbo].[RolePermission] ([PermissionId], [RoleId])
    VALUES (''c5e3a53f-ce37-4512-91f3-a6d823dabe06'', ''6bbe4b56-3f81-4957-a8f1-33c9112db4a2''),
    (''b8c5caa1-4a44-4783-af7e-eb29617a5a70'', ''6bbe4b56-3f81-4957-a8f1-33c9112db4a2''),
    (''186df72b-0328-4539-8015-2965eb13ccec'', ''6bbe4b56-3f81-4957-a8f1-33c9112db4a2''),
    (''44eb6612-536e-46d2-96ef-a752691f2296'', ''6bbe4b56-3f81-4957-a8f1-33c9112db4a2''),
    (''352dec26-951c-4236-afb5-b059f014e819'', ''6bbe4b56-3f81-4957-a8f1-33c9112db4a2''),
    (''186df72b-0328-4539-8015-2965eb13ccec'', ''22b20e06-f147-41d6-8333-7c921242ad27''),
    (''44eb6612-536e-46d2-96ef-a752691f2296'', ''22b20e06-f147-41d6-8333-7c921242ad27''),
    (''352dec26-951c-4236-afb5-b059f014e819'', ''22b20e06-f147-41d6-8333-7c921242ad27''),
    (''44eb6612-536e-46d2-96ef-a752691f2296'', ''aedb18fc-7b6c-488c-80bf-8bc2b36febe3''),
    (''352dec26-951c-4236-afb5-b059f014e819'', ''aedb18fc-7b6c-488c-80bf-8bc2b36febe3''),
    (''186df72b-0328-4539-8015-2965eb13ccec'', ''da9fbf03-d19b-4586-a28b-7b8deaa7a5b6''),
    (''44eb6612-536e-46d2-96ef-a752691f2296'', ''da9fbf03-d19b-4586-a28b-7b8deaa7a5b6''),
    (''352dec26-951c-4236-afb5-b059f014e819'', ''da9fbf03-d19b-4586-a28b-7b8deaa7a5b6'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'PermissionId', N'RoleId') AND [object_id] = OBJECT_ID(N'[dbo].[RolePermission]'))
        SET IDENTITY_INSERT [dbo].[RolePermission] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210412102701_CreateDefinition')
BEGIN
    CREATE UNIQUE INDEX [IX_Permission_Name_DisplayName] ON [dbo].[Permission] ([Name], [DisplayName]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210412102701_CreateDefinition')
BEGIN
    CREATE INDEX [IX_Rental_AircraftId] ON [dbo].[Rental] ([AircraftId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210412102701_CreateDefinition')
BEGIN
    CREATE INDEX [IX_Rental_PassengerId] ON [dbo].[Rental] ([PassengerId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210412102701_CreateDefinition')
BEGIN
    CREATE UNIQUE INDEX [IX_Role_Name_DisplayName] ON [dbo].[Role] ([Name], [DisplayName]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210412102701_CreateDefinition')
BEGIN
    CREATE INDEX [IX_RolePermission_PermissionId] ON [dbo].[RolePermission] ([PermissionId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210412102701_CreateDefinition')
BEGIN
    CREATE UNIQUE INDEX [IX_User_IdentificationDocument_Username_Email] ON [dbo].[User] ([IdentificationDocument], [Username], [Email]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210412102701_CreateDefinition')
BEGIN
    CREATE INDEX [IX_User_RoleId] ON [dbo].[User] ([RoleId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210412102701_CreateDefinition')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20210412102701_CreateDefinition', N'5.0.3');
END;
GO

COMMIT;
GO

