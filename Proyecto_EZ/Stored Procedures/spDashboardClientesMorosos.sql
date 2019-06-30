IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('spDashboardClientesMorosos') and sysstat & 0xf = 4)
	DROP PROCEDURE spDashboardClientesMorosos
GO

CREATE PROCEDURE spDashboardClientesMorosos 										
WITH ENCRYPTION AS

	DECLARE @@nRet INT

	SELECT TOP 10
  		cli_id IdCliente,
		MAX(cli_nombre + ' - ' + cli_direccion) AS Cliente,
		SUM(ped_monto - ped_factu) Monto
	FROM pedido
	JOIN cliente ON cli_id = ped_cliente
	WHERE ped_monto - ped_factu > 0
	GROUP BY cli_id
	ORDER BY Monto DESC

	SET @@nRet = @@error
	IF @@nRet <> 0 
		RETURN @@nRet

RETURN 0
GO