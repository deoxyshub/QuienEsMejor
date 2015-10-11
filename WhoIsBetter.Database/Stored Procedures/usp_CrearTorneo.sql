CREATE PROCEDURE [dbo].[usp_CrearTorneo]
	@pNombre NVARCHAR(100),
	@pNumeroParticipantes INT,
	@pNumeroContendores INT,
	@pFechaInicio DATE,
	@pFechaFin DATE,
	@pEnlace NVARCHAR(300),
	@pIDEstado INT,
	@pIDUsuario INT
AS
BEGIN
	INSERT INTO [dbo].[Torneo] (
		[Nombre],
		[NumeroParticipantes],
		[NumeroContendores],
		[FechaInicio],
		[FechaFin],
		[Enlace],
		[IDUsuario],
		[IDEstado]
	)
	VALUES (
		@pNombre,
		@pNumeroParticipantes,
		@pNumeroContendores,
		@pFechaInicio,
		@pFechaFin,
		@pEnlace,
		@pIDUsuario,
		@pIDEstado
	)

	SELECT CONVERT(INT, SCOPE_IDENTITY())
END