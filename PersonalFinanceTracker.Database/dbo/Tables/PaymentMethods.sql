CREATE TABLE [dbo].[PaymentMethods] (
    [PaymentMethodID] INT             IDENTITY (1, 1) NOT NULL,
    [UserID]          INT             NOT NULL,
    [MethodName]      NVARCHAR (100)  NOT NULL,
    [MethodType]      NVARCHAR (50)   NOT NULL,
    [CurrentBalance]  DECIMAL (18, 2) NULL,
    [IsActive]        BIT             DEFAULT ((1)) NOT NULL,
    [SalesforceID]    NVARCHAR (20)   NULL,
    [LastSyncDate]    DATETIME        NULL,
    PRIMARY KEY CLUSTERED ([PaymentMethodID] ASC),
    FOREIGN KEY ([UserID]) REFERENCES [dbo].[Users] ([UserID])
);

