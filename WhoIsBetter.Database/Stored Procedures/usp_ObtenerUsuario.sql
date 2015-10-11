CREATE PROCEDURE [dbo].[usp_ObtenerUsuario]
	@pID INT = NULL,
	@pCorreo NVARCHAR(50) = NULL
AS
BEGIN
	SET @pID = COALESCE(@pID, 0)
	SET @pCorreo = COALESCE(@pCorreo, '')

	SELECT * FROM [dbo].[Usuario] 
	WHERE	(@pID = 0 OR (@pID != 0 AND [ID] = @pID))
		AND	(@pCorreo = '' OR (@pCorreo != '' AND [Correo] = @pCorreo))
END