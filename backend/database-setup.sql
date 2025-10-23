-- Create database (run this first if the database doesn't exist)
CREATE DATABASE NotesDb;
GO

USE NotesDb;
GO

-- Create Users table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' AND xtype='U')
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

-- Create Notes table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Notes' AND xtype='U')
CREATE TABLE Notes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(200) NOT NULL,
    Content NVARCHAR(MAX),
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UserId INT NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
);

-- Create indexes for better performance
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Notes_UserId')
CREATE INDEX IX_Notes_UserId ON Notes(UserId);

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Notes_CreatedAt')
CREATE INDEX IX_Notes_CreatedAt ON Notes(CreatedAt DESC);

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Notes_UpdatedAt')
CREATE INDEX IX_Notes_UpdatedAt ON Notes(UpdatedAt DESC);

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Users_Username')
CREATE INDEX IX_Users_Username ON Users(Username);

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Users_Email')
CREATE INDEX IX_Users_Email ON Users(Email);

PRINT 'Database setup completed successfully!';
