IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('spRptClientesMorosos') and sysstat & 0xf = 4)
	DROP PROCEDURE spRptClientesMorosos
GO

CREATE PROCEDURE spRptClientesMorosos											
WITH ENCRYPTION AS

	DECLARE @@nRet INT

	SELECT 
  		cli_id IdCliente,
		MAX(cli_nombre + ' - ' + cli_direccion) AS Cliente,
		SUM(ped_monto - ped_factu) Monto,
		MAX(ped_fecha) UltimoPedido,
		ROW_NUMBER() OVER(ORDER BY SUM(ped_monto - ped_factu) DESC) AS Ranking,
		max(isnull(cli_celular,cli_telefono)) Telefono
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