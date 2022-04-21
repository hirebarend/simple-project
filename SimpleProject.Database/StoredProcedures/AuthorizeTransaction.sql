CREATE PROCEDURE [dbo].[AuthorizeTransaction]
@amount INT,
@reference VARCHAR(36),
@version SMALLINT
AS BEGIN
	BEGIN TRAN;
		DECLARE @transactions TABLE (
			[Id] BIGINT NOT NULL,
			[AccountReference] VARCHAR(36) NOT NULL,
			[Amount] INT NOT NULL,
			[Created] DATETIMEOFFSET(7) NOT NULL,
			[Metadata] NVARCHAR(MAX) NOT NULL,
			[ProductId] VARCHAR(36) NOT NULL,
			[Reference] VARCHAR(36) NOT NULL,
			[State] TINYINT NOT NULL,
			[Updated] DATETIMEOFFSET(7) NOT NULL,
			[Version] SMALLINT NOT NULL);

		UPDATE [dbo].[Transactions] 
		SET [State] = 2, [Updated] = SYSDATETIMEOFFSET(), [Version] = [Version] + 1
		OUTPUT inserted.* INTO @transactions
		WHERE [Reference] = @reference AND [Version] = @version;

		IF (SELECT COUNT(*) FROM @transactions) = 0
				BEGIN;
					ROLLBACK TRAN;
					
					SELECT TOP (1) * FROM [dbo].[Transactions]
					WHERE [Reference] = @reference;

					RETURN;
				END;

		DECLARE @balance INT;

		UPDATE [dbo].[Accounts] SET @balance = [Balance] = [Balance] + @amount
		WHERE [Reference] = '9128-7011-4037-8196';

		IF (@balance IS NULL OR @balance < 0)
			BEGIN;
				ROLLBACK TRAN;

				RAISERROR('insufficient_balance', 16, 16);

				RETURN;
			END;

		SELECT * FROM @transactions;
	COMMIT TRAN;
END;
