
-- 7. Salesforce Sync Procedures
-- ----------------------------------------

-- Sync budget category from Salesforce
CREATE PROCEDURE usp_SyncBudgetCategoryFromSalesforce
    @UserID INT,
    @SalesforceID NVARCHAR(20),
    @CategoryName NVARCHAR(100),
    @CategoryType NVARCHAR(20),
    @Description NVARCHAR(255) = NULL,
    @MonthlyAllocation DECIMAL(6,2) = NULL,
    @IsActive BIT = 1,
    @ParentSalesforceID NVARCHAR(20) = NULL
AS
BEGIN
    DECLARE @LocalCategoryID INT = NULL;
    DECLARE @ParentCategoryID INT = NULL;
    
    -- Get parent category ID if provided
    IF @ParentSalesforceID IS NOT NULL
    BEGIN
        SELECT @ParentCategoryID = CategoryID
        FROM BudgetCategories
        WHERE SalesforceID = @ParentSalesforceID;
    END
    
    -- Check if category already exists
    SELECT @LocalCategoryID = CategoryID
    FROM BudgetCategories
    WHERE SalesforceID = @SalesforceID;
    
    -- Update existing or insert new
    IF @LocalCategoryID IS NOT NULL
    BEGIN
        UPDATE BudgetCategories
        SET 
            CategoryName = @CategoryName,
            CategoryType = @CategoryType,
            Description = @Description,
            MonthlyAllocation = @MonthlyAllocation,
            IsActive = @IsActive,
            ParentCategoryID = @ParentCategoryID,
            LastSyncDate = GETDATE()
        WHERE 
            CategoryID = @LocalCategoryID;
            
        -- Log sync operation
        INSERT INTO SalesforceSync (ObjectType, LocalID, SalesforceID, LastSyncDate, SyncStatus)
        VALUES ('Budget_Category__c', @LocalCategoryID, @SalesforceID, GETDATE(), 'Success');
        
        SELECT @LocalCategoryID AS CategoryID;
    END
    ELSE
    BEGIN
        -- Insert new category
        INSERT INTO BudgetCategories (
            UserID, CategoryName, CategoryType, Description, 
            MonthlyAllocation, IsActive, ParentCategoryID, SalesforceID, LastSyncDate
        )
        VALUES (
            @UserID, @CategoryName, @CategoryType, @Description, 
            @MonthlyAllocation, @IsActive, @ParentCategoryID, @SalesforceID, GETDATE()
        );
        
        SET @LocalCategoryID = SCOPE_IDENTITY();
        
        -- Log sync operation
        INSERT INTO SalesforceSync (ObjectType, LocalID, SalesforceID, LastSyncDate, SyncStatus)
        VALUES ('Budget_Category__c', @LocalCategoryID, @SalesforceID, GETDATE(), 'Success');
        
        SELECT @LocalCategoryID AS CategoryID;
    END
END