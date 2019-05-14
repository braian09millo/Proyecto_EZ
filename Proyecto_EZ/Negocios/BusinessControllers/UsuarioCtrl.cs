using Datos;
using Entidades;
using Negocios.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocios.BusinessControllers
{
    public class UsuarioCtrl
    {
        #region SINGLETON

        private static UsuarioCtrl _UsuarioCtrl;

        public static UsuarioCtrl GetInstancia()
        {
            if (_UsuarioCtrl == null)
                _UsuarioCtrl = new UsuarioCtrl();

            return _UsuarioCtrl;
        }

        public List<grupo> obtenerGrupos(out string xsError)
        {
            xsError = "";
            var xoResultado = new List<grupo>();

            using (BD_Entities xoDB = new BD_Entities())
            {
                xoResultado = xoDB.grupo.ToList();
            }

            return xoResultado;
        }

        #endregion

        #region COMPORTAMIENTO

        public List<spGetUsuarios> ObtenerUsuarios(out string xsError)
        {
            xsError = "";
            List<spGetUsuarios> xoResultado = null;

            using (BD_Entities xoDB = new BD_Entities())
            {
                try
                {
                    xoResultado = xoDB.Database.SqlQuery<spGetUsuarios>("exec spGetUsuarios").ToList();
                }
                catch (Exception ex)
                {
                    xsError = ex.Message;
                }
            }

            return xoResultado;
        }

        public void EliminarUsuario(string xsUsuario, out string xsError)
        {
            xsError = "";

            using (BD_Entities xoDB = new BD_Entities())
            {
                try
                {
                    var xoUsuario = xoDB.usuario.Find(xsUsuario);

                    if (xoUsuario != null)
                    {
                        xoUsuario.usu_delet = "S";
                        xoDB.SaveChanges();
                    }
                    else
                        xsError = "El usuario seleccionado no existe";
                }
                catch (Exception ex)
                {
                    xsError = ex.Message;
                }
            }
        }

        public void CambiarPassword(string xsUsuario, string xsPassAntigua, string xsPassNueva, out string xsError)
        {
            xsError = "";

            using (BD_Entities xoDB = new BD_Entities())
            {
                try
                {
                    var xoUsuario = xoDB.usuario.Find(xsUsuario);

                    if (xoUsuario != null)
                    {
                        if (Encriptacion.EncriptarPassword(xsUsuario.ToUpper() + xsPassAntigua.ToUpper()).Equals(xoUsuario.usu_password))
                        {
                            xoUsuario.usu_password = Encriptacion.EncriptarPassword(xoUsuario.usu_usuario.ToUpper() + xsPassNueva.ToUpper());
                            xoDB.SaveChanges();
                        }
                        else
                            xsError = "Las contraseña ingresada no coincide con la antigua";
                    }
                    else
                        xsError = "El usuario no existe";
                }
                catch (Exception ex)
                {
                    xsError = ex.Message;
                }
            }
        }

        public void HabilitarUsuario(string xsUsuario, out string xsError)
        {
            xsError = "";

            using (BD_Entities xoDB = new BD_Entities())
            {
                try
                {
                    var xoUsuario = xoDB.usuario.Find(xsUsuario);

                    if (xoUsuario != null)
                    {
                        xoUsuario.usu_delet = "N";
                        xoDB.SaveChanges();
                    }
                    else
                        xsError = "El usuario seleccionado no existe";
                }
                catch (Exception ex)
                {
                    xsError = ex.Message;
                }
            }
        }

        public void GuardarUsuario(UsuarioForm xoUsuario, out string xsError)
        {
            xsError = "";

            using (BD_Entities xoDB = new BD_Entities())
            {
                try
                {
                    var loUsuario = xoDB.usuario.Find(xoUsuario.Usuario);

                    if (loUsuario != null)
                    {
                        loUsuario.usu_nombre = xoUsuario.Nombre;
                        loUsuario.usu_apellido = xoUsuario.Apellido;
                        loUsuario.usu_grupo = (byte)xoUsuario.Grupo;
                    }
                    else
                    {
                        var xoNuevoUsuario = new usuario()
                        {
                            usu_nombre = xoUsuario.Nombre,
                            usu_apellido = xoUsuario.Apellido,
                            usu_grupo = (byte)xoUsuario.Grupo,
                            usu_usuario = xoUsuario.Usuario,
                            usu_password = Encriptacion.EncriptarPassword(xoUsuario.Usuario.ToUpper() + xoUsuario.Password.ToUpper())                                                        
                        };

                        xoDB.usuario.Add(xoNuevoUsuario);
                    }

                    xoDB.SaveChanges();
                }
                catch (Exception ex)
                {
                    xsError = ex.Message;
                }
            }
        }



        //public usuario ValidarIdentidadUsuario(string sId, out bool bUsuarioValido, out string sError)
        //{
        //    //Definicion de variables
        //    bUsuarioValido = default(bool);
        //    sError = default(string);
        //    Usuario oUsuario = null;

        //    try
        //    {
        //        using (GestionEntities oEntidad = new GestionEntities())
        //        {
        //            var _UsuarioEncontrado = oEntidad.users.FirstOrDefault(u => u.use_id == sId);
        //            if (_UsuarioEncontrado != null)
        //            {
        //                oUsuario = CargarDatosUsuario(_UsuarioEncontrado);
        //                bUsuarioValido = true;
        //            }
        //            else
        //                sError = "Usuario inválido";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        sError = ex.Message;
        //    }

        //    return oUsuario;
        //}

        #endregion
    }
}
