using Entidades;
using Negocios;
using Negocios.BusinessControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Controllers
{
    public class PedidoController : Controller
    {
        PedidoCtrl xoPedidoCtrl = new Factory().GetCtrlPedido();
        ClienteCtrl xoClienteCtrl = new Factory().GetCtrlCliente();
        ProductoCtrl xoProductoCtrl = new Factory().GetCtrlProducto();

        public ActionResult Index()
        {
            string xsError = "";

            //Cargamos el combo de clientes
            var xoClientes = xoClienteCtrl.ObtenerClientes(out xsError);
            var lstClientes = xoClientes.Select(x => new SelectListItem { Value = x.cli_id.ToString(), Text = x.cli_nombre }).ToList();
            ViewBag.Clientes = lstClientes;

            //Cargamos el combo de productos
            ViewBag.Productos = xoProductoCtrl.ObtenerProductos(out xsError);           

            return View();
        }

        public JsonResult GetPedidos()
        {
            string xsError = "";
            var lstPedidos = xoPedidoCtrl.ObtenerPedidos(out xsError);
            var resultadoJS = new {
                data = lstPedidos,
                error = xsError
            };
            return Json(resultadoJS, JsonRequestBehavior.AllowGet);        
        }

        public JsonResult AgregarPedido(int xiCliente, decimal xdTotal, List<Pedido> xoProductos)
        {
            string xsError = "";
            xoPedidoCtrl.AgregarPedido(xiCliente, xdTotal, xoProductos, out xsError);
            return Json(xsError);
        }
    }
}