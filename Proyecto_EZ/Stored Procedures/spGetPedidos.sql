IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('spGetPedidos') and sysstat & 0xf = 4)
	DROP PROCEDURE spGetPedidos
GO

CREATE PROCEDURE spGetPedidos	
(
	@FechaDesde DATETIME = NULL,
	@FechaHasta DATETIME = NULL,
	@Cliente INT = NULL,
	@Estado CHAR(2) = NULL,
	@Usuario VARCHAR(10) = NULL
)													
WITH ENCRYPTION AS

	DECLARE @@nRet INT

	SELECT 
		ped_id AS IdPedido,
		cli_nombre AS Cliente,
		ped_fecha AS Fecha,
		ped_estado AS Estado,
		ped_monto AS Monto,
		ped_resto AS Facturado,
		ISNULL(usu_nombre, '') AS Repartidor
	FROM pedido
	JOIN cliente ON cli_id = ped_cliente
	JOIN usuario ON usu_usuario = ped_repartidor
	WHERE 
		(ped_fecha BETWEEN @FechaDesde AND @FechaHasta) AND
		(ped_cliente = @Cliente OR @Cliente IS NULL) AND
		(ped_estado = @Estado OR @Estado IS NULL) AND
		(ped_repartidor = @Usuario OR @Usuario IS NULL)
	
	SET @@nRet = @@error
	IF @@nRet <> 0 
		RETURN @@nRet

RETURN 0
GO