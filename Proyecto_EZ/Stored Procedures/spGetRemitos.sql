IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('spGetRemitos') and sysstat & 0xf = 4)
	DROP PROCEDURE spGetRemitos
GO

CREATE PROCEDURE spGetRemitos	(@FechaDesde datetime,@FechaHasta datetime,@repartidor varchar(10))														
WITH ENCRYPTION AS

	DECLARE @@nRet INT

SELECT max(usu_nombre) Repartidor,
		ped_fecha Fecha,
		--cli_nombre Cliente,
		--cli_direccion Direccion,
		max(mar_nombre) Marca,
		max(mod_nombre) Modelo,
		max(tam_descripcion) Tamanio,
		sum(det_cantidad) Cantidad
--		det_monto Monto
	FROM Pedido
	JOIN pedido_detalle  on det_pedido = ped_id
	JOIN usuario on usu_usuario = ped_repartidor
	JOIN cliente on cli_id = ped_cliente
	JOIN producto on det_producto = prod_id
	JOIN marca ON prod_marca = mar_id
	JOIN modelo ON prod_modelo = mod_id
	JOIN tamanio ON prod_tamanio = tam_id
	WHERE usu_usuario = @repartidor 
	AND ped_fecha between @FechaDesde and @FechaHasta   
	group by ped_fecha,prod_id	 

	SET @@nRet = @@error
	IF @@nRet <> 0 
		RETURN @@nRet

RETURN 0
GO