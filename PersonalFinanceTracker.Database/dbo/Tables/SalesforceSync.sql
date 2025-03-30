CREATE TABLE [dbo].[SalesforceSync] (
    [SyncID]       INT            IDENTITY (1, 1) NOT NULL,
    [ObjectType]   NVARCHAR (50)  NOT NULL,
    [LocalID]      INT            NOT NULL,
    [SalesforceID] NVARCHAR (20)  NOT NULL,
    [LastSyncDate] DATETIME       NOT NULL,
    [SyncStatus]   NVARCHAR (20)  NOT NULL,
    [ErrorMessage] NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([SyncID] ASC)
);

