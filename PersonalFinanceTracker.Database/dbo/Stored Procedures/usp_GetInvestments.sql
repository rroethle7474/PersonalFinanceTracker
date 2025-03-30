
-- Get investments
CREATE PROCEDURE usp_GetInvestments
    @UserID INT,
    @AssetClass NVARCHAR(50) = NULL
AS
BEGIN
    SELECT 
        i.InvestmentID, i.AccountID, i.AssetName, i.AssetClass, i.Ticker, 
        i.Quantity, i.PurchasePrice, i.CurrentPrice, i.PurchaseDate,
        (i.CurrentPrice * i.Quantity) AS CurrentValue,
        (i.CurrentPrice * i.Quantity) - (i.PurchasePrice * i.Quantity) AS ProfitLoss,
        (((i.CurrentPrice * i.Quantity) - (i.PurchasePrice * i.Quantity)) / (i.PurchasePrice * i.Quantity) * 100) AS ReturnPercentage,
        a.AccountName
    FROM 
        Investments i
    LEFT JOIN 
        Accounts a ON i.AccountID = a.AccountID
    WHERE 
        i.UserID = @UserID
        AND (@AssetClass IS NULL OR i.AssetClass = @AssetClass)
    ORDER BY 
        i.AssetClass, i.AssetName;
END