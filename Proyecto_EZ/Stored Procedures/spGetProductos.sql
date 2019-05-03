IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('spGetProductos') and sysstat & 0xf = 4)
	DROP PROCEDURE spGetProductos
GO

CREATE PROCEDURE spGetProductos														
WITH ENCRYPTION AS

	DECLARE @@nRet INT

	SELECT 
		prod_id AS IdProducto,
		mar_id AS IdMarca,
		mar_nombre AS Marca,
		mod_id AS IdModelo,
		mod_nombre AS Modelo,
		tam_id AS IdTamanio,
		tam_descripcion AS Tamanio,
		tip_id AS IdTipo,
		tip_descr AS Tipo,
		prod_pack AS CantidadPack,
		prd_precioC AS Costo,
		prd_precioPV AS PrecioVenta,
		prd_porcen AS Porcentaje,
		ROUND((prd_precioPV/prod_pack), 0) AS PrecioUnitario,
		ISNULL(prod_delet,'N') AS prod_delet
	FROM producto
	JOIN marca ON prod_marca = mar_id
	JOIN modelo ON prod_modelo = mod_id
	JOIN tamanio ON prod_tamanio = tam_id
	JOIN tipo ON prod_tipo = tip_id
	JOIN precio_detalle ON prd_produ = prod_id
	JOIN precio ON pre_ident = prd_campre
	WHERE 
		pre_fechaHasta IS NULL

	SET @@nRet = @@error
	IF @@nRet <> 0 
		RETURN @@nRet

RETURN 0
GO