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
	cli_delet CHAR(1) NULL,
	CONSTRAINT FK_Provincia_Cliente FOREIGN KEY (cli_provincia) REFERENCES provincia(pro_id),
	CONSTRAINT FK_Localidad_Cliente FOREIGN KEY (cli_localidad) REFERENCES localidad(loc_id)	
)

CREATE TABLE usuario
(
	usu_usuario VARCHAR(10) PRIMARY KEY,
	usu_password VARCHAR(100) NOT NULL,
	usu_nombre VARCHAR(50) NOT NULL,
	usu_apellido VARCHAR(50) NOT NULL,
	usu_fecha_acceso DATETIME NULL
)

CREATE TABLE marca 
(
	mar_id INT IDENTITY PRIMARY KEY,
	mar_nombre VARCHAR(100) NOT NULL
)

CREATE TABLE modelo
(
	mod_id INT IDENTITY,
	mod_marca INT NOT NULL,
	mod_nombre VARCHAR(100) NOT NULL,
	CONSTRAINT FK_Marca_Modelo FOREIGN KEY (mod_marca) REFERENCES MARCA(mar_id),
	CONSTRAINT PK_Modelo PRIMARY KEY (mod_id)
)

CREATE TABLE tamanio 
(
	tam_id INT IDENTITY PRIMARY KEY, 
	tam_descripcion VARCHAR(50) NOT NULL
)

CREATE TABLE tipo
(
	tip_id INT IDENTITY PRIMARY KEY,
	tip_descr VARCHAR(50) NOT NULL
)

CREATE TABLE producto
(
	prod_id INT IDENTITY,
	prod_marca INT NOT NULL,
	prod_modelo INT NOT NULL,
	prod_tamanio INT NOT NULL,
	prod_tipo INT NOT NULL,
	prod_pack INT NULL,
	prod_delete CHAR(1) NULL,
	CONSTRAINT FK_Marca_Producto FOREIGN KEY (prod_marca) REFERENCES MARCA(mar_id),
	CONSTRAINT FK_Modelo_Producto FOREIGN KEY (prod_modelo) REFERENCES MODELO(mod_id),
	CONSTRAINT FK_Tamanio_Producto FOREIGN KEY (prod_tamanio) REFERENCES TAMANIO(tam_id),
	CONSTRAINT FK_Tipo_Producto FOREIGN KEY (prod_tipo) REFERENCES TIPO(tip_id),
	CONSTRAINT PK_Producto PRIMARY KEY (prod_id)
)

CREATE TABLE pedido
(
	ped_id INT IDENTITY PRIMARY KEY,
	ped_cliente INT NOT NULL,
	ped_fecha DATETIME NOT NULL,
	ped_monto MONEY NOT NULL,
)

CREATE TABLE detalle_pedido
(
	det_id INT IDENTITY PRIMARY KEY,
	det_pedido INT NOT NULL,
	det_producto INT NOT NULL,
	det_cantidad INT NOT NULL,
	det_precio MONEY NOT NULL,
	det_monto MONEY NOT NULL,
	CONSTRAINT FK_Producto_DetallePedido FOREIGN KEY (det_producto) REFERENCES PRODUCTO(prod_id)
)

CREATE TABLE remito
(
	rem_numero INT IDENTITY PRIMARY KEY,
	rem_cliente INT NOT NULL,
	rem_pedido INT NOT NULL,
	CONSTRAINT FK_Pedido_Remito FOREIGN KEY (rem_pedido) REFERENCES PEDIDO(ped_id)
)

