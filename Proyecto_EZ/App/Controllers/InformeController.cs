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
            string xsError = "";

            //Cargamos el combo de usuarios
            var xoUsuarios = xoUsuarioCtrl.ObtenerUsuarios(out xsError);
            var lstUsuarios = xoUsuarios.Select(x => new SelectListItem { Value = x.usu_usuario, Text = x.usu_nombre }).ToList();
            ViewBag.Usuarios = lstUsuarios;
            ViewBag.UsuariosPedido = xoUsuarios.OrderBy(x => x.usu_nombre).ToList();

            return View();
        }

        public ActionResult Rendicion()
        {
            return View();
        }
    }
}