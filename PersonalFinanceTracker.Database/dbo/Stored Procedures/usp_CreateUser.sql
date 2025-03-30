
-- ========================================
-- STORED PROCEDURES
-- ========================================

-- 1. User Management Procedures
-- ----------------------------------------

-- Create new user
CREATE PROCEDURE usp_CreateUser
    @Username NVARCHAR(50),
    @Email NVARCHAR(100),
    @PasswordHash NVARCHAR(128),
    @FirstName NVARCHAR(50) = NULL,
    @LastName NVARCHAR(50) = NULL
AS
BEGIN
    INSERT INTO Users (Username, Email, PasswordHash, FirstName, LastName)
    VALUES (@Username, @Email, @PasswordHash, @FirstName, @LastName);
    
    SELECT SCOPE_IDENTITY() AS UserID;
END