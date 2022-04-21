CREATE TABLE [dbo].[DynamicRoutes]
(
    [Id] BIGINT IDENTITY (1, 1) NOT NULL,
    [Created] DATETIMEOFFSET(7) NOT NULL,
    [Method] VARCHAR(4) NOT NULL,
    [Payload] VARCHAR(MAX) NULL,
    [Reference] VARCHAR(36) NOT NULL,
    [Success] BIT NOT NULL,
    [Type] VARCHAR(100) NOT NULL,
    [Url] VARCHAR(256) NOT NULL
    CONSTRAINT [PK_Logs] PRIMARY KEY CLUSTERED ([Id] ASC)
);
