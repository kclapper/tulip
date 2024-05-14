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

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221102152519_InitialMigration'
)
BEGIN
    CREATE TABLE [AspNetRoles] (
        [Id] nvarchar(450) NOT NULL,
        [Name] nvarchar(256) NULL,
        [NormalizedName] nvarchar(256) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221102152519_InitialMigration'
)
BEGIN
    CREATE TABLE [AspNetUsers] (
        [Id] nvarchar(450) NOT NULL,
        [Discriminator] nvarchar(max) NOT NULL,
        [FirstName] nvarchar(max) NULL,
        [LastName] nvarchar(max) NULL,
        [Name] nvarchar(max) NULL,
        [UserName] nvarchar(256) NULL,
        [NormalizedUserName] nvarchar(256) NULL,
        [Email] nvarchar(256) NULL,
        [NormalizedEmail] nvarchar(256) NULL,
        [EmailConfirmed] bit NOT NULL,
        [PasswordHash] nvarchar(max) NULL,
        [SecurityStamp] nvarchar(max) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        [PhoneNumber] nvarchar(max) NULL,
        [PhoneNumberConfirmed] bit NOT NULL,
        [TwoFactorEnabled] bit NOT NULL,
        [LockoutEnd] datetimeoffset NULL,
        [LockoutEnabled] bit NOT NULL,
        [AccessFailedCount] int NOT NULL,
        CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221102152519_InitialMigration'
)
BEGIN
    CREATE TABLE [Scores] (
        [Id] int NOT NULL IDENTITY,
        [Username] nvarchar(max) NULL,
        [Score] int NOT NULL,
        CONSTRAINT [PK_Scores] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221102152519_InitialMigration'
)
BEGIN
    CREATE TABLE [AspNetRoleClaims] (
        [Id] int NOT NULL IDENTITY,
        [RoleId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221102152519_InitialMigration'
)
BEGIN
    CREATE TABLE [AspNetUserClaims] (
        [Id] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221102152519_InitialMigration'
)
BEGIN
    CREATE TABLE [AspNetUserLogins] (
        [LoginProvider] nvarchar(450) NOT NULL,
        [ProviderKey] nvarchar(450) NOT NULL,
        [ProviderDisplayName] nvarchar(max) NULL,
        [UserId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221102152519_InitialMigration'
)
BEGIN
    CREATE TABLE [AspNetUserRoles] (
        [UserId] nvarchar(450) NOT NULL,
        [RoleId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221102152519_InitialMigration'
)
BEGIN
    CREATE TABLE [AspNetUserTokens] (
        [UserId] nvarchar(450) NOT NULL,
        [LoginProvider] nvarchar(450) NOT NULL,
        [Name] nvarchar(450) NOT NULL,
        [Value] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221102152519_InitialMigration'
)
BEGIN
    CREATE TABLE [Tasks] (
        [Id] int NOT NULL IDENTITY,
        [Description] nvarchar(max) NULL,
        [StepNumber] nvarchar(max) NULL,
        [TasksResponseId] int NULL,
        CONSTRAINT [PK_Tasks] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221102152519_InitialMigration'
)
BEGIN
    CREATE TABLE [Responses] (
        [Id] int NOT NULL IDENTITY,
        [TasksId] int NOT NULL,
        [RespondantName] nvarchar(max) NULL,
        [Score] int NOT NULL,
        [IsComplete] bit NOT NULL,
        CONSTRAINT [PK_Responses] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Responses_Tasks_TasksId] FOREIGN KEY ([TasksId]) REFERENCES [Tasks] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221102152519_InitialMigration'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
        SET IDENTITY_INSERT [AspNetRoles] ON;
    EXEC(N'INSERT INTO [AspNetRoles] ([Id], [ConcurrencyStamp], [Name], [NormalizedName])
    VALUES (N''cac43a6e-f7bb-4448-baaf-1add431ccbbf'', N''8c7b6de9-701d-41e5-bdf8-5598fa4e0ff7'', N''User'', N''USER''),
    (N''cbc43a8e-f7bb-4445-baaf-1add431ffbbf'', N''e53c21d5-8122-4f7e-be63-a7470131edc6'', N''Admin'', N''ADMIN'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
        SET IDENTITY_INSERT [AspNetRoles] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221102152519_InitialMigration'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AccessFailedCount', N'ConcurrencyStamp', N'Discriminator', N'Email', N'EmailConfirmed', N'FirstName', N'LastName', N'LockoutEnabled', N'LockoutEnd', N'Name', N'NormalizedEmail', N'NormalizedUserName', N'PasswordHash', N'PhoneNumber', N'PhoneNumberConfirmed', N'SecurityStamp', N'TwoFactorEnabled', N'UserName') AND [object_id] = OBJECT_ID(N'[AspNetUsers]'))
        SET IDENTITY_INSERT [AspNetUsers] ON;
    EXEC(N'INSERT INTO [AspNetUsers] ([Id], [AccessFailedCount], [ConcurrencyStamp], [Discriminator], [Email], [EmailConfirmed], [FirstName], [LastName], [LockoutEnabled], [LockoutEnd], [Name], [NormalizedEmail], [NormalizedUserName], [PasswordHash], [PhoneNumber], [PhoneNumberConfirmed], [SecurityStamp], [TwoFactorEnabled], [UserName])
    VALUES (N''8e445865-a24d-4543-a6c6-9443d048cdb9'', 0, N''d49b75ba-78b5-4d72-905f-f28974a581e2'', N''ApplicationUser'', N''admin@localhost.com'', CAST(0 AS bit), N''System'', N''Admin'', CAST(0 AS bit), NULL, NULL, N''ADMIN@LOCALHOST.COM'', N''ADMIN'', N''AQAAAAEAACcQAAAAEFOLzE95ku5ZZIZ6bkEWOzo8ImZbwIDLQivA73D3Bq5KVQFDENym4W3yZstEREQ1pQ=='', NULL, CAST(0 AS bit), N''bba81f47-327f-41af-9e4a-66088dc6f621'', CAST(0 AS bit), N''admin''),
    (N''9e224968-33e4-4652-b7b7-8574d048cdb9'', 0, N''305d0040-ddc2-41d3-8cf0-8673f3b27cb9'', N''ApplicationUser'', N''user@localhost.com'', CAST(0 AS bit), N''System'', N''User'', CAST(0 AS bit), NULL, NULL, N''USER@LOCALHOST.COM'', N''USER'', N''AQAAAAEAACcQAAAAEPUKZecsacfzMDYzF34i/opOn+2sQ9JG68u2NnCk9H6NpMZP+JGp8kmPMM+Nd09T7g=='', NULL, CAST(0 AS bit), N''82074c9b-def8-48c0-896c-49b3452e1794'', CAST(0 AS bit), N''user'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AccessFailedCount', N'ConcurrencyStamp', N'Discriminator', N'Email', N'EmailConfirmed', N'FirstName', N'LastName', N'LockoutEnabled', N'LockoutEnd', N'Name', N'NormalizedEmail', N'NormalizedUserName', N'PasswordHash', N'PhoneNumber', N'PhoneNumberConfirmed', N'SecurityStamp', N'TwoFactorEnabled', N'UserName') AND [object_id] = OBJECT_ID(N'[AspNetUsers]'))
        SET IDENTITY_INSERT [AspNetUsers] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221102152519_InitialMigration'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RoleId', N'UserId') AND [object_id] = OBJECT_ID(N'[AspNetUserRoles]'))
        SET IDENTITY_INSERT [AspNetUserRoles] ON;
    EXEC(N'INSERT INTO [AspNetUserRoles] ([RoleId], [UserId])
    VALUES (N''cac43a6e-f7bb-4448-baaf-1add431ccbbf'', N''9e224968-33e4-4652-b7b7-8574d048cdb9'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RoleId', N'UserId') AND [object_id] = OBJECT_ID(N'[AspNetUserRoles]'))
        SET IDENTITY_INSERT [AspNetUserRoles] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221102152519_InitialMigration'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RoleId', N'UserId') AND [object_id] = OBJECT_ID(N'[AspNetUserRoles]'))
        SET IDENTITY_INSERT [AspNetUserRoles] ON;
    EXEC(N'INSERT INTO [AspNetUserRoles] ([RoleId], [UserId])
    VALUES (N''cbc43a8e-f7bb-4445-baaf-1add431ffbbf'', N''8e445865-a24d-4543-a6c6-9443d048cdb9'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RoleId', N'UserId') AND [object_id] = OBJECT_ID(N'[AspNetUserRoles]'))
        SET IDENTITY_INSERT [AspNetUserRoles] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221102152519_InitialMigration'
)
BEGIN
    CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221102152519_InitialMigration'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221102152519_InitialMigration'
)
BEGIN
    CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221102152519_InitialMigration'
)
BEGIN
    CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221102152519_InitialMigration'
)
BEGIN
    CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221102152519_InitialMigration'
)
BEGIN
    CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221102152519_InitialMigration'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221102152519_InitialMigration'
)
BEGIN
    CREATE INDEX [IX_Responses_TasksId] ON [Responses] ([TasksId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221102152519_InitialMigration'
)
BEGIN
    CREATE INDEX [IX_Tasks_TasksResponseId] ON [Tasks] ([TasksResponseId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221102152519_InitialMigration'
)
BEGIN
    ALTER TABLE [Tasks] ADD CONSTRAINT [FK_Tasks_Responses_TasksResponseId] FOREIGN KEY ([TasksResponseId]) REFERENCES [Responses] ([Id]) ON DELETE NO ACTION;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221102152519_InitialMigration'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221102152519_InitialMigration', N'8.0.2');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20230315210247_addedOtherfields'
)
BEGIN
    ALTER TABLE [AspNetUsers] ADD [ApplicationServer] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20230315210247_addedOtherfields'
)
BEGIN
    ALTER TABLE [AspNetUsers] ADD [ClientId] int NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20230315210247_addedOtherfields'
)
BEGIN
    ALTER TABLE [AspNetUsers] ADD [UserId] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20230315210247_addedOtherfields'
)
BEGIN
    EXEC(N'UPDATE [AspNetRoles] SET [ConcurrencyStamp] = N''e71fcb7a-1885-4c3e-8296-e5eeae58e335''
    WHERE [Id] = N''cac43a6e-f7bb-4448-baaf-1add431ccbbf'';
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20230315210247_addedOtherfields'
)
BEGIN
    EXEC(N'UPDATE [AspNetRoles] SET [ConcurrencyStamp] = N''4c1e116c-b85d-4102-88a7-3d1cbbd34082''
    WHERE [Id] = N''cbc43a8e-f7bb-4445-baaf-1add431ffbbf'';
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20230315210247_addedOtherfields'
)
BEGIN
    EXEC(N'UPDATE [AspNetUsers] SET [ApplicationServer] = N''e45z.4.ucc.md/sap'', [ClientId] = 111, [ConcurrencyStamp] = N''2d0c5b1e-aaf9-418c-86a2-906699cd77b8'', [PasswordHash] = N''AQAAAAEAACcQAAAAELu639vQFI9D8oR+0bJI8wO/4FAol1oNet0s12YfDxRtTtWvyrB6rtGlph0FwrPn2w=='', [SecurityStamp] = N''400e6f05-79a4-4edb-b3c8-f9719fec2121'', [UserId] = N''Learn-031''
    WHERE [Id] = N''8e445865-a24d-4543-a6c6-9443d048cdb9'';
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20230315210247_addedOtherfields'
)
BEGIN
    EXEC(N'UPDATE [AspNetUsers] SET [ApplicationServer] = N''e45z.4.ucc.md/sap'', [ClientId] = 111, [ConcurrencyStamp] = N''6fd403ba-9348-497f-a2cd-f9cca62db983'', [PasswordHash] = N''AQAAAAEAACcQAAAAEOuk3/OzdIb/1KwPNEF+zuSRZR4Fl+QSgFcUUcBb/i0gsAhdOFj1yX7/GiK1Ohs9tw=='', [SecurityStamp] = N''779eb3ea-f2f8-41af-ba64-3ddc7c9ceaca'', [UserId] = N''Learn-031''
    WHERE [Id] = N''9e224968-33e4-4652-b7b7-8574d048cdb9'';
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20230315210247_addedOtherfields'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230315210247_addedOtherfields', N'8.0.2');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20230326234910_AddedLeaderBoard'
)
BEGIN
    CREATE TABLE [LeaderBoaders] (
        [Id] int NOT NULL IDENTITY,
        [Username] nvarchar(max) NULL,
        [Point] int NOT NULL,
        [CaseStudy] nvarchar(max) NULL,
        CONSTRAINT [PK_LeaderBoaders] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20230326234910_AddedLeaderBoard'
)
BEGIN
    EXEC(N'UPDATE [AspNetRoles] SET [ConcurrencyStamp] = N''18db18c8-ccf9-4742-8d36-0b1dabafd970''
    WHERE [Id] = N''cac43a6e-f7bb-4448-baaf-1add431ccbbf'';
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20230326234910_AddedLeaderBoard'
)
BEGIN
    EXEC(N'UPDATE [AspNetRoles] SET [ConcurrencyStamp] = N''9bada91b-b41e-49d3-ba11-f5f44daca066''
    WHERE [Id] = N''cbc43a8e-f7bb-4445-baaf-1add431ffbbf'';
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20230326234910_AddedLeaderBoard'
)
BEGIN
    EXEC(N'UPDATE [AspNetUsers] SET [ConcurrencyStamp] = N''44187eea-3cd6-4342-b8a2-9ee52567beb7'', [PasswordHash] = N''AQAAAAEAACcQAAAAEFFaPLcnRuMtDXVXZLlOsYYSTlHBqMm4M2Tj/PCOFH32AJ7+XH5e470ukL7xXbJasg=='', [SecurityStamp] = N''0e2ac0e3-d5c8-4e9c-b55a-4c2adcff57ce''
    WHERE [Id] = N''8e445865-a24d-4543-a6c6-9443d048cdb9'';
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20230326234910_AddedLeaderBoard'
)
BEGIN
    EXEC(N'UPDATE [AspNetUsers] SET [ConcurrencyStamp] = N''b0783538-24ff-462c-aec2-be1286231644'', [PasswordHash] = N''AQAAAAEAACcQAAAAEM5McagOe+v22v2/awBh5elrqUjqZ8kyhBP5HKWUTHzRNSfQH3mISIHbj2OuZGHKcA=='', [SecurityStamp] = N''ab43f5ec-ea35-4fac-923b-01bdd0edc895''
    WHERE [Id] = N''9e224968-33e4-4652-b7b7-8574d048cdb9'';
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20230326234910_AddedLeaderBoard'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230326234910_AddedLeaderBoard', N'8.0.2');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20230326235748_ModifiedLeaderBoard'
)
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[LeaderBoaders]') AND [c].[name] = N'Point');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [LeaderBoaders] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [LeaderBoaders] ALTER COLUMN [Point] int NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20230326235748_ModifiedLeaderBoard'
)
BEGIN
    EXEC(N'UPDATE [AspNetRoles] SET [ConcurrencyStamp] = N''a17d64c5-d3b5-40cd-af2f-04386a783a1a''
    WHERE [Id] = N''cac43a6e-f7bb-4448-baaf-1add431ccbbf'';
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20230326235748_ModifiedLeaderBoard'
)
BEGIN
    EXEC(N'UPDATE [AspNetRoles] SET [ConcurrencyStamp] = N''596c46d2-9546-438b-9d82-03ad2002fe7d''
    WHERE [Id] = N''cbc43a8e-f7bb-4445-baaf-1add431ffbbf'';
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20230326235748_ModifiedLeaderBoard'
)
BEGIN
    EXEC(N'UPDATE [AspNetUsers] SET [ConcurrencyStamp] = N''776271d5-eab9-4283-a5c3-5e516fce02fb'', [PasswordHash] = N''AQAAAAEAACcQAAAAEH09C2/lRxNhBw8hKrP8A8o3yzRUXY55+ZFZb3FTayvzbuOP4naWm9lA1gQW47MxUQ=='', [SecurityStamp] = N''70a3c9ec-46eb-4ad2-9832-d98864db8a12''
    WHERE [Id] = N''8e445865-a24d-4543-a6c6-9443d048cdb9'';
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20230326235748_ModifiedLeaderBoard'
)
BEGIN
    EXEC(N'UPDATE [AspNetUsers] SET [ConcurrencyStamp] = N''6ddf598f-57fa-4288-a9f7-9fd0f2f01334'', [PasswordHash] = N''AQAAAAEAACcQAAAAECGZC0tB741pd5ISdFhSN2I4SwZfRPZzzaT9pEs8bT0cINboVUTBDAq+7aelAGuqtQ=='', [SecurityStamp] = N''7500f7f6-3504-40f1-b220-133f886b4315''
    WHERE [Id] = N''9e224968-33e4-4652-b7b7-8574d048cdb9'';
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20230326235748_ModifiedLeaderBoard'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230326235748_ModifiedLeaderBoard', N'8.0.2');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240213191123_AddDevData'
)
BEGIN
    EXEC(N'DELETE FROM [AspNetUserRoles]
    WHERE [RoleId] = N''cbc43a8e-f7bb-4445-baaf-1add431ffbbf'' AND [UserId] = N''8e445865-a24d-4543-a6c6-9443d048cdb9'';
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240213191123_AddDevData'
)
BEGIN
    EXEC(N'DELETE FROM [AspNetUserRoles]
    WHERE [RoleId] = N''cac43a6e-f7bb-4448-baaf-1add431ccbbf'' AND [UserId] = N''9e224968-33e4-4652-b7b7-8574d048cdb9'';
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240213191123_AddDevData'
)
BEGIN
    EXEC(N'DELETE FROM [AspNetUsers]
    WHERE [Id] = N''8e445865-a24d-4543-a6c6-9443d048cdb9'';
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240213191123_AddDevData'
)
BEGIN
    EXEC(N'DELETE FROM [AspNetUsers]
    WHERE [Id] = N''9e224968-33e4-4652-b7b7-8574d048cdb9'';
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240213191123_AddDevData'
)
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUsers]') AND [c].[name] = N'Discriminator');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [AspNetUsers] ALTER COLUMN [Discriminator] nvarchar(21) NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240213191123_AddDevData'
)
BEGIN
    EXEC(N'UPDATE [AspNetRoles] SET [ConcurrencyStamp] = NULL
    WHERE [Id] = N''cac43a6e-f7bb-4448-baaf-1add431ccbbf'';
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240213191123_AddDevData'
)
BEGIN
    EXEC(N'UPDATE [AspNetRoles] SET [ConcurrencyStamp] = NULL
    WHERE [Id] = N''cbc43a8e-f7bb-4445-baaf-1add431ffbbf'';
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240213191123_AddDevData'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240213191123_AddDevData', N'8.0.2');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240514211332_TulipSpring2024'
)
BEGIN
    ALTER TABLE [AspNetUsers] ADD [AvatarUrl] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240514211332_TulipSpring2024'
)
BEGIN
    CREATE TABLE [AIChatMessages] (
        [Id] int NOT NULL IDENTITY,
        [Timestamp] datetime2 NOT NULL,
        [Message] nvarchar(max) NOT NULL,
        [UserId] nvarchar(450) NOT NULL,
        [IsFromUser] bit NOT NULL,
        CONSTRAINT [PK_AIChatMessages] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AIChatMessages_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240514211332_TulipSpring2024'
)
BEGIN
    CREATE TABLE [ChatMessages] (
        [Id] int NOT NULL IDENTITY,
        [Timestamp] datetime2 NOT NULL,
        [SenderId] nvarchar(450) NOT NULL,
        [ReceiverId] nvarchar(450) NOT NULL,
        [Message] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_ChatMessages] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ChatMessages_AspNetUsers_ReceiverId] FOREIGN KEY ([ReceiverId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_ChatMessages_AspNetUsers_SenderId] FOREIGN KEY ([SenderId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240514211332_TulipSpring2024'
)
BEGIN
    CREATE TABLE [FloatingChats] (
        [Id] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NOT NULL,
        [OtherUserName] nvarchar(max) NOT NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_FloatingChats] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_FloatingChats_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240514211332_TulipSpring2024'
)
BEGIN
    CREATE INDEX [IX_AIChatMessages_UserId] ON [AIChatMessages] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240514211332_TulipSpring2024'
)
BEGIN
    CREATE INDEX [IX_ChatMessages_ReceiverId] ON [ChatMessages] ([ReceiverId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240514211332_TulipSpring2024'
)
BEGIN
    CREATE INDEX [IX_ChatMessages_SenderId] ON [ChatMessages] ([SenderId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240514211332_TulipSpring2024'
)
BEGIN
    CREATE INDEX [IX_FloatingChats_UserId] ON [FloatingChats] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240514211332_TulipSpring2024'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240514211332_TulipSpring2024', N'8.0.2');
END;
GO

COMMIT;
GO