CREATE TABLE precio
(
	pre_ident INT IDENTITY PRIMARY KEY,
	pre_fecha datetime NOT NULL,
	pre_fechaHasta INT NULL
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
-- Para después join contra la tabla de precios
CREATE TABLE AuxPrecios 
(
	Id INT NULL,
	Marca INT NOT NULL,
	Modelo INT NULL,
	Tamanio INT NOT NULL,
	Tipo INT NULL,
	CantidadPack INT NOT NULL,
	Costo SMALLMONEY NOT NULL,
	Porcentaje SMALLMONEY NOT NULL,
	PrecioVenta SMALLMONEY NOT NULL,

	CONSTRAINT PK_AuxPrecios PRIMARY KEY (Marca,Tamanio)
)

GO

--Inicializacion de tablas
INSERT INTO TIPO (tip_descr) VALUES ('GASEOSA')
INSERT INTO TIPO (tip_descr) VALUES ('AGUA')
INSERT INTO TIPO (tip_descr) VALUES ('CERVEZA')
INSERT INTO TIPO (tip_descr) VALUES ('JUGO')
INSERT INTO TIPO (tip_descr) VALUES ('VARIOS')

INSERT INTO TAMANIO (tam_descripcion) VALUES ('X 200 cm3')
INSERT INTO TAMANIO (tam_descripcion) VALUES ('X 273 cm3')
INSERT INTO TAMANIO (tam_descripcion) VALUES ('Lata X 354 cm3')
INSERT INTO TAMANIO (tam_descripcion) VALUES ('Lata X 473 cm3')
INSERT INTO TAMANIO (tam_descripcion) VALUES ('X 500 cm3')
INSERT INTO TAMANIO (tam_descripcion) VALUES ('X 600 cm3')
INSERT INTO TAMANIO (tam_descripcion) VALUES ('Sport X 750 cm3')
INSERT INTO TAMANIO (tam_descripcion) VALUES ('X 1 lt')
INSERT INTO TAMANIO (tam_descripcion) VALUES ('X 1,5 lts')
INSERT INTO TAMANIO (tam_descripcion) VALUES ('X 1,75 lts')
INSERT INTO TAMANIO (tam_descripcion) VALUES ('X 2 lts')
INSERT INTO TAMANIO (tam_descripcion) VALUES ('X 2,25 lts')
INSERT INTO TAMANIO (tam_descripcion) VALUES ('Retornable X 1 lt')
INSERT INTO TAMANIO (tam_descripcion) VALUES ('Descartable X 1 lt')
INSERT INTO TAMANIO (tam_descripcion) VALUES ('X 750 cm3')
INSERT INTO TAMANIO (tam_descripcion) VALUES ('X 2,5 lts')
INSERT INTO TAMANIO (tam_descripcion) VALUES ('Porrón X 330 cm3')
INSERT INTO TAMANIO (tam_descripcion) VALUES ('Porrón X 710 cm3')


INSERT INTO MARCA (mar_nombre) VALUES ('Línea Coca-Cola') -- 1
INSERT INTO MARCA (mar_nombre) VALUES ('Línea Pepsi') -- 2
INSERT INTO MARCA (mar_nombre) VALUES ('Levite') -- 3
INSERT INTO MARCA (mar_nombre) VALUES ('Villavicencio')-- 4
INSERT INTO MARCA (mar_nombre) VALUES ('Villavicencio c/gas')
INSERT INTO MARCA (mar_nombre) VALUES ('Villavicencio s/gas') 
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
INSERT INTO MARCA (mar_nombre) VALUES ('Vino Norton Cosecha Tardía')
INSERT INTO MARCA (mar_nombre) VALUES ('Bravío 4 Continentes')
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
INSERT INTO MODELO (mod_marca, mod_nombre) VALUES (2, 'PLT Tónica')
INSERT INTO MODELO (mod_marca, mod_nombre) VALUES (2, 'PLT Pomelo')

INSERT INTO MODELO (mod_marca, mod_nombre) VALUES (3, 'Anana')
INSERT INTO MODELO (mod_marca, mod_nombre) VALUES (3, 'Pera')
INSERT INTO MODELO (mod_marca, mod_nombre) VALUES (3, 'Pomelo')
INSERT INTO MODELO (mod_marca, mod_nombre) VALUES (3, 'Manzana')
INSERT INTO MODELO (mod_marca, mod_nombre) VALUES (3, 'Naranja')
INSERT INTO MODELO (mod_marca, mod_nombre) VALUES (3, 'Limón Dulce')
INSERT INTO MODELO (mod_marca, mod_nombre) VALUES (3, 'Frutilla-Limón')
INSERT INTO MODELO (mod_marca, mod_nombre) VALUES (3, 'Naranja-Mango')

insert into provincia (pro_descr) Values ('BUENOS AIRES')
insert into provincia (pro_descr) Values ('CABA')

