CREATE DATABASE Parcial2Err;

USE master
GO
CREATE LOGIN usrparcial2 WITH PASSWORD=N'12345678',
	DEFAULT_DATABASE=Parcial2Err,
	CHECK_EXPIRATION=OFF,
	CHECK_POLICY=ON
GO
USE Parcial2Err
GO
CREATE USER usrparcial2 FOR LOGIN usrparcial2
GO
ALTER ROLE db_owner ADD MEMBER usrparcial2
GO

DROP DATABASE Parcial2Err;

CREATE TABLE Serie(
id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
titulo VARCHAR(250) NOT NULL,
sinopsis VARCHAR(5000) NOT NULL,
director VARCHAR(100) NOT NULL,
duracion INT NOT NULL,
fechaEstreno DATE NOT NULL,
);

ALTER TABLE Serie ADD usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME();
ALTER TABLE Serie ADD fechaRegistro DATETIME NOT NULL DEFAULT GETDATE();
ALTER TABLE Serie ADD estado SMALLINT NOT NULL DEFAULT 1; -- -1: Eliminación lógica, 0: Inactivo, 1: Activo;



CREATE PROC paSerieListar @parametro VARCHAR(50)
AS
  SELECT id,titulo,sinopsis,director,duracion,fechaEstreno,usuarioRegistro,fechaRegistro,estado 
  FROM Serie
  WHERE estado<>-1 AND titulo LIKE '%'+REPLACE(@parametro,' ','%')+'%';

  EXEC paSerieListar 'total';
  select * from serie;

  INSERT INTO Serie(titulo,sinopsis,director,duracion,fechaEstreno)
VALUES ('Don Quijote', 'Reflexion mucho', 'Lperez', 30, '12/05/1995');

INSERT INTO Serie(titulo,sinopsis,director,duracion,fechaEstreno)
VALUES ('libro total', 'El Estado total', 'Pedro Montes', 40, '08/05/2020');

INSERT INTO Serie(titulo,sinopsis,director,duracion,fechaEstreno)
VALUES ('El toro andino', 'Es un toro que afectaba a toda una region', 'Fernando Lara', 50, '08/05/2020');

select * from Serie;