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
	DECLARE @@fechaActual DATETIME = GETDATE()
	DECLARE @@fechaLimite DATETIME = DATEADD(DAY,1,DATEFROMPARTS(YEAR(@@fechaActual),MONTH(@@fechaActual),DAY(@@fechaActual)))

	UPDATE pedido
	SET ped_estado = 'E'
	FROM pedido
	WHERE ped_estado = 'C'
	and ped_fecha < @@fechaLimite

	UPDATE pedido
	SET ped_estado = 'F'
	WHERE ped_estado IN ('E','PP') AND 
		  (ped_monto - ped_factu) = 0

	SET @@nRet = @@error
	IF @@nRet <> 0 
		RETURN @@nRet

	SELECT 
		ped_id AS IdPedido,
		cli_nombre AS Cliente,
		cli_direccion AS ClienteDireccion,
		ped_fecha AS Fecha,
		CASE ped_estado 
			WHEN 'C' THEN 'CARGADO'
			WHEN 'E' THEN 'ENTREGADO'
			WHEN 'F' THEN 'FACTURADO'
			WHEN 'PP' THEN 'PAGO PARCIAL' 
		END AS EstadoDescripcion,
		ped_monto AS Monto,
		ped_factu AS Facturado,
		ISNULL(usu_nombre, '') AS Repartidor,
		ped_estado AS Estado,
		cli_id AS IdCliente,	
		usu_usuario AS IdRepartidor,
		ISNULL(ped_apdes,'N') AS AplicaDescuento,
		ISNULL(ped_descu,0) AS Descuento,
		ISNULL(ped_vuelta,'') AS Vuelta
	FROM pedido
	LEFT JOIN cliente ON cli_id = ped_cliente
	LEFT JOIN usuario ON usu_usuario = ped_repartidor
	WHERE 
		((ped_fecha BETWEEN @FechaDesde AND @FechaHasta) OR (@FechaDesde IS NULL AND @FechaHasta IS NULL)) AND
		(ped_cliente = @Cliente OR @Cliente IS NULL) AND
		(ped_estado = @Estado OR @Estado IS NULL) AND
		(ped_repartidor = @Usuario OR @Usuario IS NULL)
	ORDER BY 
		ped_fecha DESC
	
	SET @@nRet = @@error
	IF @@nRet <> 0 
		RETURN @@nRet

RETURN 0
GO