
-- Sync financial goal from Salesforce
CREATE PROCEDURE usp_SyncFinancialGoalFromSalesforce
    @UserID INT,
    @SalesforceID NVARCHAR(20),
    @GoalName NVARCHAR(100),
    @TargetAmount DECIMAL(6,2),
    @CurrentAmount DECIMAL(6,2),
    @StartDate DATE,
    @TargetDate DATE,
    @Category NVARCHAR(50),
    @Priority NVARCHAR(20),
    @Status NVARCHAR(20),
    @Description NVARCHAR(255) = NULL,
    @MonthlyContribution DECIMAL(6,2) = NULL
AS
BEGIN
    DECLARE @LocalGoalID INT = NULL;
    
    -- Check if goal already exists
    SELECT @LocalGoalID = GoalID
    FROM FinancialGoals
    WHERE SalesforceID = @SalesforceID;
    
    -- Update existing or insert new
    IF @LocalGoalID IS NOT NULL
    BEGIN
        UPDATE FinancialGoals
        SET 
            GoalName = @GoalName,
            TargetAmount = @TargetAmount,
            CurrentAmount = @CurrentAmount,
            StartDate = @StartDate,
            TargetDate = @TargetDate,
            Category = @Category,
            Priority = @Priority,
            Status = @Status,
            Description = @Description,
            MonthlyContribution = @MonthlyContribution,
            LastSyncDate = GETDATE()
        WHERE 
            GoalID = @LocalGoalID;
            
        -- Log sync operation
        INSERT INTO SalesforceSync (ObjectType, LocalID, SalesforceID, LastSyncDate, SyncStatus)
        VALUES ('Financial_Goal__c', @LocalGoalID, @SalesforceID, GETDATE(), 'Success');
        
        SELECT @LocalGoalID AS GoalID;
    END
    ELSE
    BEGIN
        -- Insert new goal
        INSERT INTO FinancialGoals (
            UserID, GoalName, TargetAmount, CurrentAmount, StartDate, TargetDate,
            Category, Priority, Status, Description, MonthlyContribution, SalesforceID, LastSyncDate
        )
        VALUES (
            @UserID, @GoalName, @TargetAmount, @CurrentAmount, @StartDate, @TargetDate,
            @Category, @Priority, @Status, @Description, @MonthlyContribution, @SalesforceID, GETDATE()
        );
        
        SET @LocalGoalID = SCOPE_IDENTITY();
        
        -- Log sync operation
        INSERT INTO SalesforceSync (ObjectType, LocalID, SalesforceID, LastSyncDate, SyncStatus)
        VALUES ('Financial_Goal__c', @LocalGoalID, @SalesforceID, GETDATE(), 'Success');
        
        SELECT @LocalGoalID AS GoalID;
    END
END