
-- Update goal progress
CREATE PROCEDURE usp_UpdateGoalProgress
    @GoalID INT,
    @NewAmount DECIMAL(6,2),
    @NewStatus NVARCHAR(20) = NULL
AS
BEGIN
    UPDATE FinancialGoals
    SET 
        CurrentAmount = @NewAmount,
        Status = ISNULL(@NewStatus, Status),
        LastSyncDate = GETDATE()
    WHERE 
        GoalID = @GoalID;
END