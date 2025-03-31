
-- Update an existing transaction
CREATE PROCEDURE usp_UpdateTransaction
    @TransactionID INT,
    @CategoryID INT = NULL,
    @TransactionDate DATETIME,
    @Amount DECIMAL(18,2),
    @MerchantName NVARCHAR(100) = NULL,
    @Description NVARCHAR(255) = NULL,
    @TransactionType NVARCHAR(50)
AS
BEGIN
    BEGIN TRANSACTION;
    
    BEGIN TRY
        DECLARE @PreviousAmount DECIMAL(18,2);
        DECLARE @AccountID INT;
        DECLARE @IsIncome BIT;
        
        -- Get previous amount, account ID, and income flag
        SELECT 
            @PreviousAmount = Amount,
            @AccountID = AccountID,
            @IsIncome = IsIncome
        FROM 
            Transactions
        WHERE 
            TransactionID = @TransactionID;
        
        -- Update the transaction
        UPDATE Transactions
        SET 
            CategoryID = @CategoryID,
            TransactionDate = @TransactionDate,
            Amount = @Amount,
            MerchantName = @MerchantName,
            Description = @Description,
            TransactionType = @TransactionType,
            ModifiedDate = GETDATE()
        WHERE 
            TransactionID = @TransactionID;
        
        -- Update account balance based on the difference between old and new amounts
        IF @IsIncome = 1
        BEGIN
            UPDATE Accounts
            SET 
                CurrentBalance = CurrentBalance - @PreviousAmount + @Amount,
                LastUpdated = GETDATE()
            WHERE 
                AccountID = @AccountID;
        END
        ELSE
        BEGIN
            UPDATE Accounts
            SET 
                CurrentBalance = CurrentBalance + @PreviousAmount - @Amount,
                LastUpdated = GETDATE()
            WHERE 
                AccountID = @AccountID;
        END
        
        COMMIT TRANSACTION;
        SELECT 1 AS Success;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END