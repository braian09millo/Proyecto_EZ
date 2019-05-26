IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('spFacturacionResultado') and sysstat & 0xf = 4)
	DROP PROCEDURE spFacturacionResultado
GO

CREATE PROCEDURE spFacturacionResultado		
(
	@FechaDesde datetime, 
	@FechaHasta datetime,
	@Repartidor varchar(10)
)												
WITH ENCRYPTION AS

	IF 1=0 BEGIN
	    SET FMTONLY OFF
	END

	DECLARE @@nRet INT

	SELECT  
		SUM(prd_precioC*det_cantidad) Costo,
		MAX(ped_monto) MontoRemito
	INTO #Aux
	FROM Pedido
	JOIN pedido_detalle ON det_pedido = ped_id
	JOIN producto ON prod_id = det_producto
	JOIN precio ON ped_fecha between pre_fecha and pre_fechaHasta
	JOIN precio_detalle ON pre_ident = prd_campre and prd_produ = prod_id 
	JOIN cliente ON cli_id = ped_cliente
	LEFT JOIN usuario ON ped_repartidor = usu_usuario
	WHERE ped_fecha BETWEEN @FechaDesde AND @FechaHasta
	AND (ped_repartidor = @Repartidor or @repartidor is null)
	GROUP BY ped_id, ped_fecha

	SELECT 
		gas_monto MontoGasto
	INTO #Aux2
	FROM gasto
	WHERE gas_fecha BETWEEN @FechaDesde AND @FechaHasta

	SELECT 
		'Ganancia Mensual' AS Descripcion,
		SUM(MontoRemito - Costo) AS Monto
	FROM #Aux
	UNION ALL
	SELECt 
		'Gasto Mensual' AS Descripcion,
		SUM(MontoGasto) AS Monto
	FROM #Aux2

	SET @@nRet = @@error
	IF @@nRet <> 0 
		RETURN @@nRet

RETURN 0
GO