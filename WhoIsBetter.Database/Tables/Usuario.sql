CREATE TABLE [dbo].[Usuario]
(
	[ID] INT IDENTITY(1,1) NOT NULL,
	[Correo] NVARCHAR(50) NOT NULL,
	[Nombre] NVARCHAR(100) NOT NULL,
	[ApellidoPaterno] NVARCHAR(50) NOT NULL,
	[ApellidoMaterno] NVARCHAR(50) NOT NULL,
	[Clave] NVARCHAR(6) NOT NULL,
	[Sexo] BIT NULL,
	[NumeroCelular] INT NULL,
	[NumeroTelefono] INT NULL,
	[FechaNacimiento] DATE NULL,
	[IDRol] INT NOT NULL,

	CONSTRAINT [PK_Usuario] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)
)
GO

ALTER TABLE [dbo].[Usuario] ADD CONSTRAINT [FK_Usuario_Rol] FOREIGN KEY([IDRol])
REFERENCES [dbo].[Rol] ([ID])
GO

ALTER TABLE [dbo].[Usuario] CHECK CONSTRAINT [FK_Usuario_Rol]
GO