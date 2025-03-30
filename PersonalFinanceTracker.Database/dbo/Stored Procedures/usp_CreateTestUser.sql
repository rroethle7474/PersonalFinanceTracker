
-- Create a test user with sample data
CREATE PROCEDURE usp_CreateTestUser
AS
BEGIN
    -- Create test user
    DECLARE @UserID INT;
    
    EXEC usp_CreateUser 
        @Username = 'testuser',
        @Email = 'test@example.com',
        @PasswordHash = 'hashedpassword123',
        @FirstName = 'Test',
        @LastName = 'User';
    
    SET @UserID = SCOPE_IDENTITY();
    
    -- Populate sample data for the test user
    EXEC usp_PopulateSampleData @UserID = @UserID;
    
    SELECT @UserID AS UserID;
END