CREATE PROCEDURE [dbo].[InsertOrder]
@accountReference VARCHAR(36),
@metadata VARCHAR(MAX),
@productId VARCHAR(36),
@reference VARCHAR(36),
@state TINYINT
AS BEGIN
	BEGIN TRY;
		BEGIN TRAN;
			INSERT INTO [dbo].[Orders] ([AccountReference], [Created], [Metadata], [ProductId], [Reference], [State], [Updated], [Version])
			OUTPUT inserted.*
			VALUES (@accountReference, SYSDATETIMEOFFSET(), @metadata, @productId, @reference, @state, SYSDATETIMEOFFSET(), 0);
		COMMIT TRAN;
	END TRY
	BEGIN CATCH;
		SELECT TOP (1) * FROM [dbo].[Orders]
		WHERE [Reference] = @reference;
	END CATCH;
END;