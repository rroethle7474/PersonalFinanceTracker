CREATE TABLE [dbo].[Accounts] (
    [AccountID]              INT             IDENTITY (1, 1) NOT NULL,
    [UserID]                 INT             NOT NULL,
    [AccountName]            NVARCHAR (100)  NOT NULL,
    [AccountType]            NVARCHAR (50)   NOT NULL,
    [CurrentBalance]         DECIMAL (18, 2) DEFAULT ((0)) NOT NULL,
    [CurrencyCode]           NVARCHAR (3)    DEFAULT ('USD') NOT NULL,
    [IsActive]               BIT             DEFAULT ((1)) NOT NULL,
    [SalesforceID]           NVARCHAR (20)   NULL,
    [IsFinancialInstitution] BIT             DEFAULT ((0)) NOT NULL,
    [IsMerchant]             BIT             DEFAULT ((0)) NOT NULL,
    [Category]               NVARCHAR (50)   NULL,
    [LastUpdated]            DATETIME        DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([AccountID] ASC),
    FOREIGN KEY ([UserID]) REFERENCES [dbo].[Users] ([UserID])
);

