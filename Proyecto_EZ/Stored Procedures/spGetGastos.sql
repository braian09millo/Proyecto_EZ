IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('spGetGastos') and sysstat & 0xf = 4)
	DROP PROCEDURE spGetGastos
GO

CREATE PROCEDURE spGetGastos		
(
	@FechaDesde datetime, 
	@FechaHasta datetime
)												
WITH ENCRYPTION AS

	DECLARE @@nRet INT

	SELECT  *
	FROM gasto
	WHERE gas_fecha between @FechaDesde and @FechaHasta
	
	SET @@nRet = @@error
	IF @@nRet <> 0 
		RETURN @@nRet

RETURN 0
GO