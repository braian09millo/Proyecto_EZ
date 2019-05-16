IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('spRptFacturacion') and sysstat & 0xf = 4)
	DROP PROCEDURE spRptFacturacion
GO

CREATE PROCEDURE spRptFacturacion		(@FechaDesde datetime, @FechaHasta datetime,@Repartidor int)												
WITH ENCRYPTION AS

	DECLARE @@nRet INT

SELECT max(cli_nombre) as Nombre,
		count(*) as [Cantidad Pedidos],
		SUM(ped_monto) [Monto Remitos],
		SUM(ped_factu) [Debe],
		SUM(ped_monto)-SUM(ped_factu) Facturado,
		max(usu_nombre) Repartidor
	FROM Pedido
	JOIN cliente on cli_id = ped_cliente
	LEFT JOIN usuario on ped_repartidor = usu_usuario
	WHERE ped_fecha between @FechaDesde and @FechaHasta
	and (ped_repartidor = @Repartidor or @repartidor is null)
	group by cli_id,usu_usuario


	SET @@nRet = @@error
	IF @@nRet <> 0 
		RETURN @@nRet

RETURN 0
GO