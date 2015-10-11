CREATE PROCEDURE [dbo].[usp_CrearUsuario]
	@pCorreo NVARCHAR(50),
	@pNombre NVARCHAR(100),
	@pApellidoPaterno NVARCHAR(50),
	@pApellidoMaterno NVARCHAR(50),
	@pClave NVARCHAR(6),
	@pIDRol INT
AS
BEGIN
	INSERT INTO [dbo].[Usuario] (
		[Correo], 
		[Nombre], 
		[ApellidoPaterno], 
		[ApellidoMaterno], 
		[Clave], 
		[IDRol]
	)
	VALUES (
		@pCorreo, 
		@pNombre, 
		@pApellidoPaterno, 
		@pApellidoMaterno, 
		@pClave, 
		@pIDRol
	)

	SELECT CONVERT(INT, SCOPE_IDENTITY())
END