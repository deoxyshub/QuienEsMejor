CREATE PROCEDURE [dbo].[usp_ObtenerContendor]
	@pID INT
AS
BEGIN
	SELECT * FROM [dbo].[Contendor]
    WHERE [ID] = @pID
END