IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('spRptFacturacionMensual') and sysstat & 0xf = 4)
	DROP PROCEDURE spRptFacturacionMensual
GO

CREATE PROCEDURE spRptFacturacionMensual		(@FechaDesde datetime, @FechaHasta datetime,@Repartidor int)												
WITH ENCRYPTION AS

	DECLARE @@nRet INT

SELECT convert(varchar, getdate(), 111) as Dia,
		ped_id IDRemito,
		max(cli_nombre) as Nombre,
		sum(det_cantidad) Packs,
		sum(prd_precioC*det_cantidad) costo,
		max(ped_monto) MontoRemito,
		max(ped_resto) Debe,
		max(ped_monto)-isnull(max(ped_resto),0) Facturado,
		isnull(max(usu_nombre),'-') Repartidor
	FROM Pedido
	JOIN pedido_detalle on det_pedido = ped_id
	JOIN producto on prod_id = det_producto
	JOIN precio on ped_fecha between pre_fecha and pre_fechaHasta
	JOIN precio_detalle on pre_ident = prd_campre and prd_produ = prod_id 
	JOIN cliente on cli_id = ped_cliente
	LEFT JOIN usuario on ped_repartidor = usu_usuario
	WHERE ped_fecha between @FechaDesde and @FechaHasta
	and (ped_repartidor = @Repartidor or @repartidor is null)
	group by ped_id

	SET @@nRet = @@error
	IF @@nRet <> 0 
		RETURN @@nRet

RETURN 0
GO