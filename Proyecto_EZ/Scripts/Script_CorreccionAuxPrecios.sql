ALTER TABLE AuxPrecios
DROP CONSTRAINT PK_AuxPrecios

ALTER TABLE AuxPrecios
ADD Modelo INT NOT NULL

ALTER TABLE AuxPrecios
ADD CONSTRAINT PK_AuxPrecios PRIMARY KEY (Marca,Modelo,Tamanio)