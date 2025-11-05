/*Tablas en español*/

use recordsNatacion
CREATE TABLE [dbo].RecordsNatacionAtleta(
	[IdAtleta] [int] NOT NULL IDENTITY(1,1) PRIMARY KEY,
	NombreCompleto [nvarchar](50) NOT NULL,
	FechaNacimiento datetime null default null, --[nvarchar](10) NULL,
	Genero [nvarchar](1) NULL DEFAULT null,
	Pais [nvarchar](3) NULL DEFAULT null, --'ESP',
	Licencia [nvarchar](20) NULL,
	CodigoClub [int] NULL DEFAULT NULL, --300
	NombreCompletoClub [nvarchar](100) NULL DEFAULT null, --'Tenis Pamplona C.',
	NombreCortoClub [nvarchar](50) NULL DEFAULT null,--'T. Pamplona'
)
select * from RecordsNatacionAtleta
delete from RecordsNatacionAtleta

drop table [dbo].RecordsNatacionCompeticion

CREATE TABLE [dbo].RecordsNatacionCompeticion( -- aka, event
	[IdCompeticion] [int] NOT NULL IDENTITY(1,1) PRIMARY KEY,
	NombreCompeticion [nvarchar](50) NULL,
	FechaCompeticion [datetime] NULL,
	Pais nvarchar(3) NULL,
	Ciudad [nvarchar](20) NULL DEFAULT null,
	--Tipo [nvarchar](20) NULL DEFAULT 'Oficial',
	LongitudPiscina int NULL DEFAULT 50,
	NumSesion [int] NULL DEFAULT 0, 
	NombreSesion [nvarchar](50) NULL DEFAULT NULL, 
	CategoriaGenero [nvarchar](1) NULL DEFAULT null, -- F, M, X
	RondaEvento [nvarchar](10) NULL DEFAULT null, -- ex. PRE, FIN, TIM...
	-- RecorridoEvento [nvarchar](3) NULL DEFAULT null, --ex. LCM, SCM..
	-- DistanciaNado int NULL DEFAULT 0,
	-- EstiloNado [nvarchar](50) NULL DEFAULT NULL,
	CantidadRelevosNado int NULL DEFAULT 0
)
delete from RecordsNatacionCompeticion

CREATE TABLE [dbo].RecordsNatacionMarca( --aka, result
	[IdMarca] [int] NOT NULL IDENTITY(1,1) PRIMARY KEY,
	FechaMarca datetime not null, 
	TiempoNado [nvarchar](11) NULL,
	Puntos INT NULL DEFAULT 0,
	Comentario [nvarchar](50) NULL DEFAULT NULL,

	RecorridoNado [nvarchar](3) NULL DEFAULT null, --ex. LCM, SCM..
	DistanciaNado int NULL DEFAULT 0,
	DistanciaSplit INT NULL DEFAULT null,
	EstiloNado [nvarchar](50) NULL DEFAULT NULL,
	-- EsPuntuacionFina INT NULL DEFAULT 1,
	-- TiempoDeEntrada [nvarchar](11) NULL DEFAULT NULL,

	EdadAtleta int null default null,
	IdCategoriaEdad int NULL DEFAULT NULL,
	-- NombreGrupoEdad [nvarchar](20) NULL DEFAULT NULL,
	-- EdadMaxGrupoEdad INT NULL DEFAULT -1,
	-- EdadMinGrupoEdad INT NULL DEFAULT -1, 
	IdEvento INT NULL DEFAULT -1,
	IdAtleta int NOT NULL DEFAULT -1
)


select DISTINCT og.* --hay algunos duplicados de cuando consigue la misma puntuación
from RecordsNatacionMarca og
inner join (
	SELECT DISTINCT MAX(PUNTOS) AS 'PuntosMax',EstiloNado,DistanciaNado,RecorridoNado,NombreGrupoEdad 
	FROM RecordsNatacionMarca 
	where IdAtleta = 1 and NombreGrupoEdad IS NOT NULL
	GROUP BY EstiloNado,DistanciaNado,RecorridoNado,NombreGrupoEdad
	HAVING (MAX(Puntos) > 0)
) as grouped on grouped.PuntosMax = og.Puntos
	and grouped.DistanciaNado = og.DistanciaNado 
	and grouped.EstiloNado = og.EstiloNado
	and grouped.RecorridoNado = og.RecorridoNado
	and grouped.NombreGrupoEdad = og.NombreGrupoEdad
