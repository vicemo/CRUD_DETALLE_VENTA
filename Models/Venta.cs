namespace CrudDetalle.Models
{
    public class Venta
    {
        public int IdVenta { get; set; }
        public int NumeroDocumento { get; set; }

        public string RazonSocial { get; set; }

        public decimal Total {  get; set; }

        public List<DetalleVenta> lstdetalleVenta { get; set; }
    }
}
