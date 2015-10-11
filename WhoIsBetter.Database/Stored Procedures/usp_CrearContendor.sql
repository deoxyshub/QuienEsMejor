CREATE PROCEDURE [dbo].[usp_CrearContendor]
	@pIDTorneo INT,
	@pNombre NVARCHAR(100),
	@pTexto NVARCHAR(100),
	@pRutaImagen NVARCHAR(256)
AS
BEGIN
	INSERT INTO [dbo].[Contendor] (
		[IDTorneo], 
		[Nombre], 
		[Texto], 
		[RutaImagen]
	)
    VALUES (
		@pIDTorneo, 
		@pNombre, 
		@pTexto, 
		@pRutaImagen
	)

	SELECT CONVERT(INT, SCOPE_IDENTITY())
END