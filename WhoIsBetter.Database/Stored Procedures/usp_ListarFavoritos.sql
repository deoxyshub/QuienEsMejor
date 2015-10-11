CREATE PROCEDURE [dbo].[usp_ListarFavoritos]
	@pIDTorneo INT,
	@pIDParticipante INT
AS
BEGIN
	SELECT * FROM [dbo].[Favorito]
    WHERE [IDTorneo] = @pIDTorneo AND [IDParticipante] = @pIDParticipante
	ORDER BY [ID]
END