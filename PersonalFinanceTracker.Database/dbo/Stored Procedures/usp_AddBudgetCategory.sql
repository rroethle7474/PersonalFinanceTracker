
-- 4. Budget Category Procedures
-- ----------------------------------------

-- Add budget category
CREATE PROCEDURE usp_AddBudgetCategory
    @UserID INT,
    @CategoryName NVARCHAR(100),
    @CategoryType NVARCHAR(20),
    @Description NVARCHAR(255) = NULL,
    @MonthlyAllocation DECIMAL(18,2) = NULL,
    @ParentCategoryID INT = NULL,
    @SalesforceID NVARCHAR(20) = NULL
AS
BEGIN
    INSERT INTO BudgetCategories (
        UserID, CategoryName, CategoryType, Description, 
        MonthlyAllocation, ParentCategoryID, SalesforceID
    )
    VALUES (
        @UserID, @CategoryName, @CategoryType, @Description, 
        @MonthlyAllocation, @ParentCategoryID, @SalesforceID
    );
    
    SELECT SCOPE_IDENTITY() AS CategoryID;
END