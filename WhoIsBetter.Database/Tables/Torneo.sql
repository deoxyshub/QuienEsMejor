CREATE TABLE [dbo].[Torneo]
(
	[ID] INT IDENTITY(1,1) NOT NULL,
	[Nombre] NVARCHAR(100) NOT NULL,
	[NumeroParticipantes] INT NOT NULL,
	[NumeroContendores] INT NOT NULL,
	[FechaInicio] DATE NOT NULL,
	[FechaFin] DATE NOT NULL,
	[Enlace] NVARCHAR(300) NOT NULL,
	[IDUsuario] INT NOT NULL, 
	[IDEstado] INT NOT NULL,
	
    CONSTRAINT [PK_Torneo] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)
)
GO

ALTER TABLE [dbo].[Torneo] ADD CONSTRAINT [FK_Torneo_Estado] FOREIGN KEY([IDEstado])
REFERENCES [dbo].[Estado] ([ID])
GO

ALTER TABLE [dbo].[Torneo] CHECK CONSTRAINT [FK_Torneo_Estado]
GO

ALTER TABLE [dbo].[Torneo] ADD CONSTRAINT [FK_Torneo_Usuario] FOREIGN KEY([IDUsuario])
REFERENCES [dbo].[Usuario] ([ID])
GO

ALTER TABLE [dbo].[Torneo] CHECK CONSTRAINT [FK_Torneo_Usuario]
GO