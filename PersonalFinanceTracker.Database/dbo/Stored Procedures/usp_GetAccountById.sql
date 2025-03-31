-- Get account by ID
CREATE PROCEDURE usp_GetAccountById
    @AccountID INT
AS
BEGIN
    SELECT 
        AccountID, UserID, AccountName, AccountType, 
        CurrentBalance, CurrencyCode, IsActive, SalesforceID,
        IsFinancialInstitution, IsMerchant, Category, LastUpdated
    FROM 
        Accounts
    WHERE 
        AccountID = @AccountID;
END