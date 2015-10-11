CREATE PROCEDURE [dbo].[usp_ReporteTorneo]
	@pIDTorneo INT
AS
BEGIN
	SELECT *
	FROM
	(
		SELECT f.[IDTorneo],
			   NombreTorneo = t.[Nombre],
			   NumeroRealParticipantes = (SELECT COUNT(DISTINCT x.[IDParticipante]) FROM [dbo].[Favorito] x WHERE x.[IDTorneo] = t.[ID]),
			   t.[NumeroContendores],
			   t.[FechaInicio],
			   t.[FechaFin],
			   IDParticipante = p.[ID],
			   CorreoParticipante = p.[Correo],
			   NombreParticipante = p.[Nombre],
			   IDContendor1 = c1.[ID],
			   NombreContendor1 = c1.[Nombre],
			   TextoContendor1 = c1.[Texto],
			   RutaImagenContendor1 = c1.[RutaImagen],
			   IDContendor2 = c2.[ID],
			   NombreContendor2 = c2.[Nombre],
			   TextoContendor2 = c2.[Texto],
			   RutaImagenContendor2 = c2.[RutaImagen],
			   f.[IDGanador],
			   [AgrupadorInicial] = ROW_NUMBER() OVER(PARTITION BY f.[IDParticipante] ORDER BY Etapa DESC),
			   [PosFixture] = DENSE_RANK() OVER(ORDER BY Etapa DESC)
		FROM [dbo].[Favorito] f
		INNER JOIN [dbo].[Torneo] t ON t.[ID] = f.[IDTorneo]
		INNER JOIN [dbo].[Participante] p ON p.[ID] = f.[IDParticipante]
		INNER JOIN [dbo].[Contendor] c1 ON c1.[ID] = f.[IDContendor1] AND c1.[IDTorneo] = t.[ID]
		INNER JOIN [dbo].[Contendor] c2 ON c2.[ID] = f.[IDContendor2] AND c2.[IDTorneo] = t.[ID]
		WHERE   f.[IDTorneo] = @pIDTorneo
	) T
    ORDER BY [PosFixture] ASC, [AgrupadorInicial] ASC, [IDParticipante] ASC
END