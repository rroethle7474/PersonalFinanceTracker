
-- Update an existing budget category
CREATE PROCEDURE usp_UpdateBudgetCategory
    @CategoryID INT,
    @CategoryName NVARCHAR(100),
    @CategoryType NVARCHAR(20),
    @Description NVARCHAR(255) = NULL,
    @MonthlyAllocation DECIMAL(18,2) = NULL,
    @ParentCategoryID INT = NULL,
    @IsActive BIT = 1
AS
BEGIN
    UPDATE BudgetCategories
    SET 
        CategoryName = @CategoryName,
        CategoryType = @CategoryType,
        Description = @Description,
        MonthlyAllocation = @MonthlyAllocation,
        ParentCategoryID = @ParentCategoryID,
        IsActive = @IsActive,
        LastSyncDate = NULL -- Set to NULL to indicate changes need to be synced to Salesforce
    WHERE 
        CategoryID = @CategoryID;
    
    SELECT @@ROWCOUNT AS RowsAffected;
END