where og.IdAtleta = 1
Order by og.EstiloNado,og.DistanciaNado,og.RecorridoNado,og.NombreGrupoEdad;

SELECT DISTINCT MAX(PUNTOS) AS 'Puntos',EstiloNado,DistanciaNado,RecorridoNado,NombreGrupoEdad 
FROM RecordsNatacionMarca 
where IdAtleta = 1 and NombreGrupoEdad IS NOT NULL
GROUP BY EstiloNado,DistanciaNado,RecorridoNado,NombreGrupoEdad
HAVING (MAX(Puntos) > 0)

select * from RecordsNatacionMarca where IdMarca=4441

use recordsNatacion
select * from RecordsNatacionAtleta
select * from RecordsNatacionCompeticion
select * from RecordsNatacionMarca
select * from RecordsNatacionRecord



SELECT * FROM RecordsNatacionMarca WHERE IdAtleta=671 ORDER BY FechaMarca DESC

--delete from RecordsNatacionAtleta
--delete from RecordsNatacionCompeticion
--delete from RecordsNatacionMarca

use recordsNatacion
CREATE TABLE [dbo].RecordsNatacionCategoriaEdad( --aka, age group
	[IdCategoriaEdad] [int] NOT NULL IDENTITY(1,1) PRIMARY KEY,
	EdadMaxima INT NULL DEFAULT -1, -- included
	EdadMinima INT NULL DEFAULT -1, -- included
	Genero [nvarchar](1) NULL DEFAULT null, -- HACER GROUPING POR NOMBRE Y GENERO PARA SABER QUE AÑO CORRESPONDE
	NombreCategoria [nvarchar](50) NULL default null
)
drop table  [dbo].RecordsNatacionCategoriaEdad

-- datos según RFEN 
INSERT INTO [dbo].[RecordsNatacionCategoriaEdad]
           ([EdadMinima]
           ,[EdadMaxima]
		   ,genero
           ,[NombreCategoria])
     VALUES
           (11,12,'H','Benjamín'),
           (10,11,'M','Benjamín'),
           (13,14,'H','Alevín'),
           (12,13,'M','Alevín'),
           (15,16,'H','Infantil'),
           (14,15,'M','Infantil'),
           (17,18,'H','Júnior'),
           (16,17,'M','Júnior'),
           (19,-1,'H','Absoluto'),
           (18,-1,'M','Absoluto')
/* NOTAS SOBRE GRUPO DE EDAD:
- cambias de categoría el 1 de enero del año en que cumples los años, no el día de tu cumpleaños.
*/


CREATE TABLE [dbo].RecordsNatacionRecord( 
	[IdRecord] [int] NOT NULL IDENTITY(1,1) PRIMARY KEY,
	FechaRecord datetime not null, 
	TiempoNado [nvarchar](11) NULL,
	Puntos INT NULL DEFAULT 0,

	RecorridoNado [nvarchar](3) NULL DEFAULT null, --ex. LCM, SCM..
	DistanciaNado int NULL DEFAULT 0, --total
	DistanciaSplit INT NULL DEFAULT null,
	EstiloNado [nvarchar](50) NULL DEFAULT NULL,

	EdadAtleta int null default null,
	IdCategoriaEdad int NULL DEFAULT NULL,
	IdMarca INT NULL DEFAULT null,
	IdAtleta int NOT NULL DEFAULT -1,
	Archivado int not null default 0 --bool para mostrar en historico de records o no 
)

/*
CREATE TABLE [dbo].RecordsNatacionPrueba( 
	[IdPrueba] [int] NOT NULL IDENTITY(1,1) PRIMARY KEY,
	RecorridoNado [nvarchar](3) NULL DEFAULT null, --ex. LCM, SCM..
	DistanciaNado int NULL DEFAULT 0, --total
	DistanciaSplit INT NULL DEFAULT null,
	EstiloNado [nvarchar](50) NULL DEFAULT NULL,
)
*/  

