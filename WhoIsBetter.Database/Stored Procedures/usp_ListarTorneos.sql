CREATE PROCEDURE [dbo].[usp_ListarTorneos]
	@pIDUsuario INT = NULL
AS
BEGIN
	SET @pIDUsuario = COALESCE(@pIDUsuario, 0)

	SELECT distinct t.* FROM [dbo].[Torneo] t
	WHERE (@pIDUsuario = 0 OR (@pIDUsuario != 0 AND t.[IDUsuario] = @pIDUsuario))
END