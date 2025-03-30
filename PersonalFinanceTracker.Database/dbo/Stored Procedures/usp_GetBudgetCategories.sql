
-- Get budget categories
CREATE PROCEDURE usp_GetBudgetCategories
    @UserID INT,
    @CategoryType NVARCHAR(20) = NULL
AS
BEGIN
    SELECT 
        CategoryID, CategoryName, CategoryType, Description, 
        MonthlyAllocation, IsActive, ParentCategoryID, SalesforceID
    FROM 
        BudgetCategories
    WHERE 
        UserID = @UserID
        AND IsActive = 1
        AND (@CategoryType IS NULL OR CategoryType = @CategoryType)
    ORDER BY 
        CategoryName;
END