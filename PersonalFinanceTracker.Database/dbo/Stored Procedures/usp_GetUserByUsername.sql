-- Get user by username
CREATE PROCEDURE usp_GetUserByUsername
    @Username NVARCHAR(50)
AS
BEGIN
    SELECT 
        UserID, Username, Email, FirstName, LastName, 
        CreatedDate, LastLoginDate
    FROM 
        Users
    WHERE 
        Username = @Username;
END