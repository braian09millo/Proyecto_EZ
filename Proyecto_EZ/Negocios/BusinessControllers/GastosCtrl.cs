using Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;

namespace Negocios.BusinessControllers
{
    public class GastosCtrl
    {
        #region SINGLETON

        private static GastosCtrl _GastoCtrl;

        public static GastosCtrl GetInstancia()
        {
            if (_GastoCtrl == null)
                _GastoCtrl = new GastosCtrl();

            return _GastoCtrl;
        }

        #endregion


        public List<gasto> ObtenerGastos(DateTime xdFechaDesde, DateTime xdFechaHasta, out string xsError)
        {
            xsError = "";
            List<gasto> loResultado = null;

            using (BD_Entities xoDB = new BD_Entities())
            {
                try
                {
                    loResultado = xoDB.gasto.Where(x => x.gas_fecha >= xdFechaDesde && x.gas_fecha <= xdFechaHasta)
                                             .ToList();
                }
                catch (Exception ex)
                {
                    xsError = ex.Message;
                }
            }

            return loResultado;
        }

        public void GuardarGasto(GastoForm xoGasto, out string xsError)
        {
            xsError = "";

            using (BD_Entities xoDB = new BD_Entities())
            {
                try
                {
                    var loGasto = xoDB.gasto.Find(xoGasto.Id);

                    if (loGasto != null)
                    {
                        loGasto.gas_descripcion = xoGasto.Descripcion;
                        loGasto.gas_fecha = xoGasto.Fecha;
                        loGasto.gas_monto = xoGasto.Monto;
                    }
                    else
                    {
                        var xoNuevoGasto = new gasto()
                        {
                            gas_fecha = xoGasto.Fecha,
                            gas_descripcion = xoGasto.Descripcion,
                            gas_monto = xoGasto.Monto
                        };

                        xoDB.gasto.Add(xoNuevoGasto);
                    }

                    xoDB.SaveChanges();
                }
                catch (Exception ex)
                {
                    xsError = ex.Message;
                }
            }
        }
    }
}
