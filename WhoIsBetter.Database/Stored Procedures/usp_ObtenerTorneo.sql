CREATE PROCEDURE [dbo].[usp_ObtenerTorneo]
	@pID INT = NULL,
	@pEnlace NVARCHAR(300) = NULL
AS
BEGIN
	SET @pID = COALESCE(@pID, 0)
	SET @pEnlace = COALESCE(@pEnlace, '')

	SELECT	t.*, 
			NumeroRealParticipantes = (SELECT COUNT(DISTINCT f.[IDParticipante]) FROM [dbo].[Favorito] f WHERE f.[IDTorneo] = t.[ID])
	FROM [dbo].[Torneo] t
	WHERE	(@pID = 0 OR (@pID != 0 AND t.[ID] = @pID))
		AND	(@pEnlace = '' OR (@pEnlace != '' AND t.[Enlace] = @pEnlace))
END