CREATE PROCEDURE usp_GetNetWorthTrend
    @UserID INT,
    @MonthsBack INT = 12
AS
BEGIN
    -- Create a table of months to ensure we have data for each month
    DECLARE @Months TABLE (MonthDate DATE);
    
    DECLARE @StartDate DATE = DATEADD(MONTH, -@MonthsBack, GETDATE());
    DECLARE @CurrentDate DATE = GETDATE();
    
    WHILE @StartDate <= @CurrentDate
    BEGIN
        INSERT INTO @Months (MonthDate)
        VALUES (DATEFROMPARTS(YEAR(@StartDate), MONTH(@StartDate), 1));
        
        SET @StartDate = DATEADD(MONTH, 1, @StartDate);
    END
    
    -- Get account balances at the end of each month
    ;WITH AccountBalances AS (
        SELECT 
            a.AccountID,
            a.AccountType,
            m.MonthDate,
            -- For simplicity, we're using current balance for historical data
            -- In a real app, you'd track balance history
            a.CurrentBalance
        FROM 
            @Months m
        CROSS JOIN 
            Accounts a
        WHERE 
            a.UserID = @UserID
            AND a.IsActive = 1
    )
    
    SELECT 
        ab.MonthDate,
        FORMAT(ab.MonthDate, 'MMM yyyy') AS MonthYear,
        SUM(CASE WHEN ab.AccountType IN ('Credit Card', 'Loan') THEN -ab.CurrentBalance ELSE ab.CurrentBalance END) AS NetWorth,
        SUM(CASE WHEN ab.AccountType IN ('Checking', 'Savings', 'Cash') THEN ab.CurrentBalance ELSE 0 END) AS LiquidAssets,
        SUM(CASE WHEN ab.AccountType = 'Investment' THEN ab.CurrentBalance ELSE 0 END) AS Investments,
        SUM(CASE WHEN ab.AccountType IN ('Credit Card', 'Loan') THEN ab.CurrentBalance ELSE 0 END) AS Debt
    FROM 
        AccountBalances ab
    GROUP BY 
        ab.MonthDate
    ORDER BY 
        ab.MonthDate;
END