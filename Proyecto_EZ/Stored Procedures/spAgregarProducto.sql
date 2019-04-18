IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('spAgregarProducto') and sysstat & 0xf = 4)
	DROP PROCEDURE spAgregarProducto
GO

CREATE PROCEDURE spAgregarProducto
(
	@Marca INT = NULL,
	@Modelo INT = NULL,
	@Tamanio INT = NULL,
	@Tipo INT = NULL,
	@Cantidad INT = NULL,
	@Costo SMALLMONEY = NULL,
	@Porcentaje DECIMAL = NULL,
	@PrecioVenta SMALLMONEY = NULL
)														
WITH ENCRYPTION AS

	DECLARE @@nRet INT
	DECLARE @@identPrecio INT
	DECLARE @@identProd INT

	--Se inserta con el primer producto
	IF NOT EXISTS (SELECT * FROM precio)
	BEGIN
		INSERT INTO precio (pre_fecha, pre_fechaHasta)
		VALUES (GETDATE(), NULL)
	END

	--Insertamos el producto
	INSERT INTO producto (prod_marca, prod_modelo, prod_tamanio, prod_tipo, prod_pack)
	VALUES (@Marca, @Modelo, @Tamanio, @Tipo, @Cantidad)

	SET @@nRet = @@error
	IF @@nRet <> 0 
		RETURN @@nRet

	-- Insertamos el precio detalle del producto
	SET @@identPrecio = (SELECT pre_ident FROM precio WHERE pre_fechaHasta IS NULL)
	SET @@identProd = SCOPE_IDENTITY()

	INSERT INTO precio_detalle (prd_campre, prd_produ, prd_precioC, prd_porcen, prd_precioPV)
	VALUES (@@identPrecio, @@identProd, @Costo, @Porcentaje, @PrecioVenta)

	SET @@nRet = @@error
	IF @@nRet <> 0 
		RETURN @@nRet

RETURN 0
GO