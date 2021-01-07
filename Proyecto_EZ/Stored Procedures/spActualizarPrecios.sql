IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('spActualizarPrecios') and sysstat & 0xf = 4)
	DROP PROCEDURE spActualizarPrecios
GO

CREATE PROCEDURE spActualizarPrecios													
WITH ENCRYPTION AS

	DECLARE @@nRet INT
	DECLARE @@identPrecio INT

	-- Cerramos la tarifa anterior e insertamos la nueva cabecera de precios
	UPDATE precio
	SET pre_fechaHasta = GETDATE()
	WHERE pre_fechaHasta IS NULL

	INSERT INTO precio (pre_fecha, pre_fechaHasta)
	VALUES(GETDATE(), NULL)

	SET @@identPrecio = SCOPE_IDENTITY()

	--Obtenemos todos los productos para la nueva tarifa
	SELECT DISTINCT
		@@identPrecio AS IdentPrecio,
		prod_id AS IdProducto,
		Costo AS Costo,
		Porcentaje AS Porcentaje,
		PrecioVenta AS PrecioVenta
	INTO #AuxPreciosNuevos
	FROM producto
	JOIN AuxPrecios ON prod_marca = Marca and prod_modelo = Modelo and prod_tamanio = Tamanio
	JOIN precio_detalle ON prd_produ = prod_id
	WHERE ISNULL(prod_delet, 'N') <> 'S'	

	-- Insertamos todos los registros con la nueva tarifa
	INSERT INTO precio_detalle (prd_campre, prd_produ, prd_precioC, prd_porcen, prd_precioPV)
	SELECT * FROM #AuxPreciosNuevos

	SET @@nRet = @@error
	IF @@nRet <> 0 
		RETURN @@nRet

RETURN 0
GO