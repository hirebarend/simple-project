CREATE PROCEDURE [dbo].[InsertDynamicRoute]
@method VARCHAR(4),
@payload VARCHAR(MAX),
@reference VARCHAR(36),
@success BIT,
@type VARCHAR(100),
@url VARCHAR(256)
AS BEGIN
	INSERT INTO [dbo].[DynamicRoutes] ([Created], [Method], [Payload], [Reference], [Success], [Type], [Url])
	OUTPUT inserted.*
	VALUES (SYSDATETIMEOFFSET(), @method, @payload, @reference, @success, @type, @url);
END;
