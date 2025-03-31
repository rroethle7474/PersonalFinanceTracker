CREATE PROCEDURE usp_GetUserById
    @UserID INT
AS
BEGIN
    SELECT 
        UserID, Username, Email, FirstName, LastName, 
        CreatedDate, LastLoginDate
    FROM 
        Users
    WHERE 
        UserID = @UserID;
END