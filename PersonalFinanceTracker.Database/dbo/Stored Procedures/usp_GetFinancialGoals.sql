
-- Get financial goals
CREATE PROCEDURE usp_GetFinancialGoals
    @UserID INT,
    @Category NVARCHAR(50) = NULL,
    @Status NVARCHAR(20) = NULL
AS
BEGIN
    SELECT 
        GoalID, GoalName, TargetAmount, CurrentAmount, StartDate, TargetDate,
        Category, Priority, Status, Description, MonthlyContribution,
        (CurrentAmount / TargetAmount * 100) AS PercentComplete,
        DATEDIFF(DAY, GETDATE(), TargetDate) AS DaysRemaining
    FROM 
        FinancialGoals
    WHERE 
        UserID = @UserID
        AND (@Category IS NULL OR Category = @Category)
        AND (@Status IS NULL OR Status = @Status)
    ORDER BY 
        Priority, TargetDate;
END