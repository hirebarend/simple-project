CREATE PROCEDURE [dbo].[InsertOrder]
@reference NVARCHAR(36),
@state INT
AS BEGIN
	BEGIN TRAN;
		INSERT INTO [dbo].[Orders] ([Created], [Reference], [State], [Updated], [Version])
		OUTPUT inserted.*
		VALUES (SYSDATETIMEOFFSET(), @reference, @state, SYSDATETIMEOFFSET(), 0);
	COMMIT TRAN;
END;
