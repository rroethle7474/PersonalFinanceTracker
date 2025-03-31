-- Get transaction by ID
CREATE PROCEDURE usp_GetTransactionById
    @TransactionID INT
AS
BEGIN
    SELECT 
        t.TransactionID, t.UserID, t.AccountID, t.CategoryID, 
        t.TransactionDate, t.Amount, t.MerchantName, t.Description, 
        t.TransactionType, t.IsIncome, t.SalesforceID, 
        t.CreatedDate, t.ModifiedDate,
        a.AccountName, bc.CategoryName
    FROM 
        Transactions t
    INNER JOIN 
        Accounts a ON t.AccountID = a.AccountID
    LEFT JOIN 
        BudgetCategories bc ON t.CategoryID = bc.CategoryID
    WHERE 
        t.TransactionID = @TransactionID;
END