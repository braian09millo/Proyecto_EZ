IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('spRptProductosMasVendidos') and sysstat & 0xf = 4)
	DROP PROCEDURE spRptProductosMasVendidos
GO

CREATE PROCEDURE spRptProductosMasVendidos											
WITH ENCRYPTION AS

	DECLARE @@nRet INT

	SELECT
		prod_id AS IdProducto,
		CASE MAX(mod_nombre)
			WHEN 'No Aplica' THEN MAX(mar_nombre + ' - ' + env_descr + ' ' + tam_descripcion)
			ELSE MAX(mod_nombre + ' - ' + env_descr + ' ' + tam_descripcion)
		END AS Producto,
		SUM(det_cantidad) AS CantidadPacks,
		ROW_NUMBER() OVER(ORDER BY SUM(det_cantidad) DESC) AS Ranking,
		MAX(PRD_PORCEN) Porcentaje,
		SUM(det_cantidad)*max(prod_pack) VentasUnitarias,
		sum(det_monto) TotalFacturado
	FROM producto
		JOIN marca ON mar_id = prod_marca
		JOIN modelo ON mod_id = prod_modelo
		JOIN tamanio ON tam_id = prod_tamanio
		JOIN envase ON env_id = prod_envase 
		join pedido_detalle ON det_producto = prod_id
		JOIN pedido on det_pedido = ped_id
		JOIN precio on ped_fecha > pre_fecha and (ped_fecha < pre_fechaHasta or pre_fechaHasta is null) 
		JOIN precio_detalle on pre_ident = prd_campre and prd_produ = prod_id 

	GROUP BY prod_id
	order by SUM(det_cantidad) desc

	SET @@nRet = @@error
	IF @@nRet <> 0 
		RETURN @@nRet

RETURN 0
GO