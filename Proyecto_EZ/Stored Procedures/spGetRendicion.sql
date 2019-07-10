IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('spGetRendicion') and sysstat & 0xf = 4)
	DROP PROCEDURE spGetRendicion
GO

CREATE PROCEDURE spGetRendicion		
(
	@FechaDesde datetime, 
	@FechaHasta datetime
)												
WITH ENCRYPTION AS

	DECLARE @@nRet INT

	SELECT  ren_id IdRendicion, ren_desde Desde, ren_hasta Hasta, ren_total Costo
	FROM rendicion
	WHERE ren_desde between @FechaDesde and @FechaHasta
	--and ren_hasta <= @FechaHasta
	
	SET @@nRet = @@error
	IF @@nRet <> 0 
		RETURN @@nRet

RETURN 0
GO