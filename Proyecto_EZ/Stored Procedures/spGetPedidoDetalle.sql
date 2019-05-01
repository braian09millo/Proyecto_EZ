IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('spGetPedidoDetalle') and sysstat & 0xf = 4)
	DROP PROCEDURE spGetPedidoDetalle
GO

CREATE PROCEDURE spGetPedidoDetalle
(
	@Pedido INT = NULL
)													
WITH ENCRYPTION AS

	DECLARE @@nRet INT

	SELECT 
		det_pedido AS IdPedido,
		det_producto AS IdProducto,
		det_cantidad AS Cantidad,
		det_precio AS Precio,
		det_monto AS Monto
	FROM detalle_pedido
	WHERE 
		(det_pedido = @Pedido)
	
	SET @@nRet = @@error
	IF @@nRet <> 0 
		RETURN @@nRet

RETURN 0
GO