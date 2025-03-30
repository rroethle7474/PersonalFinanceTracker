
-- 6. Investment Procedures
-- ----------------------------------------

-- Add investment
CREATE PROCEDURE usp_AddInvestment
    @UserID INT,
    @AccountID INT = NULL,
    @AssetName NVARCHAR(100),
    @AssetClass NVARCHAR(50),
    @Ticker NVARCHAR(20) = NULL,
    @Quantity DECIMAL(18,6),
    @PurchasePrice DECIMAL(6,2),
    @CurrentPrice DECIMAL(6,2),
    @PurchaseDate DATE,
    @Notes NVARCHAR(255) = NULL,
    @SalesforceID NVARCHAR(20) = NULL
AS
BEGIN
    INSERT INTO Investments (
        UserID, AccountID, AssetName, AssetClass, Ticker, 
        Quantity, PurchasePrice, CurrentPrice, PurchaseDate, Notes, SalesforceID
    )
    VALUES (
        @UserID, @AccountID, @AssetName, @AssetClass, @Ticker, 
        @Quantity, @PurchasePrice, @CurrentPrice, @PurchaseDate, @Notes, @SalesforceID
    );
    
    SELECT SCOPE_IDENTITY() AS InvestmentID;
END