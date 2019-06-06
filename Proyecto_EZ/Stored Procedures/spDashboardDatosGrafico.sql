IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('spDashboardDatosGrafico') and sysstat & 0xf = 4)
	DROP PROCEDURE spDashboardDatosGrafico
GO

CREATE PROCEDURE spDashboardDatosGrafico 										
WITH ENCRYPTION AS

	DECLARE @@nRet INT

	SELECT DATENAME(MONTH, DATEADD(MM, s.number, CONVERT(DATETIME, 0))) AS Mes, 
	MONTH(DATEADD(MM, s.number, CONVERT(DATETIME, 0))) AS MesNumero,
	SUM(ped_monto) AS GananciaMensual 
	INTO #GananciaMensual
	FROM master.dbo.spt_values s 
	LEFT JOIN pedido ON MONTH(ped_fecha) - 1  = s.number AND YEAR(ped_fecha) = YEAR(CURRENT_TIMESTAMP)
	WHERE [type] = 'P' AND s.number BETWEEN 0 AND 11 
	group by DATENAME(MONTH, DATEADD(MM, s.number, CONVERT(DATETIME, 0))), MONTH(DATEADD(MM, s.number, CONVERT(DATETIME, 0)))
	ORDER BY 2

	--SELECT DATENAME(MONTH, DATEADD(MM, s.number, CONVERT(DATETIME, 0))) AS Mes, 
	--MONTH(DATEADD(MM, s.number, CONVERT(DATETIME, 0))) AS MesNumero,
	--SUM(gas_monto) AS GastoMensual
	--INTO #GastoMensual 
	--FROM master.dbo.spt_values s 
	--LEFT JOIN gasto ON MONTH(gas_fecha) - 1  = s.number AND YEAR(gas_fecha) = YEAR(CURRENT_TIMESTAMP)
	--WHERE [type] = 'P' AND s.number BETWEEN 0 AND 11 
	--GROUP BY DATENAME(MONTH, DATEADD(MM, s.number, CONVERT(DATETIME, 0))), MONTH(DATEADD(MM, s.number, CONVERT(DATETIME, 0)))
	--ORDER BY 2

	SELECT
		G1.Mes,
		G1.MesNumero,
		ISNULL(G1.GananciaMensual,0) /*- ISNULL(G2.GastoMensual,0)*/ AS GananciaTotal
	FROM #GananciaMensual G1
	--JOIN #GastoMensual G2 ON G1.MesNumero = G2.MesNumero

	SET @@nRet = @@error
	IF @@nRet <> 0 
		RETURN @@nRet

RETURN 0
GO