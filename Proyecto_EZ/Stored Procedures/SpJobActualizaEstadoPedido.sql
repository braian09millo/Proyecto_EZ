IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('SpJobActualizaEstadoPedido') and sysstat & 0xf = 4)
	DROP PROCEDURE SpJobActualizaEstadoPedido
GO

CREATE PROCEDURE SpJobActualizaEstadoPedido											
WITH ENCRYPTION AS

	DECLARE @@nRet INT


	UPDATE pedido
	SET ped_estado = 'E'
	FROM pedido
	WHERE ped_estado = 'C'
	and ped_fecha < getdate()

	SET @@nRet = @@error
	IF @@nRet <> 0 
		RETURN @@nRet

RETURN 0
GO