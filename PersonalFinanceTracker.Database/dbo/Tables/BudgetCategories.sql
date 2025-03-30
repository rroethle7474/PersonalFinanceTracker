CREATE TABLE [dbo].[BudgetCategories] (
    [CategoryID]        INT             IDENTITY (1, 1) NOT NULL,
    [UserID]            INT             NOT NULL,
    [CategoryName]      NVARCHAR (100)  NOT NULL,
    [CategoryType]      NVARCHAR (20)   NOT NULL,
    [Description]       NVARCHAR (255)  NULL,
    [MonthlyAllocation] DECIMAL (18, 2) NULL,
    [IsActive]          BIT             DEFAULT ((1)) NOT NULL,
    [ParentCategoryID]  INT             NULL,
    [SalesforceID]      NVARCHAR (20)   NULL,
    [LastSyncDate]      DATETIME        NULL,
    PRIMARY KEY CLUSTERED ([CategoryID] ASC),
    FOREIGN KEY ([ParentCategoryID]) REFERENCES [dbo].[BudgetCategories] ([CategoryID]),
    FOREIGN KEY ([UserID]) REFERENCES [dbo].[Users] ([UserID])
);

