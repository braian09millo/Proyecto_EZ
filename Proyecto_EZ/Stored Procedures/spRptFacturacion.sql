IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('spRptFacturacion') and sysstat & 0xf = 4)
	DROP PROCEDURE spRptFacturacion
GO

CREATE PROCEDURE spRptFacturacion											
WITH ENCRYPTION AS

	DECLARE @@nRet INT

SELECT max(cli_nombre)+ ' - ' + max(cli_direccion)  as Cliente,
		count(*) as CantidadPedidos,
		SUM(ped_monto) MontoVentas,
		SUM(ped_factu) Debe,
		SUM(ped_monto)-SUM(ped_factu) Facturado,
		max(ped_fecha) as UltimoPedido,
		ROW_NUMBER() OVER(ORDER BY SUM(ped_monto) DESC) AS Ranking

	FROM Pedido
	JOIN cliente on cli_id = ped_cliente
	group by cli_id
	order by sum(ped_monto) desc

	SET @@nRet = @@error
	IF @@nRet <> 0 
		RETURN @@nRet

RETURN 0
GO