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

        public JsonResult PostGuardarUsuario(UsuarioForm xoUsuario)
        {
            string xsError = "";

            xoUsuarioCtrl.GuardarUsuario(xoUsuario, out xsError);

            return Json(xsError);
        }
    }
}