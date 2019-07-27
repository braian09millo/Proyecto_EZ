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
		det_monto AS Monto,
		CASE 
			WHEN mod_nombre = 'No Aplica' THEN mar_nombre + ' - ' + env_descr + ' ' + tam_descripcion 
			ELSE mod_nombre + ' - ' + env_descr + ' ' + tam_descripcion 
		END AS ProductoDescripcion
	FROM pedido_detalle
	JOIN producto ON det_producto = prod_id
	join marca ON prod_marca = mar_id
	JOIN modelo ON prod_modelo = mod_id
	JOIN envase ON prod_envase = env_id
	JOIN tamanio ON prod_tamanio = tam_id
	WHERE 
		(det_pedido = @Pedido)
	
	SET @@nRet = @@error
	IF @@nRet <> 0 
		RETURN @@nRet

RETURN 0
GO