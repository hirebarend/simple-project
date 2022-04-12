CREATE PROCEDURE [dbo].[InsertLog]
@reference NVARCHAR(36),
@message NVARCHAR(100)
AS BEGIN
	BEGIN TRAN;
		INSERT INTO [dbo].[Logs] ([Created], [Message], [Reference])
		OUTPUT inserted.*
		VALUES (SYSDATETIMEOFFSET(), @message, @reference);
	COMMIT TRAN;
END;
