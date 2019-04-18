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
    public class ProductoController : Controller
    {
        ProductoCtrl xoProductoCtrl = new Factory().GetCtrlProducto();

        public ActionResult Index()
        {
            string xsError = "";

            //Cargamos el combo de marcas
            var xoMarcas = xoProductoCtrl.ObtenerMarcas(out xsError);
            var lstMarcas = xoMarcas.Select(x => new SelectListItem() { Value = x.mar_id.ToString(), Text = x.mar_nombre }).ToList();
            ViewBag.ComboMarcas = lstMarcas;

            //Cargamos el combo de tamanios
            var xoTamanios = xoProductoCtrl.ObtenerTamanios(out xsError);
            var lstTamanios = xoTamanios.Select(x => new SelectListItem() { Value = x.tam_id.ToString(), Text = x.tam_descripcion }).ToList();
            ViewBag.ComboTamanios = lstTamanios;

            //Cargamos el combo de tipos
            var xoTipos = xoProductoCtrl.ObtenerTipos(out xsError);
            var lstTipos = xoTipos.Select(x => new SelectListItem() { Value = x.tip_id.ToString(), Text = x.tip_descr }).ToList();
            ViewBag.ComboTipos = lstTipos;

            //Obtenemos los modelos
            ViewBag.ComboModelos = xoProductoCtrl.ObtenerModelos(out xsError);

            return View();
        }

        [HttpPost]
        public JsonResult PostGuardarProducto(ProductoForm xoProducto)
        {
            string xsError = "";

            xoProductoCtrl.GuardarProducto(xoProducto, out xsError);

            return Json(xsError);
        }
    }
}