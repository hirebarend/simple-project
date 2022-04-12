CREATE TABLE [dbo].[Orders]
(
    [Id] BIGINT IDENTITY (1, 1) NOT NULL,
    [Created] DATETIMEOFFSET(7) NOT NULL,
    [Reference] NVARCHAR(36) NOT NULL,
    [State] INT NOT NULL,
    [Updated] DATETIMEOFFSET(7) NOT NULL,
    [Version] INT NOT NULL,
    CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED ([Id] ASC)
);
