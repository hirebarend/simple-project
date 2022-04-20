CREATE TABLE [dbo].[DynamicRoutes]
(
    [Id] BIGINT IDENTITY (1, 1) NOT NULL,
    [Created] DATETIMEOFFSET(7) NOT NULL,
    [Data] VARCHAR(MAX) NULL,
    [Reference] VARCHAR(36) NOT NULL,
    [Type] VARCHAR(100) NOT NULL
    CONSTRAINT [PK_Logs] PRIMARY KEY CLUSTERED ([Id] ASC)
);
