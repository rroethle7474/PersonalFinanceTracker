-- Get user by email address
CREATE PROCEDURE usp_GetUserByEmail
    @Email NVARCHAR(100)
AS
BEGIN
    SELECT 
        UserID, Username, Email, FirstName, LastName, 
        CreatedDate, LastLoginDate
    FROM 
        Users
    WHERE 
        Email = @Email;
END