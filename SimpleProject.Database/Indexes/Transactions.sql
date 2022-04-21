CREATE NONCLUSTERED INDEX [IX_Transactions_Reference] ON [dbo].[Transactions] ([Reference] ASC);
GO;

CREATE NONCLUSTERED INDEX [IX_Transactions_AccountReference] ON [dbo].[Transactions] ([AccountReference] ASC, [Created] DESC);
GO;