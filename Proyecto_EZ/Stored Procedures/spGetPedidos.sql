IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('spGetPedidos') and sysstat & 0xf = 4)
	DROP PROCEDURE spGetPedidos
GO

CREATE PROCEDURE spGetPedidos														
WITH ENCRYPTION AS

	DECLARE @@nRet INT

	SELECT 
		ped_id AS IdPedido,
		cli_nombre AS Cliente,
		ped_fecha AS Fecha,
		ped_monto AS Monto
	FROM pedido
	JOIN cliente ON cli_id = ped_cliente
	

	SET @@nRet = @@error
	IF @@nRet <> 0 
		RETURN @@nRet

RETURN 0
GO