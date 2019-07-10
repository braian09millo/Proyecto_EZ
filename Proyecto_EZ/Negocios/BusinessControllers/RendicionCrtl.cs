using Datos;
using Entidades;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Negocios.BusinessControllers
{
    public class RendicionCtrl
    {
        #region SINGLETON

        private static RendicionCtrl _RendicionCtrl;

        public static RendicionCtrl GetInstancia()
        {
            if (_RendicionCtrl == null)
                _RendicionCtrl = new RendicionCtrl();

            return _RendicionCtrl;
        }

        #endregion


        //public List<rendicion> ObtenerRendicion(DateTime xdFechaDesde, DateTime xdFechaHasta, out string xsError)
        //{
        //    xsError = "";
        //    List<rendicion> loResultado = null;

        //    using (BD_Entities xoDB = new BD_Entities())
        //    {
        //        try
        //        {
        //            loResultado = xoDB.rendicion.Where(x => x.ren_desde >= xdFechaDesde && x.ren_hasta <= xdFechaHasta)
        //                                     .ToList();
        //        }
        //        catch (Exception ex)
        //        {
        //            xsError = ex.Message;
        //        }
        //    }

        //    return loResultado;
        //}

        public void GuardarRendicion(RendicionForm xoRendicion, out string xsError)
        {
            xsError = "";

            using (BD_Entities xoDB = new BD_Entities())
            {
                var rendicion = xoDB.rendicion.Find(xoRendicion.id);

                if (rendicion != null)
                {
                    rendicion.ren_desde = xoRendicion.desde;
                    rendicion.ren_hasta = xoRendicion.hasta;

                    xoDB.SaveChanges();
                }
                else
                {
                    using (var xoTransaccion = xoDB.Database.BeginTransaction())
                    {
                        try
                        {
                            var retVal = new SqlParameter("@RetVal", SqlDbType.Int);
                            retVal.Direction = ParameterDirection.ReturnValue;

                            xoDB.Database.ExecuteSqlCommand("exec spAgregarRendicion @Desde, @Hasta",
                                       new SqlParameter("@Desde", xoRendicion.desde),
                                       new SqlParameter("@Hasta", xoRendicion.hasta),
                                       retVal);

                            if ((int)retVal.Value != 0)
                            {
                                xsError = "Error al agregar la rendición";
                                xoTransaccion.Rollback();
                            }
                            else
                                xoTransaccion.Commit();

                        }
                        catch (Exception ex)
                        {
                            xsError = ex.Message;
                            xoTransaccion.Rollback();
                        }
                    }
                }
            }
        }
           public object ObtenerRendicion(DateTime? xdFechaDesde, DateTime? xdFechaHasta, out string xsError)
        {
            xsError = "";
            object xoRendicion = null;

            using (BD_Entities xoDB = new BD_Entities())
            {
                try
                {
                    var xoResultado = xoDB.Database.SqlQuery<spGetRendicion>("exec spGetRendicion @FechaDesde, @FechaHasta",
                                               new SqlParameter("@FechaDesde", xdFechaDesde),
                                               new SqlParameter("@FechaHasta", xdFechaHasta)).ToList();

                    foreach (var item in xoResultado)
                    {
                        item.RendicionDetalle = ObtenerDetalleRendicion(item.IdRendicion, out xsError);
                    }

                    xoRendicion = xoResultado.Select(x => new[] {
                        x.IdRendicion.ToString(),
                        x.Desde.ToString("dd-MM-yyyy"),
                        x.Hasta.ToString("dd-MM-yyyy"),
                        "$ " + string.Format("{0:0.##}", x.Costo),
                        JsonConvert.SerializeObject(x.RendicionDetalle),
                        JsonConvert.SerializeObject(x)}).ToList();
                }
                catch (Exception ex)
                {
                    xsError = ex.Message;
                }
            }

            return xoRendicion;
        }
        public List<spGetRendicionDetalle> ObtenerDetalleRendicion(int xiRendicion, out string xsError)
        {
            xsError = "";
            var lstDetalle = new List<spGetRendicionDetalle>();

            using (BD_Entities xoDB = new BD_Entities())
            {
                try
                {
                    lstDetalle = xoDB.Database.SqlQuery<spGetRendicionDetalle>("exec spGetRendicionDetalle @Rendicion",
                                              new SqlParameter("@Rendicion", xiRendicion)).ToList();
                }
                catch (Exception ex)
                {
                    xsError = ex.Message;
                }
            }

            return lstDetalle;
        }
        public void EliminarRendicion(int xiId, out string xsError)
        {
            xsError = "";

            using (BD_Entities xoDB = new BD_Entities())
            {
                try
                {
                    var loRendicion = xoDB.rendicion.Find(xiId);

                    if (loRendicion != null)
                        xoDB.rendicion.Remove(loRendicion);
                    else
                        xsError = "La Rendicion seleccionado no existe";

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
