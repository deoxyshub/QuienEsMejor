CREATE TABLE [dbo].[Contendor]
(
	[ID] INT IDENTITY(1,1) NOT NULL,
	[Nombre] NVARCHAR(100) NOT NULL,
	[Texto] NVARCHAR(200) NOT NULL,
	[RutaImagen] NVARCHAR(256) NOT NULL,
	[IDTorneo] INT NOT NULL,

	CONSTRAINT [PK_Contendor] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)
)
GO

ALTER TABLE [dbo].[Contendor] ADD CONSTRAINT [FK_Contendor_Torneo] FOREIGN KEY([IDTorneo])
REFERENCES [dbo].[Torneo] ([ID])
GO

ALTER TABLE [dbo].[Contendor] CHECK CONSTRAINT [FK_Contendor_Torneo]
GO