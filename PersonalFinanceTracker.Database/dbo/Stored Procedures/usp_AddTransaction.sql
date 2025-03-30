
-- 3. Transaction Management Procedures
-- ----------------------------------------

-- Add new transaction
CREATE PROCEDURE usp_AddTransaction
    @UserID INT,
    @AccountID INT,
    @CategoryID INT = NULL,
    @TransactionDate DATETIME,
    @Amount DECIMAL(6,2),
    @MerchantName NVARCHAR(100) = NULL,
    @Description NVARCHAR(255) = NULL,
    @TransactionType NVARCHAR(50),
    @IsIncome BIT
AS
BEGIN
    BEGIN TRANSACTION;
    
    BEGIN TRY
        -- Insert the transaction
        INSERT INTO Transactions (
            UserID, AccountID, CategoryID, TransactionDate, 
            Amount, MerchantName, Description, TransactionType, IsIncome
        )
        VALUES (
            @UserID, @AccountID, @CategoryID, @TransactionDate, 
            @Amount, @MerchantName, @Description, @TransactionType, @IsIncome
        );
        
        DECLARE @TransactionID INT = SCOPE_IDENTITY();
        
        -- Update account balance
        IF @IsIncome = 1
        BEGIN
            UPDATE Accounts
            SET CurrentBalance = CurrentBalance + @Amount, LastUpdated = GETDATE()
            WHERE AccountID = @AccountID;
        END
        ELSE
        BEGIN
            UPDATE Accounts
            SET CurrentBalance = CurrentBalance - @Amount, LastUpdated = GETDATE()
            WHERE AccountID = @AccountID;
        END
        
        COMMIT TRANSACTION;
        
        SELECT @TransactionID AS TransactionID;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END