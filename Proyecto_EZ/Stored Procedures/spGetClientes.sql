IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('spGetClientes') and sysstat & 0xf = 4)
	DROP PROCEDURE spGetClientes
GO

CREATE PROCEDURE spGetClientes														
WITH ENCRYPTION AS

	DECLARE @@nRet INT

	SELECT 
		cli_id,
		cli_nombre,
		pro_id as cli_proid,
		pro_descr as cli_provincia,
		loc_id as cli_locid,
		loc_descr as cli_localidad,
		cli_direccion,
		cli_telefono,
		cli_celular,
		cli_celular2
	FROM cliente
	JOIN provincia ON cli_provincia = pro_id
	JOIN localidad ON loc_id = cli_localidad

	SET @@nRet = @@error
	IF @@nRet <> 0 
		RETURN @@nRet

RETURN 0
GO