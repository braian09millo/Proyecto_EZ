IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('spDashboardGananciaNetaGrafico') and sysstat & 0xf = 4)
	DROP PROCEDURE spDashboardGananciaNetaGrafico
GO

CREATE PROCEDURE spDashboardGananciaNetaGrafico
WITH ENCRYPTION AS

	DECLARE @@nRet INT

SELECT  
		ped_fecha as Dia,
		ped_id IDRemito,
		max(cli_nombre) + ' - ' + max(cli_direccion)  as Nombre,
		sum(det_cantidad) Packs,
		sum(prd_precioC*det_cantidad) costo,
		max(ped_monto) MontoRemito,
		max(ped_monto)-isnull(max(ped_factu),0) Debe,
		max(ped_factu) Facturado,
		isnull(max(usu_nombre),'-') Repartidor
	INTO #Ganancia
	FROM Pedido
	JOIN pedido_detalle on det_pedido = ped_id
	JOIN producto on prod_id = det_producto
	JOIN precio on ped_fecha > pre_fecha and (ped_fecha < pre_fechaHasta or pre_fechaHasta is null) 
	JOIN precio_detalle on pre_ident = prd_campre and prd_produ = prod_id 
	JOIN cliente on cli_id = ped_cliente
	LEFT JOIN usuario on ped_repartidor = usu_usuario
	group by ped_id, ped_fecha

	SELECT DATENAME(MONTH, DATEADD(MM, s.number, CONVERT(DATETIME, 0))) AS Mes, 
	MONTH(DATEADD(MM, s.number, CONVERT(DATETIME, 0))) AS MesNumero,
	ISNULL(SUM(MontoRemito - costo),0) AS GananciaTotal 
	FROM master.dbo.spt_values s 
	LEFT JOIN #Ganancia ON MONTH(Dia) - 1  = s.number AND YEAR(Dia) = YEAR(CURRENT_TIMESTAMP)
	WHERE [type] = 'P' AND s.number BETWEEN 0 AND 11 
	group by DATENAME(MONTH, DATEADD(MM, s.number, CONVERT(DATETIME, 0))), MONTH(DATEADD(MM, s.number, CONVERT(DATETIME, 0)))
	ORDER BY 2

	SET @@nRet = @@error
	IF @@nRet <> 0 
		RETURN @@nRet

RETURN 0
GO