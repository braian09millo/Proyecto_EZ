IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('spDashboardProductosMasVendidos') and sysstat & 0xf = 4)
	DROP PROCEDURE spDashboardProductosMasVendidos
GO

CREATE PROCEDURE spDashboardProductosMasVendidos 										
WITH ENCRYPTION AS

	DECLARE @@nRet INT

	SELECT TOP 10
		prod_id AS IdProducto,
		CASE MAX(mod_nombre)
			WHEN 'No Aplica' THEN MAX(mar_nombre + ' - ' + env_descr + ' ' + tam_descripcion)
			ELSE MAX(mod_nombre + ' - ' + env_descr + ' '+ tam_descripcion)
		END AS Producto,
		SUM(det_cantidad) AS CantidadPacks
	FROM producto
		JOIN marca ON mar_id = prod_marca
		JOIN modelo ON mod_id = prod_modelo
		JOIN tamanio ON tam_id = prod_tamanio
		JOIN envase on prod_envase = env_id
		join pedido_detalle ON det_producto = prod_id
	GROUP BY prod_id
	order by SUM(det_cantidad) desc

	SET @@nRet = @@error
	IF @@nRet <> 0 
		RETURN @@nRet

RETURN 0
GO