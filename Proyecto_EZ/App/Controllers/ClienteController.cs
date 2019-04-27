using Datos;
using Negocios;
using Negocios.BusinessControllers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace App.Controllers
{
    public class ClienteController : Controller
    {
        ClienteCtrl xoClienteCtrl = new Factory().GetCtrlCliente();

        public ActionResult Index()
        {
            string xsError = "";

            var xoProvincias =  xoClienteCtrl.ObtenerProvincias(out xsError);
            var lstProvincias = xoProvincias.Select(x => new SelectListItem() { Value = x.pro_id.ToString(), Text = x.pro_descr }).ToList();
            ViewBag.ComboProvincias = lstProvincias;

            ViewBag.Localidades = xoClienteCtrl.ObtenerLocalidades(out xsError);

            return View();
        }

        public JsonResult GetClientes()
        {
            string xsError = "";

            var xoResultado = xoClienteCtrl.ObtenerClientes(out xsError);

            return Json(xoResultado, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult PostGuardarCliente(cliente xoCliente)
        {
            string xsError = "";

            xoClienteCtrl.GuardarCliente(xoCliente, out xsError);

            return Json(xsError);
        }

        [HttpPost]
        public JsonResult PostEliminarCliente(int xiId)
        {
            string xsError = "";

            xoClienteCtrl.EliminarCliente(xiId, out xsError);

            return Json(xsError);
        }

        [HttpPost]
        public JsonResult PostHabilitarCliente(int xiId)
        {
            string xsError = "";

            xoClienteCtrl.HabilitarCliente(xiId, out xsError);

            return Json(xsError);
        }

    }
}