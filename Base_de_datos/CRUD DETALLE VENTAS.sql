create table VENTA(
IdVenta int primary key identity(1,1),
NumeroDocumento varchar(20),
RazonSocial varchar(50),
Total decimal(10,2)
)


create table DETALLE_VENTA(
IdDetalleVenta int primary key identity(1,1),
IdVenta int references VENTA(IdVenta),
Producto varchar(50),
Precio decimal(10,2),
Cantidad int,
Total decimal(10,2)
)
Create procedure sp_GuardarVenta(
@venta_xml xml
)
as
begin
	

	insert into VENTA(NumeroDocumento,RazonSocial,Total)
	select
		nodo.elemento.value('NumeroDocumento[1]','varchar(20)') [NumeroDocumento],
		nodo.elemento.value('RazonSocial[1]','varchar(50)') [RazonSocial],
		nodo.elemento.value('Total[1]','decimal(10,2)') [Total]
	from @venta_xml.nodes('Venta') nodo(elemento)


	declare @idventa_generado int = (select max(idventa) from venta)

	insert into DETALLE_VENTA(IdVenta,Producto,Precio,Cantidad,Total)
	select
		@idventa_generado [idventa],
		nodo.elemento.value('Producto[1]','varchar(50)') [Producto],
		nodo.elemento.value('Precio[1]','decimal(10,2)') [Precio],
		nodo.elemento.value('Cantidad[1]','int') [Cantidad],
		nodo.elemento.value('Total[1]','decimal(10,2)') [Total]
	from @venta_xml.nodes('Venta/Detalle_Venta/Item') nodo(elemento)


end

select v.NumeroDocumento,v.RazonSocial,d.IdDetalleVenta,d.Precio,d.Producto from  VENTA v
inner join DETALLE_VENTA d on v.IdVenta =d.IdVenta