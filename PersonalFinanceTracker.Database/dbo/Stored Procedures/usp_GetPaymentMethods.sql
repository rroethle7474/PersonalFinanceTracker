
-- Get payment methods by user ID
CREATE PROCEDURE usp_GetPaymentMethods
    @UserID INT
AS
BEGIN
    SELECT 
        PaymentMethodID, UserID, MethodName, MethodType, 
        CurrentBalance, IsActive, SalesforceID, LastSyncDate
    FROM 
        PaymentMethods
    WHERE 
        UserID = @UserID
        AND IsActive = 1
    ORDER BY 
        MethodName;
END