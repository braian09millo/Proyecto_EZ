using Datos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocios.BusinessControllers
{
    public class ProductoCtrl
    {
        #region SINGLETON

        private static ProductoCtrl _ProductoCtrl;

        public static ProductoCtrl GetInstancia()
        {
            if (_ProductoCtrl == null)
                _ProductoCtrl = new ProductoCtrl();

            return _ProductoCtrl;
        }

        #endregion

        #region COMPORTAMIENTO

        public List<marca> ObtenerMarcas(out string xsError)
        {
            xsError = "";
            List<marca> loMarcas = null;

            try
            {
                using (BD_Entities xoDB = new BD_Entities())
                {
                    loMarcas = xoDB.marca.ToList();
                }
            }
            catch (Exception ex)
            {
                xsError = ex.Message;
            }

            return loMarcas;
        }

        public List<modelo> ObtenerModelos(out string xsError)
        {
            xsError = "";
            List<modelo> loModelos = null;

            try
            {
                using (BD_Entities xoDB = new BD_Entities())
                {
                    loModelos = xoDB.modelo.ToList();
                }
            }
            catch (Exception ex)
            {
                xsError = ex.Message;
            }

            return loModelos;
        }

        public List<tamanio> ObtenerTamanios(out string xsError)
        {
            xsError = "";
            List<tamanio> loTamanios = null;

            try
            {
                using (BD_Entities xoDB = new BD_Entities())
                {
                    loTamanios = xoDB.tamanio.ToList();
                }
            }
            catch (Exception ex)
            {
                xsError = ex.Message;
            }

            return loTamanios;
        }

        public List<tipo> ObtenerTipos(out string xsError)
        {
            xsError = "";
            List<tipo> loTipos = null;

            try
            {
                using (BD_Entities xoDB = new BD_Entities())
                {
                    loTipos = xoDB.tipo.ToList();
                }
            }
            catch (Exception ex)
            {
                xsError = ex.Message;
            }

            return loTipos;
        }

        public void GuardarProducto(ProductoForm xoProducto, out string xsError)
        {
            xsError = "";

            try
            {
                using (BD_Entities xoDB = new BD_Entities())
                {
                    using (var xoTransaccion = xoDB.Database.BeginTransaction())
                    {
                        var producto = xoDB.producto.Find(xoProducto.Id);
                        var retValue = 0;

                        if (producto != null)
                        {
                            retValue = xoDB.Database.ExecuteSqlCommand("exec spActualizarProducto @idProducto, @Marca, @Modelo, @Tamanio, @Tipo, @Cantidad, @Costo, @Porcentaje, @PrecioVenta",
                                           new SqlParameter("@idProducto", xoProducto.Id),
                                           new SqlParameter("@Marca", xoProducto.Marca),
                                           new SqlParameter("@Modelo", xoProducto.Modelo),
                                           new SqlParameter("@Tamanio", xoProducto.Tamanio),
                                           new SqlParameter("@Tipo", xoProducto.Tipo),
                                           new SqlParameter("@Cantidad", xoProducto.CantidadPack),
                                           new SqlParameter("@Costo", xoProducto.Costo),
                                           new SqlParameter("@Porcentaje", xoProducto.Porcentaje),
                                           new SqlParameter("@PrecioVenta", xoProducto.PrecioVenta));

                            if (retValue != 0)
                            {
                                xsError = "Error al modificar el producto";
                                xoTransaccion.Rollback();
                            }
                            else
                                xoTransaccion.Commit();

                        }
                        else
                        {
                            var retVal = new SqlParameter("@RetVal", SqlDbType.Int);
                            retVal.Direction = ParameterDirection.ReturnValue;

                            xoDB.Database.ExecuteSqlCommand("exec spAgregarProducto @Marca, @Modelo, @Tamanio, @Tipo, @Cantidad, @Costo, @Porcentaje, @PrecioVenta",
                                       new SqlParameter("@Marca", xoProducto.Marca),
                                       new SqlParameter("@Modelo", xoProducto.Modelo),
                                       new SqlParameter("@Tamanio", xoProducto.Tamanio),
                                       new SqlParameter("@Tipo", xoProducto.Tipo),
                                       new SqlParameter("@Cantidad", xoProducto.CantidadPack),
                                       new SqlParameter("@Costo", xoProducto.Costo),
                                       new SqlParameter("@Porcentaje", xoProducto.Porcentaje),
                                       new SqlParameter("@PrecioVenta", xoProducto.PrecioVenta),
                                       retVal);

                            if ((int)retVal.Value != 0)
                            {
                                xsError = "Error al agregar el producto";
                                xoTransaccion.Rollback();
                            }
                            else
                                xoTransaccion.Commit();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                xsError = ex.Message;
            }
        }

        public List<spGetProductos> ObtenerProductos(out string xsError)
        {
            xsError = "";
            List<spGetProductos> loResultado = null;

            using (BD_Entities xoDB = new BD_Entities())
            {
                try
                {
                    loResultado = xoDB.Database.SqlQuery<spGetProductos>("exec spGetProductos").ToList();
                }
                catch (Exception ex)
                {
                    xsError = ex.Message;
                }
            }

            return loResultado;
        }

        public void EliminarProducto(int xiId, out string xsError)
        {
            xsError = "";

            using (BD_Entities xoDB = new BD_Entities())
            {
                try
                {
                    var xoProducto = xoDB.producto.Find(xiId);
                    if (xoProducto != null)
                    {
                        xoProducto.prod_delet = "S";
                        xoDB.SaveChanges();
                    }
                    else
                    {
                        xsError = "El producto no existe";
                    }
                }
                catch (Exception ex)
                {
                    xsError = ex.Message;
                }
            }
        }

        #endregion

    }
}
