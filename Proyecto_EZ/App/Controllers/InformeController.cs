using Negocios;
using Negocios.BusinessControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Controllers
{
    public class InformeController : Controller
    {
        PedidoCtrl xoPedidoCtrl = new Factory().GetCtrlPedido();
        ClienteCtrl xoClienteCtrl = new Factory().GetCtrlCliente();
        ProductoCtrl xoProductoCtrl = new Factory().GetCtrlProducto();
        UsuarioCtrl xoUsuarioCtrl = new Factory().GetCtrlUsuario();

        public ActionResult FacturacionMensual()
        {
            if (Session["Usuario"] == null)
                return RedirectToAction("Index", "Login");

            string xsError = "";

            //Cargamos el combo de usuarios
            var xoUsuarios = xoUsuarioCtrl.ObtenerUsuarios(out xsError).Where(x => x.usu_delet != "S").ToList();
            var lstUsuarios = xoUsuarios.Select(x => new SelectListItem { Value = x.usu_usuario, Text = x.usu_nombre }).ToList();
            ViewBag.Usuarios = lstUsuarios;
            ViewBag.UsuariosPedido = xoUsuarios.OrderBy(x => x.usu_nombre).ToList();

            return View();
        }

        public JsonResult PuedeFacturar(DateTime xdFechaDesde, DateTime xdFechaHasta, string xsRepartidor)
        {
            var xoResultado = xoPedidoCtrl.ObtenerFacturacionMensualRpt(xdFechaDesde, xdFechaHasta, xsRepartidor);
            return Json(xoResultado);
        }

        public ActionResult Rendicion()
        {
            if (Session["Usuario"] == null)
                return RedirectToAction("Index", "Login");

            return View();
        }

        public JsonResult PuedeRendir(DateTime xdFechaDesde, DateTime xdFechaHasta)
        {
            var xoResultado = xoPedidoCtrl.ObtenerRendicionRpt(xdFechaDesde, xdFechaHasta);
            return Json(xoResultado);
        }

        public ActionResult RemitosRepartidores()
        {
            if (Session["Usuario"] == null)
                return RedirectToAction("Index", "Login");

            string xsError = "";

            //Cargamos el combo de usuarios
            var xoUsuarios = xoUsuarioCtrl.ObtenerUsuarios(out xsError).Where(x => x.usu_delet != "S").OrderBy(x => x.usu_nombre).ToList();
            var lstUsuarios = xoUsuarios.Select(x => new SelectListItem { Value = x.usu_usuario, Text = x.usu_nombre }).ToList();
            ViewBag.Usuarios = lstUsuarios;

            return View();
        }

        [HttpPost]
        public JsonResult PuedeObtenerRemito(DateTime xdFecha, string xsRepartidor)
        {
            var xoResultado = xoPedidoCtrl.ObtenerRemitoRepartidorRpt(xdFecha, xsRepartidor);
            return Json(xoResultado);
        }
    }
}