
-- 8. Reporting Procedures
-- ----------------------------------------

-- Monthly spending by category
CREATE PROCEDURE usp_GetMonthlyCategorySpending
    @UserID INT,
    @Year INT,
    @Month INT
AS
BEGIN
    SELECT 
        bc.CategoryName,
        SUM(t.Amount) AS TotalSpent,
        bc.MonthlyAllocation,
        (bc.MonthlyAllocation - SUM(t.Amount)) AS Remaining,
        CASE 
            WHEN bc.MonthlyAllocation = 0 THEN 0 
            ELSE (SUM(t.Amount) / bc.MonthlyAllocation * 100) 
        END AS PercentUsed
    FROM 
        Transactions t
    INNER JOIN 
        BudgetCategories bc ON t.CategoryID = bc.CategoryID
    WHERE 
        t.UserID = @UserID
        AND t.IsIncome = 0
        AND YEAR(t.TransactionDate) = @Year
        AND MONTH(t.TransactionDate) = @Month
    GROUP BY 
        bc.CategoryID, bc.CategoryName, bc.MonthlyAllocation
    ORDER BY 
        TotalSpent DESC;
END