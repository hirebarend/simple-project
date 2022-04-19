CREATE PROCEDURE [dbo].[InsertDynamicRoute]
@data VARCHAR(MAX),
@reference VARCHAR(36),
@message VARCHAR(100)
AS BEGIN
	BEGIN TRAN;
		INSERT INTO [dbo].[DynamicRoutes] ([Created], [Message], [Reference])
		OUTPUT inserted.*
		VALUES (SYSDATETIMEOFFSET(), @message, @reference);
	COMMIT TRAN;
END;
