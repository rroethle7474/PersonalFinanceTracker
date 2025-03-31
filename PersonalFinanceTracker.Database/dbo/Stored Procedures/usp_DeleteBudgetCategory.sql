
-- Delete a budget category (soft delete by setting IsActive = 0)
CREATE PROCEDURE usp_DeleteBudgetCategory
    @CategoryID INT
AS
BEGIN
    -- First, check if the category is a parent to other categories
    IF EXISTS (SELECT 1 FROM BudgetCategories WHERE ParentCategoryID = @CategoryID)
    BEGIN
        -- Soft delete any child categories
        UPDATE BudgetCategories
        SET 
            IsActive = 0,
            LastSyncDate = NULL
        WHERE 
            ParentCategoryID = @CategoryID;
    END
    
    -- Then soft delete the category itself
    UPDATE BudgetCategories
    SET 
        IsActive = 0,
        LastSyncDate = NULL
    WHERE 
        CategoryID = @CategoryID;
    
    SELECT @@ROWCOUNT AS RowsAffected;
END