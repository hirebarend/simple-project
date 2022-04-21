CREATE PROCEDURE [dbo].[FindTransaction]
@reference VARCHAR(36)
AS BEGIN
	SELECT TOP (1) * FROM [dbo].[Transactions]
	WHERE [Reference] = @reference;
END;
