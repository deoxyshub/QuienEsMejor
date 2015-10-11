CREATE PROCEDURE [dbo].[usp_CrearFavorito]
	@pIDTorneo INT,
	@pIDParticipante INT,
	@pIDContendor1 INT,
	@pIDContendor2 INT,
	@pIDGanador INT,
	@pEtapa INT
AS
BEGIN
	INSERT INTO [dbo].[Favorito] (
		[IDTorneo],
		[IDParticipante],
		[IDContendor1],
		[IDContendor2],
		[IDGanador],
		[Etapa]
	)
	VALUES (
		@pIDTorneo,
		@pIDParticipante,
		@pIDContendor1,
		@pIDContendor2,
		@pIDGanador,
		@pEtapa
	)

	SELECT CONVERT(INT, SCOPE_IDENTITY())
END