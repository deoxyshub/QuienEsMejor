CREATE PROCEDURE [dbo].[usp_EliminarTorneo]
	@pID INT
AS
BEGIN
	DELETE FROM [dbo].[Torneo]
	WHERE [ID] = @pID
END