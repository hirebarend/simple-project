﻿CREATE PROCEDURE [dbo].[UpdateOrder]
@reference VARCHAR(36),
@state TINYINT,
@version SMALLINT
AS BEGIN
	BEGIN TRAN;
		DECLARE @orders TABLE (
			[Id] BIGINT NOT NULL,
			[Created] DATETIMEOFFSET(7) NOT NULL,
			[Reference] VARCHAR(36) NOT NULL,
			[State] INT NOT NULL,
			[Updated] DATETIMEOFFSET(7) NOT NULL,
			[Version] INT NOT NULL);

		UPDATE [dbo].[Orders] 
		SET [State] = @state, [Updated] = SYSDATETIMEOFFSET(), [Version] = [Version] + 1
		OUTPUT inserted.* INTO @orders
		WHERE [Reference] = @reference AND [Version] = @version;

		IF (SELECT COUNT(*) FROM @orders) = 0
				BEGIN;
					ROLLBACK TRAN;
					
					SELECT TOP (1) * FROM [dbo].[Orders]
					WHERE [Reference] = @reference;

					RETURN;
				END;

		SELECT * FROM @orders;
	COMMIT TRAN;
END;
