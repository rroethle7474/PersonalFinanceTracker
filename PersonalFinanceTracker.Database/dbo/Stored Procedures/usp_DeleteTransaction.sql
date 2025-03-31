
-- Delete a transaction
CREATE PROCEDURE usp_DeleteTransaction
    @TransactionID INT
AS
BEGIN
    BEGIN TRANSACTION;
    
    BEGIN TRY
        DECLARE @Amount DECIMAL(18,2);
        DECLARE @AccountID INT;
        DECLARE @IsIncome BIT;
        
        -- Get transaction details before deletion
        SELECT 
            @Amount = Amount,
            @AccountID = AccountID,
            @IsIncome = IsIncome
        FROM 
            Transactions
        WHERE 
            TransactionID = @TransactionID;
        
        -- Adjust account balance
        IF @IsIncome = 1
        BEGIN
            UPDATE Accounts
            SET 
                CurrentBalance = CurrentBalance - @Amount,
                LastUpdated = GETDATE()
            WHERE 
                AccountID = @AccountID;
        END
        ELSE
        BEGIN
            UPDATE Accounts
            SET 
                CurrentBalance = CurrentBalance + @Amount,
                LastUpdated = GETDATE()
            WHERE 
                AccountID = @AccountID;
        END
        
        -- Delete the transaction
        DELETE FROM Transactions
        WHERE TransactionID = @TransactionID;
        
        COMMIT TRANSACTION;
        SELECT 1 AS Success;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END