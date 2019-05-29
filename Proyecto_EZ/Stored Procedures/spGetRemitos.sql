IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('spGetRemitos') and sysstat & 0xf = 4)
	DROP PROCEDURE spGetRemitos
GO

CREATE PROCEDURE spGetRemitos	(@repartidor varchar(10),@FechaDesde datetime,@FechaHasta datetime)														
WITH ENCRYPTION AS

	DECLARE @@nRet INT

SELECT usu_nombre Repartidor,
		ped_fecha Fecha,
		cli_nombre Cliente,
		cli_direccion Direccion,
		mar_nombre Marca,
		mod_nombre Modelo,
		tam_descripcion Tamanio,
		det_cantidad Cantidad,
		det_monto Monto
	FROM Pedido
	JOIN pedido_detalle  on det_pedido = ped_id
	JOIN usuario on usu_usuario = ped_repartidor
	JOIN cliente on cli_id = ped_cliente
	JOIN producto on det_producto = prod_id
	JOIN marca ON prod_marca = mar_id
	JOIN modelo ON prod_modelo = mod_id
	JOIN tamanio ON prod_tamanio = tam_id
	JOIN tipo ON prod_tipo = tip_id
	--JOIN precio_detalle ON prd_produ = prod_id
	--JOIN precio ON pre_ident = prd_campre
	WHERE usu_usuario = @repartidor 
	AND ped_fecha between @FechaDesde and @FechaHasta   
		--(ped_fecha between pre_fecha and pre_fechahasta or pre_fechaHasta IS NULL )
		 

	SET @@nRet = @@error
	IF @@nRet <> 0 
		RETURN @@nRet

RETURN 0
GO