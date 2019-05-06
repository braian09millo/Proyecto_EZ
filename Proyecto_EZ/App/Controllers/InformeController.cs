using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Controllers
{
    public class InformeController : Controller
    {
        // GET: Informe
        public ActionResult FacturacionCliente()
        {
            return View();
        }
    }
}