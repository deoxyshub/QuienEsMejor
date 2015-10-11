CREATE PROCEDURE [dbo].[usp_EliminarUsuario]
	@pID INT
AS
BEGIN
	DELETE FROM [dbo].[Usuario] 
	WHERE [ID] = @pID
END