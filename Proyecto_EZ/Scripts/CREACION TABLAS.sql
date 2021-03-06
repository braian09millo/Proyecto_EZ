CREATE DATABASE BD_EZ

USE BD_EZ

-- CREACION DE LAS TABLAS CON SUS RESPECTIVAS RELACIONES

CREATE TABLE provincia
(
	pro_id INT IDENTITY PRIMARY KEY,
	pro_descr VARCHAR(50) NOT NULL
)

CREATE TABLE localidad
(
	loc_id INT IDENTITY PRIMARY KEY,
	loc_provincia INT NOT NULL,
	loc_descr VARCHAR(50) NOT NULL
)

CREATE TABLE cliente
(
	cli_id INT IDENTITY PRIMARY KEY,
	cli_nombre VARCHAR(50) NOT NULL,
	cli_provincia INT NOT NULL,
	cli_localidad INT NOT NULL,
	cli_direccion VARCHAR(100) NOT NULL,
	cli_telefono VARCHAR(15) NULL,
	cli_celular VARCHAR(15) NULL,
	cli_celular2 VARCHAR(15) NULL,
	cli_coordenadas VARCHAR(150) NULL,
	cli_mail VARCHAR(50) NULL,
	cli_delet CHAR(1) NULL,
	CONSTRAINT FK_Provincia_Cliente FOREIGN KEY (cli_provincia) REFERENCES provincia(pro_id),
	CONSTRAINT FK_Localidad_Cliente FOREIGN KEY (cli_localidad) REFERENCES localidad(loc_id)	
)

CREATE TABLE grupo
(
	gru_id TINYINT IDENTITY PRIMARY KEY,
	gru_descr VARCHAR(20) NOT NULL
)

CREATE TABLE usuario
(
	usu_usuario VARCHAR(10) PRIMARY KEY,
	usu_password VARCHAR(100) NOT NULL,
	usu_grupo TINYINT NOT NULL,
	usu_nombre VARCHAR(50) NOT NULL,
	usu_apellido VARCHAR(50) NOT NULL,
	usu_fecha_acceso DATETIME NULL,
	usu_delet CHAR(1) NULL,
	CONSTRAINT FK_Grupo_Usuario FOREIGN KEY (usu_grupo) REFERENCES GRUPO(gru_id),
)

CREATE TABLE marca 
(
	mar_id INT IDENTITY PRIMARY KEY,
	mar_nombre VARCHAR(100) NOT NULL,
	mar_delet CHAR(1) NULL
)

CREATE TABLE modelo
(
	mod_id INT IDENTITY,
	mod_marca INT NOT NULL,
	mod_nombre VARCHAR(100) NOT NULL,
	mod_delet CHAR(1) NULL,
	CONSTRAINT FK_Marca_Modelo FOREIGN KEY (mod_marca) REFERENCES MARCA(mar_id),
	CONSTRAINT PK_Modelo PRIMARY KEY (mod_id)
)

CREATE TABLE tipo
(
	tip_id INT IDENTITY PRIMARY KEY,
	tip_descr VARCHAR(50) NOT NULL
)

CREATE TABLE envase
(
	env_id INT IDENTITY PRIMARY KEY,
	env_descr VARCHAR(50) NOT NULL,
	env_delet CHAR(1) NULL
)

CREATE TABLE tamanio 
(
	tam_id INT IDENTITY PRIMARY KEY, 
	tam_envase INT NOT NULL,
	tam_descripcion VARCHAR(50) NOT NULL,
	tam_delet CHAR(1) NULL,
	CONSTRAINT FK_Envase_Tamanio FOREIGN KEY (tam_envase) REFERENCES ENVASE(env_id)
)

CREATE TABLE producto
(
	prod_id INT IDENTITY,
	prod_marca INT NOT NULL,
	prod_modelo INT NOT NULL,
	prod_envase INT NOT NULL,
	prod_tamanio INT NOT NULL,
	prod_tipo INT NOT NULL,
	prod_pack INT NULL,
	prod_delete CHAR(1) NULL,
	CONSTRAINT FK_Marca_Producto FOREIGN KEY (prod_marca) REFERENCES MARCA(mar_id),
	CONSTRAINT FK_Modelo_Producto FOREIGN KEY (prod_modelo) REFERENCES MODELO(mod_id),
	CONSTRAINT FK_Envase_Producto FOREIGN KEY (prod_envase) REFERENCES ENVASE(env_id),
	CONSTRAINT FK_Tamanio_Producto FOREIGN KEY (prod_tamanio) REFERENCES TAMANIO(tam_id),
	CONSTRAINT FK_Tipo_Producto FOREIGN KEY (prod_tipo) REFERENCES TIPO(tip_id),
	CONSTRAINT PK_Producto PRIMARY KEY (prod_id)
)

