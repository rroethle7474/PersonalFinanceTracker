
-- Asset allocation
CREATE PROCEDURE usp_GetAssetAllocation
    @UserID INT
AS
BEGIN
    SELECT 
        AssetClass,
        SUM(CurrentPrice * Quantity) AS TotalValue,
        (SUM(CurrentPrice * Quantity) / 
         (SELECT SUM(CurrentPrice * Quantity) FROM Investments WHERE UserID = @UserID)) * 100 AS Percentage
    FROM 
        Investments
    WHERE 
        UserID = @UserID
    GROUP BY 
        AssetClass
    ORDER BY 
        TotalValue DESC;
END