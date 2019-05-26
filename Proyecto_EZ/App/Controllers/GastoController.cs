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
    public class GastoController : Controller
    {
        GastosCtrl xoGastoCtrl = new Factory().GetCtrlGasto();

        public ActionResult Index()
        {
            if (Session["Usuario"] == null)
                return RedirectToAction("Index", "Login");

            return View();
        }

        public JsonResult GetGastos(string xsFechaDesde, string xsFechaHasta)
        {
            string xsError = "";
            DateTime xdFechaDesde = DateTime.MinValue, xdFechaHasta = DateTime.MinValue;

            if (!string.IsNullOrEmpty(xsFechaDesde)) xdFechaDesde = Convert.ToDateTime(xsFechaDesde);
            if (!string.IsNullOrEmpty(xsFechaHasta)) xdFechaHasta = Convert.ToDateTime(xsFechaHasta);

            var lstGastos = xoGastoCtrl.ObtenerGastos(xdFechaDesde, xdFechaHasta, out xsError);

            var resultadoJS = new
            {
                data = lstGastos.AsEnumerable().Select(x => new[] {
                    x.gas_fecha.ToString("dd-MM-yyyy"),
                    x.gas_descripcion,
                    string.Format("{0:0.##}", x.gas_monto),
                    JsonConvert.SerializeObject(x)
                }),
                error = xsError
            };

            return Json(resultadoJS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PostGuardarGasto(GastoForm xoGasto)
        {
            string xsError = "";

            xoGastoCtrl.GuardarGasto(xoGasto, out xsError);

            return Json(xsError);
        }

        public JsonResult PostEliminarGasto(int xiId)
        {
            string xsError = "";

            xoGastoCtrl.EliminarGasto(xiId, out xsError);

            return Json(xsError);
        }
    }
}