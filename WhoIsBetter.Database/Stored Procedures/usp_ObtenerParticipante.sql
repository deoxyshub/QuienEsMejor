CREATE PROCEDURE [dbo].[usp_ObtenerParticipante]
	@pIDTorneo INT,
	@pCorreo NVARCHAR(50)
AS
BEGIN
	SELECT p.ID, p.Nombre, p.Correo
	FROM [dbo].[Participante] p
	INNER JOIN [dbo].[Favorito] f ON f.[IDTorneo] = @pIDTorneo
		AND p.[ID] = f.[IDParticipante]
	WHERE p.[Correo] = @pCorreo
END