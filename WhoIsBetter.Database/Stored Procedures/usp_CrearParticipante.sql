CREATE PROCEDURE [dbo].[usp_CrearParticipante]
	@pNombre NVARCHAR(100),
	@pCorreo NVARCHAR(50)
AS
BEGIN
	INSERT INTO [dbo].[Participante] (
		[Nombre], 
		[Correo]
	)
    VALUES (
		@pNombre, 
		@pCorreo
	)

	SELECT CONVERT(INT, SCOPE_IDENTITY())
END