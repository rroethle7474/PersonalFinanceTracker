
-- Authenticate user
CREATE PROCEDURE usp_AuthenticateUser
    @Username NVARCHAR(50),
    @PasswordHash NVARCHAR(128)
AS
BEGIN
    DECLARE @UserID INT = NULL;
    
    SELECT @UserID = UserID
    FROM Users
    WHERE Username = @Username AND PasswordHash = @PasswordHash;
    
    IF @UserID IS NOT NULL
    BEGIN
        UPDATE Users SET LastLoginDate = GETDATE()
        WHERE UserID = @UserID;
        
        SELECT UserID, Username, Email, FirstName, LastName
        FROM Users
        WHERE UserID = @UserID;
    END
END