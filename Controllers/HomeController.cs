using CrudDetalle.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Xml.Linq;
using System.Data;
using System.Data.SqlClient;
namespace CrudDetalle.Controllers
{
    public class HomeController : Controller
    {
        private readonly string cadenaSQL;

        public HomeController(IConfiguration config)
        {
            cadenaSQL= config.GetConnectionString("cadenaSQL");
        }

        public IActionResult Index()
        {



              
            return View();
        }
        [HttpPost]
        public JsonResult GuardarVenta([FromBody]Venta body)
        {
            XElement venta = new XElement("Venta",
    new XElement("NumeroDocumento", body.NumeroDocumento),
    new XElement("RazonSocial",body.RazonSocial),
    new XElement("Total",body.Total)
      );

            XElement oDetalleVenta = new XElement("Detalle_Venta");

            foreach(DetalleVenta item in body.lstdetalleVenta)
            {
                oDetalleVenta.Add(new XElement("Item",
                    new XElement("Producto", item.Producto),
                    new XElement("Precio", item.Precio),
                    new XElement("Cantidad", item.Cantidad),
                    new XElement("Total", item.Total)
                    ));
            }
                
            venta.Add(oDetalleVenta);

            using (SqlConnection conexion = new SqlConnection(cadenaSQL)) { 
            
                conexion.Open();
                SqlCommand cmd = new SqlCommand("sp_GuardarVenta", conexion);
                cmd.Parameters.Add("Venta_xml", SqlDbType.Xml).Value = venta.ToString();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
            }
                return Json(new {respuesta=true});
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
