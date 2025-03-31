-- Get payment method by ID
CREATE PROCEDURE usp_GetPaymentMethodById
    @PaymentMethodID INT
AS
BEGIN
    SELECT 
        PaymentMethodID, UserID, MethodName, MethodType, 
        CurrentBalance, IsActive, SalesforceID, LastSyncDate
    FROM 
        PaymentMethods
    WHERE 
        PaymentMethodID = @PaymentMethodID;
END