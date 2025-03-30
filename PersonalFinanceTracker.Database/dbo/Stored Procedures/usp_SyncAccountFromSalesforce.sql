
-- Sync account from Salesforce
CREATE PROCEDURE usp_SyncAccountFromSalesforce
    @UserID INT,
    @SalesforceID NVARCHAR(20),
    @AccountName NVARCHAR(100),
    @AccountType NVARCHAR(50),
    @CurrentBalance DECIMAL(6,2),
    @CurrencyCode NVARCHAR(3) = 'USD',
    @IsActive BIT = 1,
    @IsFinancialInstitution BIT = 0,
    @IsMerchant BIT = 0,
    @Category NVARCHAR(50) = NULL
AS
BEGIN
    DECLARE @LocalAccountID INT = NULL;
    
    -- Check if account already exists
    SELECT @LocalAccountID = AccountID
    FROM Accounts
    WHERE SalesforceID = @SalesforceID;
    
    -- Update existing or insert new
    IF @LocalAccountID IS NOT NULL
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
            AccountID = @LocalAccountID;
            
        -- Log sync operation
        INSERT INTO SalesforceSync (ObjectType, LocalID, SalesforceID, LastSyncDate, SyncStatus)
        VALUES ('Account', @LocalAccountID, @SalesforceID, GETDATE(), 'Success');
        
        SELECT @LocalAccountID AS AccountID;
    END
    ELSE
    BEGIN
        -- Insert new account
        INSERT INTO Accounts (
            UserID, AccountName, AccountType, CurrentBalance, CurrencyCode,
            IsActive, SalesforceID, IsFinancialInstitution, IsMerchant, Category, LastUpdated
        )
        VALUES (
            @UserID, @AccountName, @AccountType, @CurrentBalance, @CurrencyCode,
            @IsActive, @SalesforceID, @IsFinancialInstitution, @IsMerchant, @Category, GETDATE()
        );
        
        SET @LocalAccountID = SCOPE_IDENTITY();
        
        -- Log sync operation
        INSERT INTO SalesforceSync (ObjectType, LocalID, SalesforceID, LastSyncDate, SyncStatus)
        VALUES ('Account', @LocalAccountID, @SalesforceID, GETDATE(), 'Success');
        
        SELECT @LocalAccountID AS AccountID;
    END
END