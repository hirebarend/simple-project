CREATE PROCEDURE [dbo].[InsertDynamicRoute]
@data VARCHAR(MAX),
@reference VARCHAR(36),
@type VARCHAR(100)
AS BEGIN
	INSERT INTO [dbo].[DynamicRoutes] ([Created], [Data], [Reference], [Type])
	OUTPUT inserted.*
	VALUES (SYSDATETIMEOFFSET(), @data, @reference, @type);
END;
