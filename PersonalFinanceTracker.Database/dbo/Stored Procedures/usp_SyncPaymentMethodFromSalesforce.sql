
-- For the SyncFromSalesforce method
CREATE PROCEDURE usp_SyncPaymentMethodFromSalesforce
    @UserID INT,
    @SalesforceID NVARCHAR(20),
    @MethodName NVARCHAR(100),
    @MethodType NVARCHAR(50),
    @CurrentBalance DECIMAL(18,2) = NULL,
    @IsActive BIT = 1
AS
BEGIN
    DECLARE @LocalMethodID INT = NULL;
    
    -- Check if payment method already exists
    SELECT @LocalMethodID = PaymentMethodID
    FROM PaymentMethods
    WHERE SalesforceID = @SalesforceID;
    
    -- Update existing or insert new
    IF @LocalMethodID IS NOT NULL
    BEGIN
        UPDATE PaymentMethods
        SET 
            MethodName = @MethodName,
            MethodType = @MethodType,
            CurrentBalance = @CurrentBalance,
            IsActive = @IsActive,
            LastSyncDate = GETDATE()
        WHERE 
            PaymentMethodID = @LocalMethodID;
            
        -- Log sync operation
        INSERT INTO SalesforceSync (ObjectType, LocalID, SalesforceID, LastSyncDate, SyncStatus)
        VALUES ('Payment_Method__c', @LocalMethodID, @SalesforceID, GETDATE(), 'Success');
        
        SELECT @LocalMethodID AS PaymentMethodID;
    END
    ELSE
    BEGIN
        -- Insert new payment method
        INSERT INTO PaymentMethods (
            UserID, MethodName, MethodType, CurrentBalance, 
            IsActive, SalesforceID, LastSyncDate
        )
        VALUES (
            @UserID, @MethodName, @MethodType, @CurrentBalance, 
            @IsActive, @SalesforceID, GETDATE()
        );
        
        SET @LocalMethodID = SCOPE_IDENTITY();
        
        -- Log sync operation
        INSERT INTO SalesforceSync (ObjectType, LocalID, SalesforceID, LastSyncDate, SyncStatus)
        VALUES ('Payment_Method__c', @LocalMethodID, @SalesforceID, GETDATE(), 'Success');
        
        SELECT @LocalMethodID AS PaymentMethodID;
    END
END