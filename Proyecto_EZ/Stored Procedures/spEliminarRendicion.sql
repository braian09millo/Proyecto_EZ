IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('spEliminarRendicion') and sysstat & 0xf = 4)
	DROP PROCEDURE spEliminarRendicion
GO

CREATE PROCEDURE spEliminarRendicion
(
	@Rendicion int = NULL
)														
WITH ENCRYPTION AS

	DECLARE @@nRet INT
	
	--Marcamos los pedidos como NO rendidos
	UPDATE pedido SET ped_rendido = 'N'
	FROM pedido
	JOIN rendicion_detalle on red_pedido = ped_id
	where red_rendi = @Rendicion

	SET @@nRet = @@error
	IF @@nRet <> 0 
		RETURN @@nRet

	--Borramos Detalle de Rendicion
	DELETE rendicion_detalle 
	WHERE red_rendi = @Rendicion

		SET @@nRet = @@error
	IF @@nRet <> 0 
		RETURN @@nRet

	--Borramos Encabezado de Rendicion
	DELETE rendicion
	WHERE ren_id = @Rendicion

	SET @@nRet = @@error
	IF @@nRet <> 0 
		RETURN @@nRet

RETURN 0
GO