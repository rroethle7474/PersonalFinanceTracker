
-- Income vs Expenses by month
CREATE PROCEDURE usp_GetIncomeVsExpensesByMonth
    @UserID INT,
    @MonthsBack INT = 12
AS
BEGIN
    WITH MonthlyData AS (
        SELECT 
            DATEFROMPARTS(YEAR(TransactionDate), MONTH(TransactionDate), 1) AS MonthDate,
            IsIncome,
            SUM(Amount) AS TotalAmount
        FROM 
            Transactions
        WHERE 
            UserID = @UserID
            AND TransactionDate >= DATEADD(MONTH, -@MonthsBack, GETDATE())
        GROUP BY 
            DATEFROMPARTS(YEAR(TransactionDate), MONTH(TransactionDate), 1),
            IsIncome
    )
    
    SELECT 
        MonthDate,
        FORMAT(MonthDate, 'MMM yyyy') AS MonthYear,
        ISNULL(MAX(CASE WHEN IsIncome = 1 THEN TotalAmount END), 0) AS Income,
        ISNULL(MAX(CASE WHEN IsIncome = 0 THEN TotalAmount END), 0) AS Expenses,
        ISNULL(MAX(CASE WHEN IsIncome = 1 THEN TotalAmount END), 0) - 
        ISNULL(MAX(CASE WHEN IsIncome = 0 THEN TotalAmount END), 0) AS NetSavings
    FROM 
        MonthlyData
    GROUP BY 
        MonthDate
    ORDER BY 
        MonthDate;
END