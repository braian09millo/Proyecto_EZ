using Datos;
using Negocios;
using Negocios.BusinessControllers;
using System.Web.Mvc;

namespace App.Controllers
{
    public class ClienteController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetClientes()
        {
            ClienteCtrl xoClienteCtrl = new Factory().GetCtrlCliente();
            string xsError = "";

            var xoResultado = xoClienteCtrl.ObtenerClientes(out xsError);

            return Json(xoResultado, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult PostGuardarCliente(cliente xoCliente)
        {
            ClienteCtrl xoClienteCtrl = new Factory().GetCtrlCliente();
            string xsError = "";

            xoClienteCtrl.GuardarCliente(xoCliente, out xsError);

            return Json(xsError);
        }

        [HttpPost]
        public ActionResult PostEliminarCliente(int xiId)
        {
            ClienteCtrl xoClienteCtrl = new Factory().GetCtrlCliente();
            string xsError = "";

            xoClienteCtrl.EliminarCliente(xiId, out xsError);

            return Json(xsError);
        }
    }
}