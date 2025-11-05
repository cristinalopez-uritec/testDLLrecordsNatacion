use recordsNatacion
CREATE TABLE [dbo].RecordsNatacionAtleta(
	[IdAtleta] [int] NOT NULL IDENTITY(1,1) PRIMARY KEY,
	NombreCompleto [nvarchar](50) NOT NULL,
	FechaNacimiento [nvarchar](10) NULL default null, --[nvarchar](10) NULL,
	Genero [nvarchar](1) NULL DEFAULT null,
	Pais [nvarchar](3) NULL DEFAULT null, --'ESP',
	Licencia [nvarchar](20) NULL,
	CodigoClub [int] NULL DEFAULT -1, --300
	NombreCompletoClub [nvarchar](100) NULL DEFAULT null, --'Tenis Pamplona C.',
	NombreCortoClub [nvarchar](50) NULL DEFAULT null,--'T. Pamplona'
)
CREATE TABLE [dbo].[Athlete](
	[Id] [int] NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[FullName] [nvarchar](50) NOT NULL,
	[Birthdate] [nvarchar](10) NULL,
	[Gender] [nvarchar](1) NULL DEFAULT null,
	[Nation] [nvarchar](3) NULL DEFAULT null, --'ESP',
	[License] [nvarchar](20) NULL,
	[ClubCode] [int] NULL DEFAULT -1, --300
	[ClubName] [nvarchar](50) NULL DEFAULT null, --'Tenis Pamplona C.',
	[ClubShortName] [nvarchar](50) NULL DEFAULT null,--'T. Pamplona'
)
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
	EsPuntuacionFina INT NULL DEFAULT 1,
	TiempoDeEntrada [nvarchar](11) NULL DEFAULT NULL,
	NombreGrupoEdad [nvarchar](20) NULL DEFAULT NULL,
	EdadMaxGrupoEdad INT NULL DEFAULT -1,
	EdadMinGrupoEdad INT NULL DEFAULT -1, 
	IdEvento INT NULL DEFAULT -1,
	IdAtleta int NOT NULL DEFAULT -1
)
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
CREATE TABLE [dbo].Record(
	[Id] [int] NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[RecordDate] datetime not null,
	Position INT NULL DEFAULT -1, -- -1 = not in competition?, save only the first 5 positions for each type of event
	[meetStatus]  [nvarchar](20) NULL DEFAULT null, --ex. official, unofficial
	[recordType] [nvarchar](20) NULL DEFAULT null, --ex. final, partial(if split or relay)
	AgeCategory [nvarchar](20) NULL DEFAULT null,
	SwimTime [nvarchar](11) NULL,
	SwimCourse [nvarchar](3) NULL DEFAULT null,
	SwimDistance int NULL DEFAULT 50,
	SwimStroke [nvarchar](50) NULL DEFAULT null,
	Points INT NULL DEFAULT 0,
	ResultId int NULL DEFAULT -1, -- -1 if no result associated (show as "no data")
	AthleteId int not null
)
CREATE TABLE [dbo].[Result]( --aka, marca
	[Id] [int] NOT NULL IDENTITY(1,1) PRIMARY KEY,
	SplitDistance INT NULL DEFAULT 50,
	SwimTime [nvarchar](11) NULL,
	Points INT NULL DEFAULT 0,
	IsWaScoring INT NULL DEFAULT 1,
	EntryTime [nvarchar](11) NULL DEFAULT NULL,
    Comment [nvarchar](50) NULL DEFAULT NULL,
	AgeGroupName [nvarchar](20) NULL DEFAULT NULL,
	AgeGroupMaxAge INT NULL DEFAULT -1,
	AgeGroupMinAge INT NULL DEFAULT -1, 
	EventId INT NULL DEFAULT -1,
	AthleteId int NULL DEFAULT -1
)
CREATE TABLE [dbo].RecordsNatacionCompeticion( -- aka, event
	[IdCompeticion] [int] NOT NULL IDENTITY(1,1) PRIMARY KEY,
	NombreCompeticion [nvarchar](50) NULL,
	FechaCompeticion [datetime] NULL,
	Pais nvarchar(3) NULL,
	Ciudad [nvarchar](20) NULL DEFAULT null,
	LongitudPiscina int NULL DEFAULT 50,
	NumSesion [int] NULL DEFAULT 0, 
	NombreSesion [nvarchar](50) NULL DEFAULT NULL, 
	CategoriaGenero [nvarchar](1) NULL DEFAULT null, -- F, M, X
	RondaEvento [nvarchar](10) NULL DEFAULT null, -- ex. PRE, FIN, TIM...
	CantidadRelevosNado int NULL DEFAULT 0
)
CREATE TABLE [dbo].[Event](
	[Id] [int] NOT NULL IDENTITY(1,1) PRIMARY KEY,
	MeetName [nvarchar](50) NULL,
	MeetDate [datetime] NULL,
	Nation nvarchar(3) NULL,
	City [nvarchar](20) NULL DEFAULT null,
	[Status] [nvarchar](20) NULL DEFAULT 'NO INFO',
	PoolLength int NULL DEFAULT 50,
	SessionNum [int] NULL DEFAULT 0, 
	SessionName [nvarchar](50) NULL DEFAULT NULL, 
	GenderCategory [nvarchar](1) NULL DEFAULT 'X',
	EventRound [nvarchar](10) NULL DEFAULT '',
	EventCourse [nvarchar](3) NULL DEFAULT 'LCM',
	SwimDistance int NULL DEFAULT 0,
	SwimStroke [nvarchar](50) NULL DEFAULT NULL,
	SwimRelayCount int NULL DEFAULT 0,
)


DROP TABLE [Event]
DROP TABLE Athlete
DROP TABLE Record
DROP TABLE Result
DROP TABLE RecordsNatacionAtleta
DROP TABLE RecordsNatacionCompeticion
DROP TABLE RecordsNatacionMarca
DROP TABLE RecordsNatacionRecord


select * from Athlete 
select * from [Event] 
select * from Result 
select * from Record 

select * from RecordsNatacionAtleta
select * from RecordsNatacionCompeticion
select * from RecordsNatacionMarca
select * from RecordsNatacionRecord
