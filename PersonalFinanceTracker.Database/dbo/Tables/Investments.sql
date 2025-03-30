CREATE TABLE [dbo].[Investments] (
    [InvestmentID]  INT             IDENTITY (1, 1) NOT NULL,
    [UserID]        INT             NOT NULL,
    [AccountID]     INT             NULL,
    [AssetName]     NVARCHAR (100)  NOT NULL,
    [AssetClass]    NVARCHAR (50)   NOT NULL,
    [Ticker]        NVARCHAR (20)   NULL,
    [Quantity]      DECIMAL (18, 6) NOT NULL,
    [PurchasePrice] DECIMAL (18, 2) NOT NULL,
    [CurrentPrice]  DECIMAL (18, 2) NOT NULL,
    [PurchaseDate]  DATE            NOT NULL,
    [Notes]         NVARCHAR (255)  NULL,
    [SalesforceID]  NVARCHAR (20)   NULL,
    [LastUpdated]   DATETIME        DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([InvestmentID] ASC),
    FOREIGN KEY ([AccountID]) REFERENCES [dbo].[Accounts] ([AccountID]),
    FOREIGN KEY ([UserID]) REFERENCES [dbo].[Users] ([UserID])
);

