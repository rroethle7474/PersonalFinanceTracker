
-- Delete payment method
CREATE PROCEDURE usp_DeletePaymentMethod
    @PaymentMethodID INT
AS
BEGIN
    UPDATE PaymentMethods
    SET 
        IsActive = 0
    WHERE 
        PaymentMethodID = @PaymentMethodID;
    
    SELECT @@ROWCOUNT AS RowsAffected;
END