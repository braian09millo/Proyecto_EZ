IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('spGetUsuarios') and sysstat & 0xf = 4)
	DROP PROCEDURE spGetUsuarios
GO

CREATE PROCEDURE spGetUsuarios														
WITH ENCRYPTION AS

	DECLARE @@nRet INT

	SELECT 
		usu_usuario,
		gru_descr as usu_grupoDesc,
		usu_nombre,
		usu_apellido,
		usu_fecha_acceso,
		usu_password,
		usu_grupo,
		ISNULL(usu_delet,'N') as usu_delet
	FROM usuario
	JOIN grupo ON gru_id = usu_grupo

	SET @@nRet = @@error
	IF @@nRet <> 0 
		RETURN @@nRet

RETURN 0
GO