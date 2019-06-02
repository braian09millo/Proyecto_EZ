IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('spDashboardMayoresVentasClientes') and sysstat & 0xf = 4)
	DROP PROCEDURE spDashboardMayoresVentasClientes
GO

CREATE PROCEDURE spDashboardMayoresVentasClientes 										
WITH ENCRYPTION AS

	DECLARE @@nRet INT

	SELECT TOP 5
		cli_id AS IdCliente,
		MAX(cli_nombre + ' - ' + cli_direccion) AS Cliente,
		CEILING(SUM(ped_monto)) AS Monto
	FROM pedido
	JOIN cliente on cli_id = ped_cliente
	GROUP BY cli_id

	SET @@nRet = @@error
	IF @@nRet <> 0 
		RETURN @@nRet

RETURN 0
GO