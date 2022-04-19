CREATE TABLE [dbo].[Accounts]
(
    [Id] BIGINT IDENTITY (1, 1) NOT NULL,
    [Balance] INT NOT NULL,
    [Created] DATETIMEOFFSET(7) NOT NULL,
    [Reference] VARCHAR(36) NOT NULL,
    [Updated] DATETIMEOFFSET(7) NOT NULL,
    [Version] SMALLINT NOT NULL,
    CONSTRAINT [PK_Accounts] PRIMARY KEY CLUSTERED ([Id] ASC)
);
