using Datos;
using Entidades;
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

        public void GuardarCliente(cliente xoCliente, out string xsError)
        {
            xsError = "";

            try
            {
                using (BD_Entities xoDB = new BD_Entities())
                {
                    var cliente = xoDB.cliente.Find(xoCliente.cli_id);

                    if (cliente != null)
                        xoDB.Entry(cliente).CurrentValues.SetValues(xoCliente);
                    else
                        xoDB.cliente.Add(xoCliente);

                    xoDB.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                xsError = ex.Message;
            }
        }

        public List<spGetClientes> ObtenerClientes(out string xsError)
        {
            xsError = "";
            List<spGetClientes> xoResultado = null;

            try
            {
                using (BD_Entities xoDB = new BD_Entities())
                {
                    xoResultado = xoDB.Database.SqlQuery<spGetClientes>("exec spGetClientes").ToList();
                }
            }
            catch (Exception ex)
            {
                xsError = ex.Message;
            }

            return xoResultado;
        }

        public void EliminarCliente(int xiId, out string xsError)
        {
            xsError = "";

            try
            {
                using (BD_Entities xoDB = new BD_Entities())
                {
                    var xoCliente = xoDB.cliente.Find(xiId);

                    if (xoCliente != null)
                    {
                        xoDB.cliente.Remove(xoCliente);
                        xoDB.SaveChanges();
                    }
                    else
                        xsError = "El usuario no existe";
                }
            }
            catch (Exception ex)
            {
                xsError = ex.Message;
            }
        }

        #endregion
    }
}
