CREATE PROCEDURE [dbo].[InsertTransaction]
@amount INT,
@reference VARCHAR(36),
@state TINYINT
AS BEGIN
	BEGIN TRY;
		BEGIN TRAN;
			INSERT INTO [dbo].[Transactions] ([Amount], [Created], [Reference], [State], [Updated], [Version])
			OUTPUT inserted.*
			VALUES (@amount, SYSDATETIMEOFFSET(), @reference, @state, SYSDATETIMEOFFSET(), 0);
		COMMIT TRAN;
	END TRY
	BEGIN CATCH;
		SELECT TOP (1) * FROM [dbo].[Transactions]
		WHERE [Reference] = @reference;
	END CATCH;
END;