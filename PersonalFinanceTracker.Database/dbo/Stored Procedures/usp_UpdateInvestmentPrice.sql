
-- Update investment price
CREATE PROCEDURE usp_UpdateInvestmentPrice
    @InvestmentID INT,
    @NewPrice DECIMAL(6,2)
AS
BEGIN
    UPDATE Investments
    SET 
        CurrentPrice = @NewPrice,
        LastUpdated = GETDATE()
    WHERE 
        InvestmentID = @InvestmentID;
END