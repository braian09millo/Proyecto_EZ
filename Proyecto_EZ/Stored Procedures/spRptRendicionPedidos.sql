IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('spRptRendicionPedidos') and sysstat & 0xf = 4)
	DROP PROCEDURE spRptRendicionPedidos
GO

CREATE PROCEDURE spRptRendicionPedidos (@FechaDesde datetime, @FechaHasta datetime,@rendido char(1))												
WITH ENCRYPTION AS

	DECLARE @@nRet INT



	SELECT  
		ISNULL(CONVERT(varchar,max(ren_desde),103) + ' - ' + CONVERT(varchar,max(ren_hasta),103),'-') FechaRendicion,
		max(cli_nombre) + ' - ' + max(cli_direccion) Cliente,
		max(ped_fecha) FechaPedido,
		sum(prd_precioPV * det_cantidad) Costo,
		max(CASE WHEN ped_rendido='S' THEN 'SI' ELSE 'NO' END ) Rendido
	FROM Pedido
		JOIN pedido_detalle on det_pedido = ped_id
		LEFT JOIN rendicion_detalle on ped_id = red_pedido
		LEFT JOIN rendicion  on ren_id = red_rendi	
		JOIN Cliente on cli_id = ped_cliente
		JOIN producto on prod_id = det_producto
		JOIN marca on mar_id = prod_marca
		JOIN modelo on mod_id = prod_modelo
		JOIN tamanio on tam_id = prod_tamanio
		JOIN precio on ped_fecha > pre_fecha and (ped_fecha < pre_fechaHasta or pre_fechaHasta is null) 
		JOIN precio_detalle on pre_ident = prd_campre and prd_produ = prod_id 
	WHERE 
		ped_fecha between @FechaDesde and @FechaHasta
		and (ped_rendido = @Rendido or isnull(@rendido,'S') = 'S')
		GROUP BY ped_id,ped_fecha
		ORDER BY ped_fecha

	SET @@nRet = @@error
	IF @@nRet <> 0 
		RETURN @@nRet

RETURN 0
GO