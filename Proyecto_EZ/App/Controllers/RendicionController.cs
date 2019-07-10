using Entidades;
using Negocios;
using Negocios.BusinessControllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Controllers
{
    public class RendicionController : Controller
    {
        RendicionCtrl xoRendicionCtrl = new Factory().GetCtrlRendicion();

        public ActionResult Index()
        {
            if (Session["Usuario"] == null)
                return RedirectToAction("Index", "Login");

            return View();
        }

        public JsonResult GetRendicion(string xsFechaDesde, string xsFechaHasta)
        {
            string xsError = "";
            DateTime xdFechaDesde = DateTime.MinValue, xdFechaHasta = DateTime.MinValue;

            if (!string.IsNullOrEmpty(xsFechaDesde)) xdFechaDesde = Convert.ToDateTime(xsFechaDesde);
            if (!string.IsNullOrEmpty(xsFechaHasta)) xdFechaHasta = Convert.ToDateTime(xsFechaHasta);

            var lstRendicion = xoRendicionCtrl.ObtenerRendicion(xdFechaDesde, xdFechaHasta, out xsError);

     var resultadoJS = new
     {
         data = lstRendicion,
         error = xsError
     };
            return Json(resultadoJS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PostGuardarRendicion(RendicionForm xoRendicion)
        {
            string xsError = "";

            xoRendicionCtrl.GuardarRendicion(xoRendicion, out xsError);

            return Json(xsError);
        }

        public JsonResult PostEliminarRendicion(int xiId)
        {
            string xsError = "";

            xoRendicionCtrl.EliminarRendicion(xiId, out xsError);

            return Json(xsError);
        }
        public JsonResult GetDetallePedido(int xiIdPedido)
        {
            string xsError = "";
            var lstResultado = xoRendicionCtrl.ObtenerDetalleRendicion(xiIdPedido, out xsError);
            var resuladoJS = new
            {
                data = lstResultado,
                error = xsError
            };

            return Json(resuladoJS);
        }
        
    }
}