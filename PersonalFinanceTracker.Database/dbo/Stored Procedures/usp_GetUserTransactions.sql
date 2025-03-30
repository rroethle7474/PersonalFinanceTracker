
-- Get user transactions
CREATE PROCEDURE usp_GetUserTransactions
    @UserID INT,
    @StartDate DATE = NULL,
    @EndDate DATE = NULL,
    @CategoryID INT = NULL,
    @AccountID INT = NULL,
    @IsIncome BIT = NULL
AS
BEGIN
    SELECT 
        t.TransactionID, t.AccountID, t.CategoryID, t.TransactionDate, 
        t.Amount, t.MerchantName, t.Description, t.TransactionType, t.IsIncome,
        a.AccountName, bc.CategoryName
    FROM 
        Transactions t
    INNER JOIN 
        Accounts a ON t.AccountID = a.AccountID
    LEFT JOIN 
        BudgetCategories bc ON t.CategoryID = bc.CategoryID
    WHERE 
        t.UserID = @UserID
        AND (@StartDate IS NULL OR t.TransactionDate >= @StartDate)
        AND (@EndDate IS NULL OR t.TransactionDate <= @EndDate)
        AND (@CategoryID IS NULL OR t.CategoryID = @CategoryID)
        AND (@AccountID IS NULL OR t.AccountID = @AccountID)
        AND (@IsIncome IS NULL OR t.IsIncome = @IsIncome)
    ORDER BY 
        t.TransactionDate DESC;
END