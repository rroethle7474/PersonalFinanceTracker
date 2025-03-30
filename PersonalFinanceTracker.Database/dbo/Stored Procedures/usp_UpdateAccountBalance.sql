
-- Update account balance
CREATE PROCEDURE usp_UpdateAccountBalance
    @AccountID INT,
    @NewBalance DECIMAL(18,2)
AS
BEGIN
    UPDATE Accounts
    SET CurrentBalance = @NewBalance, LastUpdated = GETDATE()
    WHERE AccountID = @AccountID;
END