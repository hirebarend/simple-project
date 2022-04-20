CREATE PROCEDURE [dbo].[FindOrder]
@reference VARCHAR(36)
AS BEGIN
	SELECT TOP (1) * FROM [dbo].[Orders]
	WHERE [Reference] = @reference;
END;
