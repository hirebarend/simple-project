CREATE PROCEDURE [dbo].[InsertOrder]
@reference VARCHAR(36),
@state TINYINT
AS BEGIN
	BEGIN TRY;
		BEGIN TRAN;
			INSERT INTO [dbo].[Orders] ([Created], [Reference], [State], [Updated], [Version])
			OUTPUT inserted.*
			VALUES (SYSDATETIMEOFFSET(), @reference, @state, SYSDATETIMEOFFSET(), 0);
		COMMIT TRAN;
	END TRY
	BEGIN CATCH;
		SELECT TOP (1) * FROM [dbo].[Orders]
		WHERE [Reference] = @reference;
	END CATCH;
END;