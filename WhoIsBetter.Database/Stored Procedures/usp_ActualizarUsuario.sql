CREATE PROCEDURE [dbo].[usp_ActualizarUsuario]
	@pID INT,
	@pNombre NVARCHAR(100),
	@pApellidoPaterno NVARCHAR(50),
	@pApellidoMaterno NVARCHAR(50),
	@pClave NVARCHAR(6),
	@pSexo BIT NULL,
	@pNumeroCelular INT NULL,
	@pNumeroTelefono INT NULL,
	@pFechaNacimiento DATE NULL
AS
BEGIN
	UPDATE [dbo].[Usuario] 
	SET [Nombre] = @pNombre, 
		[ApellidoPaterno] = @pApellidoPaterno,
		[ApellidoMaterno] = @pApellidoMaterno,
		[Clave] = @pClave, 
		[Sexo] = @pSexo, 
		[NumeroCelular] = @pNumeroCelular, 
		[NumeroTelefono] = @pNumeroTelefono, 
		[FechaNacimiento] = @pFechaNacimiento
	WHERE [ID] = @pID
END