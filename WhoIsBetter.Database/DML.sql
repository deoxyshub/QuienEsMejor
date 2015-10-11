USE [WhoIsBetter]
GO

INSERT INTO [dbo].[Rol] ([ID], [Nombre]) VALUES (1,'Admin');
INSERT INTO [dbo].[Rol] ([ID], [Nombre]) VALUES (2,'Usuario');
GO

INSERT INTO [dbo].[Estado] ([ID], [Descripcion]) VALUES (1,'Inicial');
INSERT INTO [dbo].[Estado] ([ID], [Descripcion]) VALUES (2,'En curso');
INSERT INTO [dbo].[Estado] ([ID], [Descripcion]) VALUES (3,'Finalizado');
INSERT INTO [dbo].[Estado] ([ID], [Descripcion]) VALUES (4,'Cancelado');
GO