using App.Models;
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
    public class ReporteController : Controller
    {
        PedidoCtrl xoCtrlPedido = new Factory().GetCtrlPedido();

        [HttpGet]
        public FileResult GetRemito(int xiPedido)
        {
            var _nombre = "rptRemito.rdlc";
            var _nombreDs = "RemitoDS";
            var _path = HttpContext.Server.MapPath("~/Reportes/" + _nombre);
            var _lista = xoCtrlPedido.ObtenerPedidosRpt(xiPedido);

            var bytes = Reporting.GenerarInforme(_lista, _path, _nombre, _nombreDs, "PDF");

            return File(bytes, "application/pdf", "Remito_N°" + xiPedido.ToString() + ".pdf");
        }

        [HttpGet]
        public FileResult GetFacturacionMensual(DateTime xdFechaDesde, DateTime xdFechaHasta, string xsRepartidor)
        {
            var _nombre = "rptFacturacionMensual.rdlc";
            var _nombreDs = "FactMensualDS";
            var _path = HttpContext.Server.MapPath("~/Reportes/" + _nombre);
            var _lista = xoCtrlPedido.ObtenerFacturacionMensualRpt(xdFechaDesde, xdFechaHasta, xsRepartidor);

            var bytes = Reporting.GenerarInforme(_lista, _path, _nombre, _nombreDs, "PDF");

            return File(bytes, "application/pdf", "FacturacionMensual_" + xdFechaDesde.ToString("ddMMyyyy") + "_" + xdFechaHasta.ToString("ddMMyyyy") + ".pdf");
        }
    }
}