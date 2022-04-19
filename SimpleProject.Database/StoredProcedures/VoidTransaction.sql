﻿CREATE PROCEDURE [dbo].[VoidTransaction]
@amount INT,
@reference VARCHAR(36),
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
		SET [State] = 4, [Updated] = SYSDATETIMEOFFSET(), [Version] = [Version] + 1
		OUTPUT inserted.* INTO @transactions
		WHERE [Reference] = @reference AND [Version] = @version;

		IF (SELECT COUNT(*) FROM @transactions) = 0
				BEGIN;
					ROLLBACK TRAN;
					
					SELECT TOP (1) * FROM [dbo].[Transactions]
					WHERE [Reference] = @reference;

					RETURN;
				END;

		UPDATE [dbo].[Accounts] SET [Balance] = [Balance] + (@amount * -1)
		WHERE [Reference] = '9128-7011-4037-8196';

		SELECT * FROM @transactions;
	COMMIT TRAN;
END;
