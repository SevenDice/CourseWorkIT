use [DB_BOOKS]
go

-- Workers
alter table dbo.Workers
alter column FName nvarchar(50) not null;

alter table dbo.Workers
alter column SName nvarchar(50) not null;

alter table dbo.Workers
alter column TName nvarchar(50) not null;

alter table dbo.Workers
alter column [Login] nvarchar(50) not null;

alter table dbo.Workers
alter column [Password] nvarchar(50) not null;

--Places
alter table dbo.Places
alter column [Pname] nvarchar(50) not null;

--Inventory
alter table dbo.Inventory
alter column [IName] nvarchar(50) not null;

--Cultures
alter table dbo.Cultures
alter column [Cname] nvarchar(50) not null;

--Animals
alter table dbo.Animals
alter column [AName] nvarchar(50) not null;