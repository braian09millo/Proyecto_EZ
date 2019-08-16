IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('spGetRptPedido') and sysstat & 0xf = 4)
	DROP PROCEDURE spGetRptPedido
GO

CREATE PROCEDURE spGetRptPedido
(
	@Pedido INT = NULL
)													
WITH ENCRYPTION AS

	DECLARE @@nRet INT

	--SELECT 
	--	ped_id AS IdPedido,
	--	cli_nombre AS Cliente,
	--	cli_direccion AS Direccion,
	--	ped_fecha AS Fecha,
	--	'Descuento' AS ProductoDescripcion,
	--	0 AS Cantidad,
	--	0 AS Precio,
	--	(
	--		SELECT ROUND(SUM(det_cantidad * det_precio) * (ISNULL(ped_descu,0) / 100), 0) * -1
	--		FROM pedido 
	--		JOIN pedido_detalle ON det_pedido = ped_id
	--		WHERE ped_id = @Pedido
	--		GROUP BY
	--			ped_descu
				
	--	) AS Monto
	--FROM pedido
	--JOIN pedido_detalle ON det_pedido = ped_id
	--JOIN producto ON det_producto = prod_id
	--JOIN marca ON mar_id = prod_marca
	--JOIN modelo ON prod_modelo = mod_id
	--JOIN tamanio ON prod_tamanio = tam_id
	--JOIN cliente ON cli_id = ped_cliente
	--WHERE 
	--	(det_pedido = @Pedido)
	--GROUP BY 
	--	ped_id, ped_descu, cli_nombre, cli_direccion, ped_fecha
	
	--UNION
	
	SELECT 
		ped_id AS IdPedido,
		cli_nombre AS Cliente,
		cli_direccion AS Direccion,
		ped_fecha AS Fecha,
		CASE mod_nombre 
			WHEN 'No Aplica' 
			THEN mar_nombre + ' - ' + env_descr + ' ' + tam_descripcion 
			ELSE mod_nombre + ' - ' + env_descr + ' ' + tam_descripcion 
		END AS ProductoDescripcion,
		det_cantidad AS Cantidad,
		det_precio AS Precio,
		det_monto AS Monto,
				(
			SELECT ROUND(SUM(det_cantidad * det_precio) * (ISNULL(ped_descu,0) / 100), 0) * -1
			FROM pedido 
			JOIN pedido_detalle ON det_pedido = ped_id
			WHERE ped_id = @Pedido
			GROUP BY
				ped_descu
				
		) AS Descuento
	FROM pedido
	JOIN pedido_detalle ON det_pedido = ped_id
	JOIN producto ON det_producto = prod_id
	JOIN marca ON mar_id = prod_marca
	JOIN modelo ON prod_modelo = mod_id
	JOIN envase ON prod_envase = env_id
	JOIN tamanio ON prod_tamanio = tam_id
	JOIN cliente ON cli_id = ped_cliente
	WHERE 
		(det_pedido = @Pedido)
	ORDER BY
		 Cantidad DESC
	
	SET @@nRet = @@error
	IF @@nRet <> 0 
		RETURN @@nRet

RETURN 0
GO