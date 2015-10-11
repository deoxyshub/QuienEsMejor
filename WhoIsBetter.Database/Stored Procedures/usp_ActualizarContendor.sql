CREATE PROCEDURE [dbo].[usp_ActualizarContendor]
	@pID INT,
	@pIDTorneo INT,
	@pNombre NVARCHAR(100),
	@pTexto NVARCHAR(100),
	@pRutaImagen NVARCHAR(256)
AS
BEGIN
	update [dbo].[Contendor]
	set	[Nombre] = @pNombre,
		[Texto] = @pTexto,
		[RutaImagen] = @pRutaImagen,
		[IDTorneo] = @pIDTorneo
    where [ID] = @pID
END