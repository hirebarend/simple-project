CREATE PROCEDURE [dbo].[InsertLog]
@reference VARCHAR(36),
@message VARCHAR(100)
AS BEGIN
	BEGIN TRAN;
		INSERT INTO [dbo].[Logs] ([Created], [Message], [Reference])
		OUTPUT inserted.*
		VALUES (SYSDATETIMEOFFSET(), @message, @reference);
	COMMIT TRAN;
END;
