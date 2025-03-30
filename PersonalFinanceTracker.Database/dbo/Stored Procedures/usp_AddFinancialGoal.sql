
-- 5. Financial Goal Procedures
-- ----------------------------------------

-- Add financial goal
CREATE PROCEDURE usp_AddFinancialGoal
    @UserID INT,
    @GoalName NVARCHAR(100),
    @TargetAmount DECIMAL(6,2),
    @CurrentAmount DECIMAL(6,2),
    @StartDate DATE,
    @TargetDate DATE,
    @Category NVARCHAR(50),
    @Priority NVARCHAR(20),
    @Status NVARCHAR(20),
    @Description NVARCHAR(255) = NULL,
    @MonthlyContribution DECIMAL(6,2) = NULL,
    @SalesforceID NVARCHAR(20) = NULL
AS
BEGIN
    INSERT INTO FinancialGoals (
        UserID, GoalName, TargetAmount, CurrentAmount, StartDate, TargetDate,
        Category, Priority, Status, Description, MonthlyContribution, SalesforceID
    )
    VALUES (
        @UserID, @GoalName, @TargetAmount, @CurrentAmount, @StartDate, @TargetDate,
        @Category, @Priority, @Status, @Description, @MonthlyContribution, @SalesforceID
    );
    
    SELECT SCOPE_IDENTITY() AS GoalID;
END