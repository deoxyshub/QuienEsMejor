CREATE PROCEDURE [dbo].[usp_ListarContendores]
	@pIDTorneo INT
AS
BEGIN
	SELECT * FROM [dbo].[Contendor]
    WHERE [IDTorneo] = @pIDTorneo
END