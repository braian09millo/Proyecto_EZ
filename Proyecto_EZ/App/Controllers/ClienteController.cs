using Datos;
using Negocios;
using Negocios.BusinessControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Controllers
{
    public class ClienteController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetClientes()
        {
            ClienteCtrl xoClienteCtrl = new Factory().GetCtrlCliente();
            string xsError = "";

            var xoResultado = xoClienteCtrl.ObtenerClientes(out xsError);

            return Json(xoResultado, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult PostGuardarCliente(CLIENTE xoCliente)
        {
            ClienteCtrl xoClienteCtrl = new Factory().GetCtrlCliente();
            string xsError = "";

            xoClienteCtrl.GuardarCliente(xoCliente, out xsError);

            return Json(xsError);
        }
    }
}