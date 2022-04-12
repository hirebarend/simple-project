CREATE TABLE [dbo].[Transactions]
(
    [Id] BIGINT IDENTITY (1, 1) NOT NULL,
    [Amount] INT NOT NULL,
    [Created] DATETIMEOFFSET(7) NOT NULL,
    [Reference] VARCHAR(36) NOT NULL,
    [State] TINYINT NOT NULL,
    [Updated] DATETIMEOFFSET(7) NOT NULL,
    [Version] SMALLINT NOT NULL,
    CONSTRAINT [PK_Transactions] PRIMARY KEY CLUSTERED ([Id] ASC)
);
