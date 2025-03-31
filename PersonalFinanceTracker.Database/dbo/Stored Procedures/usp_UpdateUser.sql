-- Update existing user
CREATE PROCEDURE usp_UpdateUser
    @UserID INT,
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @Email NVARCHAR(100),
    @LastLoginDate DATETIME = NULL
AS
BEGIN
    -- Check if email already exists for another user
    IF EXISTS (SELECT 1 FROM Users WHERE Email = @Email AND UserID != @UserID)
    BEGIN
        -- Return 0 to indicate failure - email already in use
        RETURN 0;
    END
    
    UPDATE Users
    SET 
        FirstName = @FirstName,
        LastName = @LastName,
        Email = @Email,
        LastLoginDate = ISNULL(@LastLoginDate, LastLoginDate)
    WHERE 
        UserID = @UserID;
    
    -- Return number of rows affected
    RETURN @@ROWCOUNT;
END