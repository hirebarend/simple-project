CREATE PROCEDURE [dbo].[InsertTransaction]
@accountReference VARCHAR(36),
@amount INT,
@metadata VARCHAR(MAX),
@productId VARCHAR(36),
@reference VARCHAR(36),
@state TINYINT
AS BEGIN
	BEGIN TRY;
		BEGIN TRAN;
			INSERT INTO [dbo].[Transactions] ([AccountReference], [Amount], [Created], [Metadata], [ProductId], [Reference], [State], [Updated], [Version])
			OUTPUT inserted.*
			VALUES (@accountReference, @amount, SYSDATETIMEOFFSET(), @metadata, @productId, @reference, @state, SYSDATETIMEOFFSET(), 0);
		COMMIT TRAN;
	END TRY
	BEGIN CATCH;
		SELECT TOP (1) * FROM [dbo].[Transactions]
		WHERE [Reference] = @reference;
	END CATCH;
END;