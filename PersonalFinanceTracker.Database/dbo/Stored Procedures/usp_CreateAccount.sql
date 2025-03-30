
-- 2. Account Management Procedures
-- ----------------------------------------

-- Create new account
CREATE PROCEDURE usp_CreateAccount
    @UserID INT,
    @AccountName NVARCHAR(100),
    @AccountType NVARCHAR(50),
    @CurrentBalance DECIMAL(18,2),
    @CurrencyCode NVARCHAR(3) = 'USD',
    @SalesforceID NVARCHAR(20) = NULL
AS
BEGIN
    INSERT INTO Accounts (UserID, AccountName, AccountType, CurrentBalance, CurrencyCode, SalesforceID)
    VALUES (@UserID, @AccountName, @AccountType, @CurrentBalance, @CurrencyCode, @SalesforceID);
    
    SELECT SCOPE_IDENTITY() AS AccountID;
END