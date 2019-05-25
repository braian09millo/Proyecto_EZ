using Entidades;
using Negocios;
using Negocios.BusinessControllers;
using Negocios.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult VerificarSesion(LoginModel xoLogin)
        {
            //Declaramos las variables
            string xsError = "";
            bool xbUsuarioValido = default(bool);

            try
            {
                //Obtenemos la instancia del controlador
                UsuarioCtrl xoCtrlUsuario = new Factory().GetCtrlUsuario();

                //Obtenemos el usuario en cuestion
                var oUsuario = xoCtrlUsuario.ValidarIdentidadUsuario(xoLogin.Usuario, out xbUsuarioValido, out xsError);

                if (xbUsuarioValido)
                {
                    string sPassEncriptada = Encriptacion.EncriptarPassword(xoLogin.Usuario.ToUpper() + xoLogin.Password.ToUpper());
                    if (sPassEncriptada != oUsuario.usu_password)
                    {
                        xbUsuarioValido = false;
                        xsError = "Password incorrecta";
                    }
                }
            }
            catch (Exception ex)
            {
                xsError = ex.Message;
            }

            return Json(xsError);
        }
    }
}