CREATE TABLE pedido
(
	ped_id INT IDENTITY PRIMARY KEY,
	ped_cliente INT NOT NULL,
	ped_fecha DATETIME NOT NULL,
	ped_fechaCarga DATETIME NOT NULL,
	ped_monto MONEY NOT NULL,
	ped_factu MONEY NOT NULL,
	ped_estado CHAR(2) NOT NULL, /* C: cargado, E: entregado, F: facturado, PP: pago parcial */
	ped_repartidor VARCHAR(10) NULL,
	ped_rendido CHAR(1) NULL,
	ped_apdes CHAR(1) NULL, /* Aplica descuento S/N */
	ped_descu DECIMAL NULL,  /* Descuento */
	ped_vuelta TINYINT NULL /* Vuelta para cada tanda de pedidos por repartidor */
	CONSTRAINT FK_Cliente_Pedido FOREIGN KEY (ped_cliente) REFERENCES CLIENTE(cli_id),
	CONSTRAINT FK_Usuario_Pedido FOREIGN KEY (ped_repartidor) REFERENCES USUARIO(usu_usuario)
)

CREATE TABLE pedido_detalle
(
	det_pedido INT NOT NULL,
	det_producto INT NOT NULL,
	det_cantidad INT NOT NULL,
	det_precio MONEY NOT NULL,
	det_monto MONEY NOT NULL,
	CONSTRAINT FK_Producto_DetallePedido FOREIGN KEY (det_producto) REFERENCES PRODUCTO(prod_id),
	CONSTRAINT FK_Pedido_DetallePedido FOREIGN KEY (det_pedido) REFERENCES PEDIDO(ped_id),
	CONSTRAINT PK_Pedido_Detalle PRIMARY KEY (det_pedido,det_producto)

)

CREATE TABLE remito
(
	rem_numero INT IDENTITY PRIMARY KEY,
	rem_cliente INT NOT NULL,
	rem_pedido INT NOT NULL,
	CONSTRAINT FK_Pedido_Remito FOREIGN KEY (rem_pedido) REFERENCES PEDIDO(ped_id),
	CONSTRAINT FK_Cliente_Remito FOREIGN KEY (rem_cliente) REFERENCES CLIENTE(cli_id)
)

CREATE TABLE precio
(
	pre_ident INT IDENTITY PRIMARY KEY,
	pre_fecha DATETIME NOT NULL,
	pre_fechaHasta DATETIME NULL
)

CREATE TABLE precio_detalle
(
	prd_campre INT NOT NULL, --Identity de tabla Precio
	prd_produ INT NOT NULL, --Id del producto
	prd_precioC SMALLMONEY NOT NULL,
	prd_porcen SMALLMONEY NOT NULL,
	prd_precioPV SMALLMONEY NOT NULL, --Precio 
	CONSTRAINT FK_Precio_Precio_Detalle FOREIGN KEY (prd_campre) REFERENCES PRECIO(pre_ident),
	CONSTRAINT FK_Producto_Precio_Detalle FOREIGN KEY (prd_produ) REFERENCES PRODUCTO(prod_id),
	CONSTRAINT PK_Precio_Detalle PRIMARY KEY (prd_campre,prd_produ)
)

-- Aca se van a volcar los datos de los precios nuevos
-- Para despu�s join contra la tabla de precios
CREATE TABLE AuxPrecios 
(
	Marca INT NOT NULL,
	Tamanio INT NOT NULL,
	Modelo INT NOT NULL,
	Costo SMALLMONEY NOT NULL,
	Porcentaje SMALLMONEY NOT NULL,
	PrecioVenta SMALLMONEY NOT NULL,

	CONSTRAINT PK_AuxPrecios PRIMARY KEY (Marca,Modelo,Tamanio)
)

CREATE TABLE gasto
(
	gas_id INT IDENTITY PRIMARY KEY,
	gas_fecha DATETIME NOT NULL,
	gas_descripcion VARCHAR(100) NOT NULL,
	gas_monto MONEY NOT NULL
)

