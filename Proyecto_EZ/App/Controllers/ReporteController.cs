using App.Models;
using Entidades;
using Microsoft.Reporting.WebForms;
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
        ProductoCtrl xoCtrlProducto = new Factory().GetCtrlProducto();
        GastosCtrl xoCtrlGasto = new Factory().GetCtrlGasto();

        [HttpGet]
        public FileResult GetRemito(int xiPedido)
        {
            var _nombre = "rptRemito.rdlc";
            var _nombreDs = "RemitoDS";
            var _path = HttpContext.Server.MapPath("~/Reportes/" + _nombre);
            var _lista = xoCtrlPedido.ObtenerPedidosRpt(xiPedido);

            var bytes = Reporting.GenerarInforme(_lista, _path, _nombre, _nombreDs, "PDF", null);

            return File(bytes, "application/pdf", "Remito_N°" + xiPedido.ToString() + ".pdf");
        }

        [HttpGet]
        public FileResult GetFacturacionMensual(DateTime xdFechaDesde, DateTime xdFechaHasta, string xsRepartidor)
        {
            string xsError = "";
            var _nombre = "rptFacturacionMensual.rdlc";
            var _nombresDs = new List<string>() { "FactMensualDS", "GastoMensualDS", "FactResultadoDS" };
            var _path = HttpContext.Server.MapPath("~/Reportes/" + _nombre);
            var listas = new List<object>();
            listas.Add(xoCtrlPedido.ObtenerFacturacionMensualRpt(xdFechaDesde, xdFechaHasta, xsRepartidor));
            listas.Add(xoCtrlGasto.ObtenerGastos(xdFechaDesde, xdFechaHasta, out xsError));
            listas.Add(xoCtrlPedido.ObtenerFacturacionResultadoRpt(xdFechaDesde, xdFechaHasta, xsRepartidor));

            var parFechaDesde = new ReportParameter("FechaDesde", xdFechaDesde.ToString("dd-MM-yy"));
            var parFechaHasta = new ReportParameter("FechaHasta", xdFechaHasta.ToString("dd-MM-yy"));
            var parRepartidor = new ReportParameter("Repartidor", xsRepartidor);
            var listaParametros = new List<ReportParameter>() { parFechaDesde, parFechaHasta, parRepartidor };

            var bytes = Reporting.GenerarInforme(listas, _path, _nombre, _nombresDs, "PDF", listaParametros);
            return File(bytes, "application/pdf", "FacturacionMensual_" + xdFechaDesde.ToString("ddMMyyyy") + "_" + xdFechaHasta.ToString("ddMMyyyy") + ".pdf");
        }

        [HttpGet]
        public FileResult GetRendicion(DateTime xdFechaDesde, DateTime xdFechaHasta)
        {
            var _nombre = "rptRendicion.rdlc";
            var _nombreDs = "RendicionDS";
            var _path = HttpContext.Server.MapPath("~/Reportes/" + _nombre);
            var lista = xoCtrlPedido.ObtenerRendicionRpt(xdFechaDesde, xdFechaHasta);

            var parFechaDesde = new ReportParameter("FechaDesde", xdFechaDesde.ToString("dd-MM-yy"));
            var parFechaHasta = new ReportParameter("FechaHasta", xdFechaHasta.ToString("dd-MM-yy"));
            var listaParametros = new List<ReportParameter>() { parFechaDesde, parFechaHasta };

            var bytes = Reporting.GenerarInforme(lista, _path, _nombre, _nombreDs, "PDF", listaParametros);
            return File(bytes, "application/pdf", "RendicionFacu_" + xdFechaDesde.ToString("ddMMyyyy") + "_" + xdFechaHasta.ToString("ddMMyyyy") + ".pdf");
        }

        [HttpGet]
        public FileResult GetRemitoRepartidor(DateTime xdFecha, string xsRepartidor, int? xiVuelta)
        {
            var _nombre = "rptRemitoRepartidor.rdlc";
            var _nombreDs = "RemitoRepartidorDS";
            var _path = HttpContext.Server.MapPath("~/Reportes/" + _nombre);
            var lista = xoCtrlPedido.ObtenerRemitoRepartidorRpt(xdFecha, xsRepartidor, xiVuelta);      

            var bytes = Reporting.GenerarInforme(lista, _path, _nombre, _nombreDs, "PDF", null);
            return File(bytes, "application/pdf", "Remito_"+ xsRepartidor + "_" + xdFecha.ToString("ddMMyyyy") + ".pdf");
        }

        [HttpGet]
        public FileResult GetListaPrecios()
        {
            string xsError = "";
            var _nombre = "rptListaPrecios.rdlc";
            var _nombreDs = "PreciosDS";
            var _path = HttpContext.Server.MapPath("~/Reportes/" + _nombre);
            var lista = xoCtrlProducto.ObtenerListaPrecios(out xsError);

            var bytes = Reporting.GenerarInforme(lista, _path, _nombre, _nombreDs, "PDF", null);
            return File(bytes, "application/pdf", "ListaPrecios_" + DateTime.Today.ToString("ddMMyyyy") + ".pdf");
        }
    }
}