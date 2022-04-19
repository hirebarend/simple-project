ALTER TABLE [dbo].[DynamicRoutes] ADD CONSTRAINT UC_DynamicRoutes_Reference UNIQUE ([Message], [Reference]);