CREATE TABLE rendicion
(
	ren_id INT IDENTITY PRIMARY KEY,
	ren_desde DATETIME NOT NULL,
	ren_hasta DATETIME NOT NULL,
	ren_total MONEY NOT NULL
)


CREATE TABLE rendicion_detalle
(
	red_rendi INT,
	red_pedido DATETIME NOT NULL
)

GO


--Inicializacion de tablas
INSERT INTO TIPO (tip_descr) VALUES ('GASEOSA')
INSERT INTO TIPO (tip_descr) VALUES ('AGUA')
INSERT INTO TIPO (tip_descr) VALUES ('CERVEZA')
INSERT INTO TIPO (tip_descr) VALUES ('JUGO')
INSERT INTO TIPO (tip_descr) VALUES ('VARIOS')

INSERT INTO ENVASE(env_descr) VALUES('LATA')
INSERT INTO ENVASE(env_descr) VALUES('PORRON')
INSERT INTO ENVASE(env_descr) VALUES('BOTELLA')
INSERT INTO ENVASE(env_descr) VALUES('RETORNABLE')
INSERT INTO ENVASE(env_descr) VALUES('DESCARTABLE')
INSERT INTO ENVASE(env_descr) VALUES('CARTON')

INSERT INTO TAMANIO (tam_descripcion) VALUES ('X 200 cm3')
INSERT INTO TAMANIO (tam_descripcion) VALUES ('X 273 cm3')
INSERT INTO TAMANIO (tam_descripcion) VALUES ('X 330 cm3') /* PORRON */
INSERT INTO TAMANIO (tam_descripcion) VALUES ('X 354 cm3') /* LATA */
INSERT INTO TAMANIO (tam_descripcion) VALUES ('X 473 cm3') /* LATA */
INSERT INTO TAMANIO (tam_descripcion) VALUES ('X 500 cm3')
INSERT INTO TAMANIO (tam_descripcion) VALUES ('X 600 cm3')
INSERT INTO TAMANIO (tam_descripcion) VALUES ('X 710 cm3') /* PORRON */
INSERT INTO TAMANIO (tam_descripcion) VALUES ('X 750 cm3')
INSERT INTO TAMANIO (tam_descripcion) VALUES ('X 1 lt') /* BOTELLA, DESCARTABLE, RETORNABLE */
INSERT INTO TAMANIO (tam_descripcion) VALUES ('X 1,5 lts')
INSERT INTO TAMANIO (tam_descripcion) VALUES ('X 1,75 lts')
INSERT INTO TAMANIO (tam_descripcion) VALUES ('X 2 lts')
INSERT INTO TAMANIO (tam_descripcion) VALUES ('X 2,25 lts')
INSERT INTO TAMANIO (tam_descripcion) VALUES ('X 2,5 lts')

INSERT INTO MARCA (mar_nombre) VALUES ('L�nea Coca-Cola') -- 1
INSERT INTO MARCA (mar_nombre) VALUES ('L�nea Pepsi') -- 2
INSERT INTO MARCA (mar_nombre) VALUES ('Levite') -- 3
INSERT INTO MARCA (mar_nombre) VALUES ('Villavicencio')-- 4
INSERT INTO MARCA (mar_nombre) VALUES ('Baggio') -- 4
INSERT INTO MARCA (mar_nombre) VALUES ('Quilmes') -- 5
INSERT INTO MARCA (mar_nombre) VALUES ('Brahma') -- 6
INSERT INTO MARCA (mar_nombre) VALUES ('Stella Artois') -- 7
INSERT INTO MARCA (mar_nombre) VALUES ('Heineken') -- 8
INSERT INTO MARCA (mar_nombre) VALUES ('Schneider') -- 9
INSERT INTO MARCA (mar_nombre) VALUES ('Budweiser') -- 10
INSERT INTO MARCA (mar_nombre) VALUES ('Isenbeck') -- 11
INSERT INTO MARCA (mar_nombre) VALUES ('Gatorade') -- 12
INSERT INTO MARCA (mar_nombre) VALUES ('Corona') -- 13
INSERT INTO MARCA (mar_nombre) VALUES ('Fernet Branca') -- 14
INSERT INTO MARCA (mar_nombre) VALUES ('Vodka Smirnoff') -- 15
INSERT INTO MARCA (mar_nombre) VALUES ('Gancia') -- 16
INSERT INTO MARCA (mar_nombre) VALUES ('Campari') -- 17
INSERT INTO MARCA (mar_nombre) VALUES ('Speed Energizante') -- 18
INSERT INTO MARCA (mar_nombre) VALUES ('Red Bull') -- 19
INSERT INTO MARCA (mar_nombre) VALUES ('Cepita')
INSERT INTO MARCA (mar_nombre) VALUES ('Miller')
INSERT INTO MARCA (mar_nombre) VALUES ('Frizze Blue')
INSERT INTO MARCA (mar_nombre) VALUES ('Vino Norton Cosecha Tard�a')
INSERT INTO MARCA (mar_nombre) VALUES ('Brav�o 4 Continentes')
INSERT INTO MARCA (mar_nombre) VALUES ('Vino Benjamin Nieto')
INSERT INTO MARCA (mar_nombre) VALUES ('Sidra Real')
INSERT INTO MARCA (mar_nombre) VALUES ('Sidra 1888')
INSERT INTO MARCA (mar_nombre) VALUES ('Anana Fizz Real')

