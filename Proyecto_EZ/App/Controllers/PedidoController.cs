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
        UsuarioCtrl xoUsuarioCtrl = new Factory().GetCtrlUsuario();

        public ActionResult Index()
        {
            string xsError = "";

            //Cargamos el combo de clientes
            var xoClientes = xoClienteCtrl.ObtenerClientes(out xsError);
            var lstClientes = xoClientes.OrderBy(x => x.cli_nombre).Select(x => new SelectListItem { Value = x.cli_id.ToString(), Text = x.cli_nombre }).ToList();
            ViewBag.Clientes = lstClientes;
            ViewBag.ClientesPedido = xoClientes.OrderBy(x => x.cli_nombre).ToList();

            //Cargamos el combo de usuarios
            var xoUsuarios = xoUsuarioCtrl.ObtenerUsuarios(out xsError);
            var lstUsuarios = xoUsuarios.Select(x => new SelectListItem { Value = x.usu_usuario, Text = x.usu_nombre }).ToList();
            ViewBag.Usuarios = lstUsuarios;
            ViewBag.UsuariosPedido = xoUsuarios.OrderBy(x => x.usu_nombre).ToList();

            //Cargamos el combo de productos
            ViewBag.Productos = xoProductoCtrl.ObtenerProductos(out xsError);           

            return View();
        }

        public JsonResult GetPedidos(string xsFechaDesde, string xsFechaHasta, string xsCliente, string xsEstado, string xsUsuario)
        {
            string xsError = "";
            DateTime? xdFechaDesde = null;
            DateTime? xdFechaHasta = null;
            int? xiCliente = null;

            if (xsFechaDesde != null) xdFechaDesde = Convert.ToDateTime(xsFechaDesde);
            if (xsFechaHasta != null) xdFechaHasta = Convert.ToDateTime(xsFechaHasta);
            if (xsCliente != null) xiCliente = int.Parse(xsCliente);            

            var lstPedidos = xoPedidoCtrl.ObtenerPedidos(xdFechaDesde, xdFechaHasta, xiCliente, xsEstado, xsUsuario, out xsError);
            var resultadoJS = new {
                data = lstPedidos,
                error = xsError
            };
            return Json(resultadoJS, JsonRequestBehavior.AllowGet);        
        }

        public JsonResult AgregarPedido(int xiCliente, string xsUsuario, decimal xdTotal, decimal xdFacturado, List<Pedido> xoProductos)
        {
            string xsError = "";
            if (xsUsuario == "") xsUsuario = null;
            xoPedidoCtrl.AgregarPedido(xiCliente, xsUsuario, xdTotal, xdFacturado, xoProductos, out xsError);
            return Json(xsError);
        }
    }
}