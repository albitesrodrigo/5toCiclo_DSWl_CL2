create database CL2BautistaAlbites
go

create table tbl_nrocds
(Idcd  int identity(10,2)  primary key,
cliente varchar(255),
CapaDisco int,
CapaCds int,
totalCds int)
go

--creamos sus respectivos procedimientos almacenados
create procedure sp_guardar_cds
@cli varchar(255),
@cad int,
@cac int,
@tot int
as
insert into tbl_nrocds values(@cli,@cad,@cac,@tot)
go

--testeamos
exec sp_guardar_cds 'Grupo Lucero S.A.C.',500,700,732
exec sp_guardar_cds 'Grupo Pana S.A.',400,700,586
exec sp_guardar_cds 'Grupo Aereo E.I.R.L.',450,700,659
exec sp_guardar_cds 'Grupo Ferro S.A.C.',470,700,688
exec sp_guardar_cds 'Grupo Teoma S.A.',410,700,600
exec sp_guardar_cds 'Grupo Fuxion S.A.C.',350,700,512
exec sp_guardar_cds 'Grupo San Luis S.A.',480,700,703
exec sp_guardar_cds 'Grupo Cielo S.A.C.',530,700,776
exec sp_guardar_cds 'El Olivar S.A.',570,700,834
exec sp_guardar_cds 'Rokys E.I.R.L.',510,700,747
exec sp_guardar_cds 'Transportes El Pana S.A.',590,700,864
exec sp_guardar_cds 'Ciberfarma S.A.C.',640,700,936
exec sp_guardar_cds 'Dynamic Gym S.A.',680,700,995
exec sp_guardar_cds 'El Cisne E.I.R.L.',610,700,893
go

--creamos el procedimiento almacenado actualizar
create procedure sp_actualizar_cds
@cod int,
@cli varchar(255),
@cad int,
@cac int,
@tot int
as
update tbl_nrocds set cliente=@cli,CapaDisco=@cad,CapaCds=@cac,totalCds=@tot
where Idcd=@cod;
go
select * from tbl_nrocds
go
--testeamos--
exec sp_actualizar_cds 12,'Grupo Ferre S.A.C.',500,700,732
go

--creamos el procedimiento almacenado listar
create procedure sp_listar_cds
as
select * from tbl_nrocds;
go
--testeamos--
exec sp_listar_cds
go

 --creamos el procedimiento almacenado eliminar..
 create procedure sp_eliminar_cds 
 @cod int
 as
 delete from  tbl_nrocds where Idcd=@cod;
 go
 --testeamos ....
 exec sp_eliminar_cds 10
 go

 --creamos el procedimiento almacenado buscar
create procedure sp_buscar_cds
@cod int
as
select * from tbl_nrocds where Idcd=@cod;
go
--testeamos el procedimiento buscar...
exec sp_buscar_cds 12
go

exec sp_help "tbl_nrocds"
go