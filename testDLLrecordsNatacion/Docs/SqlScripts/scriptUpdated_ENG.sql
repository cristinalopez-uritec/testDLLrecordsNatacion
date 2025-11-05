
SELECT TOP (1000) [Id]
      ,[FullName]
      ,[Birthdate]
      ,[Gender]
      ,[Nation]
      ,[License]
      ,[ClubCode]
      ,[ClubName]
      ,[ClubShortName]
  FROM [recordsNatacion].[dbo].[Athlete]

drop table recordsNatacion.dbo.Athlete

CREATE TABLE [dbo].[Athlete](
	[Id] [int] NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[FullName] [nvarchar](50) NOT NULL,
	[Birthdate] [nvarchar](10) NULL,
	[Gender] [nvarchar](1) NULL DEFAULT 'X',
	[Nation] [nvarchar](3) NULL DEFAULT null, --'ESP',
	[License] [nvarchar](20) NULL,
	[ClubCode] [int] NULL DEFAULT -1, --300
	[ClubName] [nvarchar](50) NULL DEFAULT null, --'Tenis Pamplona C.',
	[ClubShortName] [nvarchar](50) NULL DEFAULT null,--'T. Pamplona'
)

select * from Athlete ORDER BY FULLNAME where id = 1
delete from athlete

select * from Athlete 
select * from [Event] 
select * from Result 
select * from Record 

use recordsNatacion
drop table dbo.[Event]
CREATE TABLE [dbo].[Event](
	[Id] [int] NOT NULL IDENTITY(1,1) PRIMARY KEY,
	MeetName [nvarchar](50) NULL,
	MeetDate [datetime] NULL,
	Nation nvarchar(3) NULL,
	City [nvarchar](20) NULL DEFAULT 'X',
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
select  * from [event]
delete from [Event]

CREATE TABLE [dbo].[Result]( --aka, marca
	[Id] [int] NOT NULL IDENTITY(1,1) PRIMARY KEY,
	ResultDate datetime not null, 
	SplitDistance INT NULL DEFAULT 50,
	SwimTime [nvarchar](11) NULL,
	SwimCourse [nvarchar](3) NULL DEFAULT null,
	SwimDistance int NULL DEFAULT 50,
	SwimStroke [nvarchar](50) NULL DEFAULT null,
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

insert into Result (comment) values (null)
select * from Result 
delete from result

drop table dbo.Record;
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
-- las pruebas de los records son un conjunto de distance, course y stroke.
-- age category solo es un dato más para el record, no depende de él

delete from Record;

select * from Record;


select * from [Event] order by MeetDate
/*--------------------------------------------------------------------*/