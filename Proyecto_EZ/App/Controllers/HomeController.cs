using Negocios;
using Negocios.BusinessControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Controllers
{
    public class HomeController : Controller
    {
        HomeCtrl xoCtrlHome = new Factory().GetCtrlHome();

        public ActionResult Index()
        {
            if (Session["Usuario"] == null)
                return RedirectToAction("Index", "Login");

            var xoListaMayoresVentas = xoCtrlHome.ObtenerClientesConMayoresVentas();
            var xoListaClientesMorosos = xoCtrlHome.ObtenerClientesMorosos();
            var xoListaProductosVendidos = xoCtrlHome.ObtenerProductosMasVendidos();

            ViewBag.ClientesMayoresVenta = xoListaMayoresVentas;
            ViewBag.ClientesMorosos = xoListaClientesMorosos;
            ViewBag.ProductosMasVendidos = xoListaProductosVendidos;

            return View();
        }
    }
}