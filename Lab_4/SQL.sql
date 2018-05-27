use [DB_BOOKS]
go

-- Workers
alter table dbo.Workers
alter column FName nvarchar(50) not null;
go

alter table dbo.Workers
alter column SName nvarchar(50) not null;
go

alter table dbo.Workers
alter column TName nvarchar(50) not null;
go

alter table dbo.Workers
alter column [Login] nvarchar(50) not null;
go

alter table dbo.Workers
alter column [Password] nvarchar(50) not null;
go

update dbo.Workers
set [FName] = TRIM([FName]),
	[SName] = TRIM([SName]),
	[TName] = TRIM([TName]),
	[Login] = TRIM([Login]),
	[Password] = TRIM([Password]);
go

--Places
alter table dbo.Places
alter column [Pname] nvarchar(50) not null;
go

update dbo.Places
set [Pname] = TRIM(Pname);
go

--Inventory
alter table dbo.Inventory
alter column [IName] nvarchar(50) not null;
go

update dbo.Inventory
set [Iname] = TRIM(Iname);
go

--Cultures
alter table dbo.Cultures
alter column [Cname] nvarchar(50) not null;
go

update dbo.Cultures
set [Cname] = TRIM(Cname);
go

--Animals
alter table dbo.Animals
alter column [AName] nvarchar(50) not null;
go

update dbo.Animals
set [Aname] = TRIM(Aname);
go