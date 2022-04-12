CREATE PROCEDURE [dbo].[UpdateTransaction]
@reference VARCHAR(36),
@state TINYINT,
@version SMALLINT
AS BEGIN
	BEGIN TRAN;
		DECLARE @transactions TABLE (
			[Id] BIGINT NOT NULL,
			[Amount] INT NOT NULL,
			[Created] DATETIMEOFFSET(7) NOT NULL,
			[Reference] VARCHAR(36) NOT NULL,
			[State] INT NOT NULL,
			[Updated] DATETIMEOFFSET(7) NOT NULL,
			[Version] INT NOT NULL);

		UPDATE [dbo].[Transactions] 
		SET [State] = @state, [Updated] = SYSDATETIMEOFFSET(), [Version] = [Version] + 1
		OUTPUT inserted.* INTO @transactions
		WHERE [Reference] = @reference AND [Version] = @version;

		IF (SELECT COUNT(*) FROM @transactions) = 0
				BEGIN;
					ROLLBACK TRAN;
					
					SELECT TOP (1) * FROM [dbo].[Orders]
					WHERE [Reference] = @reference;

					RETURN;
				END;

		SELECT * FROM @transactions;
	COMMIT TRAN;
END;
