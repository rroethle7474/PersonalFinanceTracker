
-- Create new payment method
CREATE PROCEDURE usp_CreatePaymentMethod
    @UserID INT,
    @MethodName NVARCHAR(100),
    @MethodType NVARCHAR(50),
    @CurrentBalance DECIMAL(18,2) = NULL,
    @IsActive BIT = 1,
    @SalesforceID NVARCHAR(20) = NULL
AS
BEGIN
    INSERT INTO PaymentMethods (
        UserID, MethodName, MethodType, CurrentBalance, 
        IsActive, SalesforceID
    )
    VALUES (
        @UserID, @MethodName, @MethodType, @CurrentBalance, 
        @IsActive, @SalesforceID
    );
    
    SELECT SCOPE_IDENTITY() AS PaymentMethodID;
END