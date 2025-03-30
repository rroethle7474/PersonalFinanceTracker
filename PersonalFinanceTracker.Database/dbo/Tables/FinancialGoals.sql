CREATE TABLE [dbo].[FinancialGoals] (
    [GoalID]              INT             IDENTITY (1, 1) NOT NULL,
    [UserID]              INT             NOT NULL,
    [GoalName]            NVARCHAR (100)  NOT NULL,
    [TargetAmount]        DECIMAL (18, 2) NOT NULL,
    [CurrentAmount]       DECIMAL (18, 2) DEFAULT ((0)) NOT NULL,
    [StartDate]           DATE            NOT NULL,
    [TargetDate]          DATE            NOT NULL,
    [Category]            NVARCHAR (50)   NOT NULL,
    [Priority]            NVARCHAR (20)   NOT NULL,
    [Status]              NVARCHAR (20)   NOT NULL,
    [Description]         NVARCHAR (255)  NULL,
    [MonthlyContribution] DECIMAL (18, 2) NULL,
    [SalesforceID]        NVARCHAR (20)   NULL,
    [LastSyncDate]        DATETIME        NULL,
    PRIMARY KEY CLUSTERED ([GoalID] ASC),
    FOREIGN KEY ([UserID]) REFERENCES [dbo].[Users] ([UserID])
);

