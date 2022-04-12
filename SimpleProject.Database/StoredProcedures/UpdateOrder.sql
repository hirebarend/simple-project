CREATE PROCEDURE [dbo].[UpdateOrder]
@reference NVARCHAR(36),
@state INT,
@version INT
AS BEGIN
	BEGIN TRAN;
		DECLARE @orders TABLE (
			[Id] BIGINT NOT NULL,
			[Created] DATETIMEOFFSET(7) NOT NULL,
			[Reference] NVARCHAR(36) NOT NULL,
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
					RAISERROR ('optimistic_lock_exception', 16, 16);

					RETURN;
				END;

		SELECT * FROM @orders;
	COMMIT TRAN;
END;
