CREATE TABLE [dbo].[Orders]
(
    [Id] BIGINT IDENTITY (1, 1) NOT NULL,
    [AccountReference] VARCHAR(36) NOT NULL,
    [Created] DATETIMEOFFSET(7) NOT NULL,
    [Metadata] NVARCHAR(MAX) NOT NULL,
    [ProductId] VARCHAR(36) NOT NULL,
    [Reference] VARCHAR(36) NOT NULL,
    [State] TINYINT NOT NULL,
    [Updated] DATETIMEOFFSET(7) NOT NULL,
    [Version] SMALLINT NOT NULL,
    CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED ([Id] ASC)
);
