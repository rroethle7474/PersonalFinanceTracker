
-- Update payment method
CREATE PROCEDURE usp_UpdatePaymentMethod
    @PaymentMethodID INT,
    @MethodName NVARCHAR(100),
    @MethodType NVARCHAR(50),
    @CurrentBalance DECIMAL(18,2) = NULL,
    @IsActive BIT = 1
AS
BEGIN
    UPDATE PaymentMethods
    SET 
        MethodName = @MethodName,
        MethodType = @MethodType,
        CurrentBalance = @CurrentBalance,
        IsActive = @IsActive,
        LastSyncDate = NULL
    WHERE 
        PaymentMethodID = @PaymentMethodID;
    
    SELECT @@ROWCOUNT AS RowsAffected;
END