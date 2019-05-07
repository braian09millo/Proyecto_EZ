IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('spGetRptPedido') and sysstat & 0xf = 4)
	DROP PROCEDURE spGetRptPedido
GO

CREATE PROCEDURE spGetRptPedido
(
	@Pedido INT = NULL
)													
WITH ENCRYPTION AS

	DECLARE @@nRet INT

	SELECT 
		ped_id AS IdPedido,
		cli_nombre AS Cliente,
		cli_direccion AS Direccion,
		ped_fecha AS Fecha,
		mod_nombre + ' - ' + tam_descripcion AS ProductoDescripcion,
		det_cantidad AS Cantidad,
		det_precio AS Precio,
		det_monto AS Monto
	FROM pedido
	JOIN pedido_detalle ON det_pedido = ped_id
	JOIN producto ON det_producto = prod_id
	JOIN modelo ON prod_modelo = mod_id
	JOIN tamanio ON prod_tamanio = tam_id
	JOIN cliente ON cli_id = ped_cliente
	WHERE 
		(det_pedido = @Pedido)
	
	SET @@nRet = @@error
	IF @@nRet <> 0 
		RETURN @@nRet

RETURN 0
GO