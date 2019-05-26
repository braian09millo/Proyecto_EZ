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
    public class UsuarioController : Controller
    {
        UsuarioCtrl xoUsuarioCtrl = new Factory().GetCtrlUsuario();

        // GET: Usuario
        public ActionResult Index()
        {
            if (Session["Usuario"] == null)
                return RedirectToAction("Index", "Login");

            string xsError = "";

            //Cargamos el combo de grupos
            var lstGrupos = xoUsuarioCtrl.obtenerGrupos(out xsError);
            ViewBag.ComboGrupos = lstGrupos.Select(x => new SelectListItem { Value = x.gru_id.ToString(), Text = x.gru_descr });

            return View();
        }

        public JsonResult GetUsuarios()
        {
            string xsError = "";

            var lstUsuarios = xoUsuarioCtrl.ObtenerUsuarios(out xsError);

            var resultadoJS = new
            {
                data = lstUsuarios,
                error = xsError
            };

            return Json(resultadoJS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PostHabilitarUsuario(string xsUsuario)
        {
            string xsError = "";
            xoUsuarioCtrl.HabilitarUsuario(xsUsuario, out xsError);
            return Json(xsError);
        }

        public JsonResult PostEliminarUsuario(string xsUsuario)
        {
            string xsError = "";
            xoUsuarioCtrl.EliminarUsuario(xsUsuario, out xsError);
            return Json(xsError);
        }

        public JsonResult PostCambiarPassword(string xsUsuario, string xsPassAntigua, string xsPassNueva)
        {
            string xsError = "";

            if (string.IsNullOrEmpty(xsPassAntigua) || string.IsNullOrEmpty(xsPassNueva))
                xsError = "Ninguno de los campos puede ser nulo o vacío";
            else
                xoUsuarioCtrl.CambiarPassword(xsUsuario, xsPassAntigua, xsPassNueva, out xsError);

            return Json(xsError);
        }

        public JsonResult PostGuardarUsuario(UsuarioForm xoUsuario)
        {
            string xsError = "";

            xoUsuarioCtrl.GuardarUsuario(xoUsuario, out xsError);

            return Json(xsError);
        }
    }
}