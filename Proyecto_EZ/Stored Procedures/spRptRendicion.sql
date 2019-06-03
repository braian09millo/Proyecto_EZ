IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('spRptRendicion') and sysstat & 0xf = 4)
	DROP PROCEDURE spRptRendicion
GO

CREATE PROCEDURE spRptRendicion (@FechaDesde datetime, @FechaHasta datetime)												
WITH ENCRYPTION AS

	DECLARE @@nRet INT

	UPDATE pedido
	SET ped_rendido = 'S'
	WHERE ped_fecha between @FechaDesde and @FechaHasta

	SELECT  
		sum(det_cantidad) Cantidad,
		max(mod_nombre + ' - ' + tam_descripcion) Descripcion,
		max(prd_precioPV) Precio,
		sum(prd_precioPV * det_cantidad) Total
	FROM Pedido
		JOIN pedido_detalle on det_pedido = ped_id
		JOIN producto on prod_id = det_producto
		JOIN marca on mar_id = prod_marca
		JOIN modelo on mod_id = prod_modelo
		JOIN tamanio on tam_id = prod_tamanio
		JOIN precio on ped_fecha between pre_fecha and pre_fechaHasta
		JOIN precio_detalle on pre_ident = prd_campre and prd_produ = prod_id 
	WHERE 
		ped_fecha between @FechaDesde and @FechaHasta
	GROUP BY 
		prod_id

	SET @@nRet = @@error
	IF @@nRet <> 0 
		RETURN @@nRet

RETURN 0
GO