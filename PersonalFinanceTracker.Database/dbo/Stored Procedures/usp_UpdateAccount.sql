
-- Update an existing account
CREATE PROCEDURE usp_UpdateAccount
    @AccountID INT,
    @AccountName NVARCHAR(100),
    @AccountType NVARCHAR(50),
    @CurrentBalance DECIMAL(18,2),
    @CurrencyCode NVARCHAR(3),
    @IsActive BIT,
    @IsFinancialInstitution BIT,
    @IsMerchant BIT,
    @Category NVARCHAR(50) = NULL
AS
BEGIN
    UPDATE Accounts
    SET 
        AccountName = @AccountName,
        AccountType = @AccountType,
        CurrentBalance = @CurrentBalance,
        CurrencyCode = @CurrencyCode,
        IsActive = @IsActive,
        IsFinancialInstitution = @IsFinancialInstitution,
        IsMerchant = @IsMerchant,
        Category = @Category,
        LastUpdated = GETDATE()
    WHERE 
        AccountID = @AccountID;
    
    SELECT @@ROWCOUNT AS RowsAffected;
END