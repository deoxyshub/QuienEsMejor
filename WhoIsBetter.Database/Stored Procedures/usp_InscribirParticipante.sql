CREATE PROCEDURE [dbo].[usp_InscribirParticipante]
	@pIDTorneo INT,
	@pCorreo NVARCHAR(50),
	@pNombre NVARCHAR(100)
AS
BEGIN

	DECLARE @id INT = null

	SELECT @id = [ID] FROM [dbo].[Participante] WHERE [Correo] = @pCorreo

	IF (@@ROWCOUNT = 0)
	BEGIN
		INSERT INTO [dbo].[Participante] (
			[Correo], 
			[Nombre]
		)
		VALUES (
			@pCorreo, 
			@pNombre
		)

		SET @id = CONVERT(INT, SCOPE_IDENTITY())

		UPDATE [dbo].[Torneo]
		SET [NumeroParticipantes] = [NumeroParticipantes] + 1
		WHERE [ID] = @pIDTorneo
	END

	SELECT @id
END