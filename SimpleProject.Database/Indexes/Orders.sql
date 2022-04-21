CREATE NONCLUSTERED INDEX [IX_Orders_Reference] ON [dbo].[Orders] ([Reference] ASC);
GO;

CREATE NONCLUSTERED INDEX [IX_Orders_AccountReference] ON [dbo].[Orders] ([AccountReference] ASC, [Created] DESC);
GO;