using Datos;
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

        #endregion

        #region COMPORTAMIENTO

        public List<usuario> ObtenerUsuarios(out string xsError)
        {
            xsError = "";
            List<usuario> xoResultado = null;

            using (BD_Entities xoDB = new BD_Entities())
            {
                try
                {
                    xoResultado = xoDB.usuario.ToList();
                }
                catch (Exception ex)
                {
                    xsError = ex.Message;
                }
            }

            return xoResultado;
        }

        #endregion
    }
}
