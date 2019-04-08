using Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocios.BusinessControllers
{
    public class ClienteCtrl
    {
        #region SINGLETON

        private static ClienteCtrl _ClienteCtrl;

        public static ClienteCtrl GetInstancia()
        {
            if (_ClienteCtrl == null)
                _ClienteCtrl = new ClienteCtrl();

            return _ClienteCtrl;
        }

        #endregion

        #region COMPORTAMIENTO

        public void GuardarCliente(CLIENTE xoCliente, out string xsError)
        {
            xsError = "";

            try
            {
                using (BD_Entities xoDB = new BD_Entities())
                {
                    var cliente = xoDB.CLIENTE.Find(xoCliente.cli_id);

                    if (cliente != null)
                        xoDB.Entry(cliente).CurrentValues.SetValues(xoCliente);
                    else
                        xoDB.CLIENTE.Add(xoCliente);

                    xoDB.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                xsError = ex.Message;
            }
        }

        public List<CLIENTE> ObtenerClientes(out string xsError)
        {
            xsError = "";
            List<CLIENTE> xoResultado = null;

            try
            {
                using (BD_Entities xoDB = new BD_Entities())
                {
                    xoResultado = xoDB.CLIENTE.ToList();
                }
            }
            catch (Exception ex)
            {
                xsError = ex.Message;
            }

            return xoResultado;
        }

        #endregion
    }
}
