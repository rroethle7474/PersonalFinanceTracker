-- Get a budget category by ID
CREATE PROCEDURE usp_GetCategoryById
    @CategoryID INT
AS
BEGIN
    SELECT 
        CategoryID, UserID, CategoryName, CategoryType, 
        Description, MonthlyAllocation, IsActive, 
        ParentCategoryID, SalesforceID, LastSyncDate
    FROM 
        BudgetCategories
    WHERE 
        CategoryID = @CategoryID;
END