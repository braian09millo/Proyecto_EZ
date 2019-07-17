IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('spRptRendicion') and sysstat & 0xf = 4)
	DROP PROCEDURE spRptRendicion
GO

CREATE PROCEDURE spRptRendicion (@IdRendcion int)												
WITH ENCRYPTION AS

	DECLARE @@nRet INT

	SELECT  
		sum(det_cantidad) Cantidad,
		max(CASE WHEN mod_nombre = 'No aplica' THEN mar_nombre ELSE mod_nombre END + ' - ' + tam_descripcion) Descripcion,
		max(prd_precioPV) Precio,
		sum(prd_precioPV * det_cantidad) Total,
		max(ren_desde) Desde,
		max(ren_hasta) Hasta
	FROM rendicion
		JOIN rendicion_detalle on ren_id = red_rendi
		JOIN Pedido on ped_id = red_pedido
		JOIN pedido_detalle on det_pedido = ped_id
		JOIN producto on prod_id = det_producto
		JOIN marca on mar_id = prod_marca
		JOIN modelo on mod_id = prod_modelo
		JOIN tamanio on tam_id = prod_tamanio
		JOIN precio on ped_fecha > pre_fecha and (ped_fecha < pre_fechaHasta or pre_fechaHasta is null) 
		JOIN precio_detalle on pre_ident = prd_campre and prd_produ = prod_id 
	WHERE 
		ren_id = @IdRendcion
	GROUP BY 
		prod_id
		ORDER BY max(mod_nombre + ' - ' + tam_descripcion)

	SET @@nRet = @@error
	IF @@nRet <> 0 
		RETURN @@nRet

RETURN 0
GO