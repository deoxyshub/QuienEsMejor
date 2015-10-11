CREATE PROCEDURE [dbo].[usp_ActualizarTorneo]
	@pID INT,
	@pNombre NVARCHAR(100),
	@pNumeroParticipantes INT,
	@pNumeroContendores INT,
	@pFechaInicio DATE,
	@pFechaFin DATE,
	@pEnlace NVARCHAR(300),
	@pIDEstado INT
AS
BEGIN
	UPDATE [dbo].[Torneo]
	SET	[Nombre] = @pNombre,
		[NumeroParticipantes] = @pNumeroParticipantes,
		[NumeroContendores] = @pNumeroContendores,
		[FechaInicio] = @pFechaInicio,
		[FechaFin] = @pFechaFin,
		[Enlace] = @pEnlace,
		[IDEstado] = @pIDEstado
	WHERE [ID] = @pID
END