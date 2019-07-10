IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('spAgregarRendicion') and sysstat & 0xf = 4)
	DROP PROCEDURE spAgregarRendicion
GO

CREATE PROCEDURE spAgregarRendicion
(
	@Desde datetime = NULL,
	@Hasta datetime = NULL
)														
WITH ENCRYPTION AS

	DECLARE @@nRet INT
	
	--Guardamos los pedidos a rendir
	SELECT ped_id id, max(ped_fecha) fecha, sum(prd_precioC*det_cantidad) costo 
	INTO #Pedidos 
	FROM pedido
	JOIN pedido_detalle on det_pedido = ped_id
	JOIN producto on prod_id = det_producto
	JOIN precio on ped_fecha > pre_fecha and (ped_fecha < pre_fechaHasta or pre_fechaHasta is null) 
	JOIN precio_detalle on pre_ident = prd_campre and prd_produ = prod_id 
	WHERE ped_fecha between @Desde and @Hasta
	and (ped_rendido is null or ped_rendido = 'N')
	GROUP BY ped_id

	--Marcamos los pedidos como rendidos
	UPDATE pedido SET ped_rendido = 'S'
	FROM pedido
	where ped_fecha between @desde and @hasta

	SET @@nRet = @@error
	IF @@nRet <> 0 
		RETURN @@nRet

	INSERT INTO rendicion(ren_desde,ren_hasta,ren_total)
	SELECT @desde,@hasta,sum(costo)
	FROM #Pedidos 

		--Insertamos Detalle de Rendicion
	INSERT INTO rendicion_detalle (red_rendi,red_pedido)
	SELECT (select max(ren_id) from rendicion),id
	FROM #Pedidos

	SET @@nRet = @@error
	IF @@nRet <> 0 
		RETURN @@nRet

RETURN 0
GO