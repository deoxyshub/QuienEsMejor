CREATE TABLE [dbo].[Favorito]
(
	[ID] INT IDENTITY(1,1) NOT NULL,
	[IDTorneo] INT NOT NULL,
	[IDParticipante] INT NOT NULL,
	[IDContendor1] INT NOT NULL,
	[IDContendor2] INT NOT NULL,
	[IDGanador] INT NOT NULL,
	[Etapa] SMALLINT NOT NULL,

	CONSTRAINT [PK_Favorito] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC,
		[IDTorneo] ASC,
		[IDParticipante] ASC
	)
)
GO

ALTER TABLE [dbo].[Favorito] ADD CONSTRAINT [FK_Favorito_Torneo] FOREIGN KEY([IDTorneo])
REFERENCES [dbo].[Torneo] ([ID])
GO

ALTER TABLE [dbo].[Favorito] CHECK CONSTRAINT [FK_Favorito_Torneo]
GO

ALTER TABLE [dbo].[Favorito] ADD CONSTRAINT [FK_Favorito_Participante] FOREIGN KEY([IDParticipante])
REFERENCES [dbo].[Participante] ([ID])
GO

ALTER TABLE [dbo].[Favorito] CHECK CONSTRAINT [FK_Favorito_Participante]
GO