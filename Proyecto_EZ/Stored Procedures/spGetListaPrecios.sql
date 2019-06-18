IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('spGetListaPrecios') and sysstat & 0xf = 4)
	DROP PROCEDURE spGetListaPrecios
GO

CREATE PROCEDURE spGetListaPrecios														
WITH ENCRYPTION AS

	DECLARE @@nRet INT	
	
	SELECT DISTINCT
		mar_id AS IdMarca,
		mar_nombre AS Marca,
		tam_id AS IdTamanio,
		tam_descripcion AS Tamanio,
		tip_id AS IdTipo,
		CASE
			WHEN tam_id = 13 AND tip_id = 3 THEN 'CERVEZA/CAJONES'
			WHEN tam_id IN (3,4) AND tip_id = 3 THEN 'CERVEZA/LATAS'
			WHEN tam_id = 14 AND tip_id = 3 THEN 'CERVEZA/DESCARTABLES'
			WHEN tam_id IN (17,18) AND tip_id = 3 THEN 'CERVEZA/PORRON'
			ELSE tip_descr
		END AS Tipo,
		prd_precioC AS Costo,
		prd_precioPV AS PrecioVenta,
		prd_porcen AS Porcentaje,
		ROUND((prd_precioPV/prod_pack),0) AS PrecioUnitario
	FROM producto
	JOIN marca ON prod_marca = mar_id
	JOIN modelo ON prod_modelo = mod_id
	JOIN tamanio ON prod_tamanio = tam_id
	JOIN tipo ON prod_tipo = tip_id
	JOIN precio_detalle ON prd_produ = prod_id
	JOIN precio ON pre_ident = prd_campre
	WHERE 
		pre_fechaHasta IS NULL AND
		ISNULL(prod_delet,'N') <> 'S'
	ORDER BY
		IdTipo,
		CASE
			WHEN tam_id = 13 AND tip_id = 3 THEN 'CERVEZA/CAJONES'
			WHEN tam_id IN (3,4) AND tip_id = 3 THEN 'CERVEZA/LATAS'
			WHEN tam_id = 14 AND tip_id = 3 THEN 'CERVEZA/DESCARTABLES'
			WHEN tam_id IN (17,18) AND tip_id = 3 THEN 'CERVEZA/PORRON'
			ELSE tip_descr
		END

	SET @@nRet = @@error
	IF @@nRet <> 0 
		RETURN @@nRet

RETURN 0
GO