INSERT INTO MODELO (mod_marca, mod_nombre) VALUES (1, 'Coca')
INSERT INTO MODELO (mod_marca, mod_nombre) VALUES (1, 'Sprite')
INSERT INTO MODELO (mod_marca, mod_nombre) VALUES (1, 'Fanta')
INSERT INTO MODELO (mod_marca, mod_nombre) VALUES (1, 'Coca Light')
INSERT INTO MODELO (mod_marca, mod_nombre) VALUES (1, 'Coca Zero')
INSERT INTO MODELO (mod_marca, mod_nombre) VALUES (1, 'Sprite Zero')

INSERT INTO MODELO (mod_marca, mod_nombre) VALUES (2, 'Pepsi')
INSERT INTO MODELO (mod_marca, mod_nombre) VALUES (2, '7up')
INSERT INTO MODELO (mod_marca, mod_nombre) VALUES (2, 'Pepsi Light')
INSERT INTO MODELO (mod_marca, mod_nombre) VALUES (2, '7up Free')
INSERT INTO MODELO (mod_marca, mod_nombre) VALUES (2, 'PLT T�nica')
INSERT INTO MODELO (mod_marca, mod_nombre) VALUES (2, 'PLT Pomelo')

INSERT INTO MODELO (mod_marca, mod_nombre) VALUES (3, 'Anana')
INSERT INTO MODELO (mod_marca, mod_nombre) VALUES (3, 'Pera')
INSERT INTO MODELO (mod_marca, mod_nombre) VALUES (3, 'Pomelo')
INSERT INTO MODELO (mod_marca, mod_nombre) VALUES (3, 'Manzana')
INSERT INTO MODELO (mod_marca, mod_nombre) VALUES (3, 'Naranja')
INSERT INTO MODELO (mod_marca, mod_nombre) VALUES (3, 'Lim�n Dulce')
INSERT INTO MODELO (mod_marca, mod_nombre) VALUES (3, 'Frutilla-Lim�n')
INSERT INTO MODELO (mod_marca, mod_nombre) VALUES (3, 'Naranja-Mango')

INSERT INTO MODELO (mod_marca, mod_nombre) VALUES (7, 'Naranja')
INSERT INTO MODELO (mod_marca, mod_nombre) VALUES (7, 'Manzana')
INSERT INTO MODELO (mod_marca, mod_nombre) VALUES (7, 'Multifruta')

INSERT INTO MODELO (mod_marca, mod_nombre) VALUES (15, 'Cool Blue')
INSERT INTO MODELO (mod_marca, mod_nombre) VALUES (15, 'Frutas Tropicales')
INSERT INTO MODELO (mod_marca, mod_nombre) VALUES (15, 'Manzana')

INSERT INTO MODELO (mod_marca, mod_nombre) VALUES (4, 'Villavicencio c/gas')
INSERT INTO MODELO (mod_marca, mod_nombre) VALUES (4, 'Villavicencio s/gas')
INSERT INTO MODELO (mod_marca, mod_nombre) VALUES (4, 'Villavicencio Sport')

INSERT INTO modelo (mod_marca, mod_nombre)
SELECT mar_id, 'No Aplica' 
--select *
FROM marca
LEFT JOIN modelo ON mar_id = mod_marca
WHERE mod_id IS NULL

INSERT INTO provincia (pro_descr) VALUES ('BUENOS AIRES')
INSERT INTO provincia (pro_descr) VALUES ('CABA')

INSERT INTO grupo (gru_descr) VALUES ('Administrador')
INSERT INTO grupo (gru_descr) VALUES ('Repartidor')
