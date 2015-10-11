CREATE PROCEDURE [dbo].[usp_EliminarContendor]
	@pID INT
AS
BEGIN
	DELETE FROM [dbo].[Contendor]
    WHERE [ID] = @pID
END