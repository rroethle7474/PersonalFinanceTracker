CREATE TABLE [dbo].[Transactions] (
    [TransactionID]   INT             IDENTITY (1, 1) NOT NULL,
    [UserID]          INT             NOT NULL,
    [AccountID]       INT             NOT NULL,
    [CategoryID]      INT             NULL,
    [TransactionDate] DATETIME        NOT NULL,
    [Amount]          DECIMAL (18, 2) NOT NULL,
    [MerchantName]    NVARCHAR (100)  NULL,
    [Description]     NVARCHAR (255)  NULL,
    [TransactionType] NVARCHAR (50)   NOT NULL,
    [IsIncome]        BIT             DEFAULT ((0)) NOT NULL,
    [SalesforceID]    NVARCHAR (20)   NULL,
    [CreatedDate]     DATETIME        DEFAULT (getdate()) NULL,
    [ModifiedDate]    DATETIME        DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([TransactionID] ASC),
    FOREIGN KEY ([AccountID]) REFERENCES [dbo].[Accounts] ([AccountID]),
    FOREIGN KEY ([CategoryID]) REFERENCES [dbo].[BudgetCategories] ([CategoryID]),
    FOREIGN KEY ([UserID]) REFERENCES [dbo].[Users] ([UserID])
);

