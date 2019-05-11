using Datos;
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

        #region PRODUCTOS
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

        public JsonResult GetProductos()
        {
            string xsError = "";
            var lstProductos = xoProductoCtrl.ObtenerProductos(out xsError);
            var xoResultado = new
            {
                data = lstProductos,
                error = xsError
            };

            return Json(xoResultado, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult PostEliminarProducto(int xiId)
        {
            string xsError = "";
            xoProductoCtrl.EliminarProducto(xiId, out xsError);
            return Json(xsError);
        }

        [HttpPost]
        public JsonResult PostHabilitarProducto(int xiId)
        {
            string xsError = "";
            xoProductoCtrl.HabilitarProducto(xiId, out xsError);
            return Json(xsError);
        }

        [HttpPost]
        public JsonResult PostGuardarProducto(ProductoForm xoProducto)
        {
            string xsError = "";
            xoProductoCtrl.GuardarProducto(xoProducto, out xsError);
            return Json(xsError);
        }

        public ActionResult EditorPrecios()
        {
            string xsError = "";
            ViewBag.Productos = xoProductoCtrl.ObtenerProductosEdicion(out xsError);

            return View();
        }

        [HttpPost]
        public JsonResult PostActualizarPrecios(List<PrecioEdicion> xoPrecios)
        {
            string xsError = "";
            xoProductoCtrl.ActualizarPrecios(xoPrecios, out xsError);
            return Json(xsError);
        }
        #endregion

        #region MARCAS
        public ActionResult Marca()
        {
            return View();
        }

        public JsonResult GetMarcas()
        {
            string xsError = "";
            var lstMarcas = xoProductoCtrl.ObtenerMarcas(out xsError)
                                          .Select(x => new marca { mar_id = x.mar_id, mar_nombre = x.mar_nombre, mar_delet = x.mar_delet ?? "N" })
                                          .ToList();

            var resultadoJS = new
            {
                data = lstMarcas,
                error = xsError
            };

            return Json(resultadoJS, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult PostEliminarMarca(int xiId)
        {
            string xsError = "";
            xoProductoCtrl.EliminarMarca(xiId, out xsError);
            return Json(xsError);
        }

        [HttpPost]
        public JsonResult PostHabilitarMarca(int xiId)
        {
            string xsError = "";
            xoProductoCtrl.HabilitarMarca(xiId, out xsError);
            return Json(xsError);
        }

        [HttpPost]
        public JsonResult PostGuardarMarca(MarcaForm xoMarca)
        {
            string xsError = "";
            xoProductoCtrl.GuardarMarca(xoMarca, out xsError);
            return Json(xsError);
        }
        #endregion

        #region MODELOS

        public ActionResult Modelo()
        {
            string xsError = "";

            //Cargamos el combo de marcas
            var xoMarcas = xoProductoCtrl.ObtenerMarcas(out xsError);
            var lstMarcas = xoMarcas.Select(x => new SelectListItem() { Value = x.mar_id.ToString(), Text = x.mar_nombre }).ToList();
            ViewBag.ComboMarcas = lstMarcas;

            return View();
        }

        public string obtenerMarcaPorModelo(int xiId)
        {
            string xsError = "", xsResultado = "";
            var loMarcas = xoProductoCtrl.ObtenerMarcas(out xsError);
            if (xsError == "")
                xsResultado = loMarcas.FirstOrDefault(x => x.mar_id == xiId).mar_nombre;

            return xsResultado;
        }

        public JsonResult GetModelos()
        {
            string xsError = "";
            var lstModelos = xoProductoCtrl.ObtenerModelos(out xsError)                                          
                                          .Select(x => new { mod_id = x.mod_id, mod_nombre = x.mod_nombre, mod_delet = x.mod_delet ?? "N", mod_marca = x.mod_marca, mod_marcaDesc = obtenerMarcaPorModelo(x.mod_marca)})
                                          .ToList();

            var resultadoJS = new
            {
                data = lstModelos,
                error = xsError
            };

            return Json(resultadoJS, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult PostEliminarModelo(int xiId)
        {
            string xsError = "";
            xoProductoCtrl.EliminarModelo(xiId, out xsError);
            return Json(xsError);
        }

        [HttpPost]
        public JsonResult PostHabilitarModelo(int xiId)
        {
            string xsError = "";
            xoProductoCtrl.HabilitarModelo(xiId, out xsError);
            return Json(xsError);
        }

        [HttpPost]
        public JsonResult PostGuardarModelo(ModeloForm xoModelo)
        {
            string xsError = "";
            xoProductoCtrl.GuardarModelo(xoModelo, out xsError);
            return Json(xsError);
        }

        #endregion
    }
}