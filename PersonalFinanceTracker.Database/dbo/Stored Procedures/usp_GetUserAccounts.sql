
-- Get user accounts
CREATE PROCEDURE usp_GetUserAccounts
    @UserID INT
AS
BEGIN
    SELECT AccountID, AccountName, AccountType, CurrentBalance, CurrencyCode, IsActive, LastUpdated
    FROM Accounts
    WHERE UserID = @UserID AND IsActive = 1
    ORDER BY AccountName;
END