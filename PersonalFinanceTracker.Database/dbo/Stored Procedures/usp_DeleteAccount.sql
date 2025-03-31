
-- Delete an account (soft delete by setting IsActive = 0)
CREATE PROCEDURE usp_DeleteAccount
    @AccountID INT
AS
BEGIN
    BEGIN TRANSACTION;
    
    BEGIN TRY
        -- Check if there are any active transactions for this account
        IF EXISTS (SELECT 1 FROM Transactions WHERE AccountID = @AccountID)
        BEGIN
            -- Soft delete the account instead of removing it
            UPDATE Accounts
            SET 
                IsActive = 0,
                LastUpdated = GETDATE()
            WHERE 
                AccountID = @AccountID;
        END
        ELSE
        BEGIN
            -- If no transactions, we can safely hard delete
            -- But for consistency, we'll still do a soft delete
            UPDATE Accounts
            SET 
                IsActive = 0,
                LastUpdated = GETDATE()
            WHERE 
                AccountID = @AccountID;
        END
        
        COMMIT TRANSACTION;
        SELECT @@ROWCOUNT AS RowsAffected;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END