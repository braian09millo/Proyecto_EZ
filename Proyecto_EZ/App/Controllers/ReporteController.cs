using App.Models;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Controllers
{
    public class ReporteController : Controller
    {
        [HttpGet]
        public FileResult GetInformeRemito(int xiPedido)
        {
            var _nombre = "RptRecaudacion.rdlc";
            var _nombreDs = "RecaudacionDS";
            var _path = HttpContext.Server.MapPath("~/Reportes/" + _nombre);
            var _lista = new List<spGetPedidos>();

            Reporting.GenerarInforme(_lista, _path, _nombre, _nombreDs, "XLS");

            return File(_path, "application/pdf");
        }
    }
}