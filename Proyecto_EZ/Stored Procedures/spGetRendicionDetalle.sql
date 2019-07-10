IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('spGetRendicionDetalle') and sysstat & 0xf = 4)
	DROP PROCEDURE spGetRendicionDetalle
GO

CREATE PROCEDURE spGetRendicionDetalle
(
	@Rendicion INT = NULL
)													
WITH ENCRYPTION AS

	DECLARE @@nRet INT

	SELECT 
		ped_fecha Fecha,
		max(cli_nombre) + ' - ' + max(cli_direccion)  as Nombre,
		sum(prd_precioC*det_cantidad) Costo
	FROM rendicion_detalle
	JOIN Pedido on ped_id = red_pedido
	JOIN pedido_detalle on det_pedido = ped_id	
	JOIN producto on prod_id = det_producto
	JOIN precio on ped_fecha > pre_fecha and (ped_fecha < pre_fechaHasta or pre_fechaHasta is null) 
	JOIN precio_detalle on pre_ident = prd_campre and prd_produ = prod_id 
	JOIN cliente on cli_id = ped_cliente
	WHERE 
		(red_rendi = @Rendicion)
	GROUP BY ped_id,ped_fecha
	ORDER BY ped_fecha

	SET @@nRet = @@error
	IF @@nRet <> 0 
		RETURN @@nRet

RETURN 0
GO