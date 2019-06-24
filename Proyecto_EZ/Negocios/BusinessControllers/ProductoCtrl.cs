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

        #region PRODUCTOS

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
                    loModelos = xoDB.modelo.GroupBy(x => x.mod_nombre).Select(x => x.FirstOrDefault()).ToList();
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

            using (BD_Entities xoDB = new BD_Entities())
            {
                var producto = xoDB.producto.Find(xoProducto.Id);

                if (producto != null)
                {
                    producto.prod_marca = xoProducto.Marca;
                    producto.prod_modelo = xoProducto.Modelo;
                    producto.prod_tamanio = xoProducto.Tamanio;
                    producto.prod_tipo = xoProducto.Tipo;
                    producto.prod_pack = xoProducto.CantidadPack;

                    xoDB.SaveChanges();
                }
                else
                {
                    var prodExistente = xoDB.producto.SingleOrDefault(x => x.prod_marca == xoProducto.Marca && x.prod_modelo == xoProducto.Modelo && x.prod_tamanio == xoProducto.Tamanio);

                    if (prodExistente != null)
                    {
                        xsError = "El producto ya existe";
                    }
                    else
                    {
                        if (xoProducto.Modelo == 0)
                            xoProducto.Modelo = xoDB.modelo.FirstOrDefault(x => x.mod_marca == xoProducto.Marca).mod_id;

                        using (var xoTransaccion = xoDB.Database.BeginTransaction())
                        {
                            try
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
                            catch (Exception ex)
                            {
                                xsError = ex.Message;
                                xoTransaccion.Rollback();
                            }
                        }
                    }
                }
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

        public void HabilitarProducto(int xiId, out string xsError)
        {
            xsError = "";

            using (BD_Entities xoDB = new BD_Entities())
            {
                try
                {
                    var xoProducto = xoDB.producto.Find(xiId);
                    if (xoProducto != null)
                    {
                        xoProducto.prod_delet = "N";
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

        public List<spGetPreciosLista> ObtenerListaPrecios(out string xsError)
        {
            xsError = "";
            List<spGetPreciosLista> loResultado = null;

            using (BD_Entities xoDB = new BD_Entities())
            {
                try
                {
                    loResultado = xoDB.Database.SqlQuery<spGetPreciosLista>("exec spGetListaPrecios").ToList();
                }
                catch (Exception ex)
                {
                    xsError = ex.Message;
                }
            }

            return loResultado;
        }

        public void ActualizarPrecios(List<PrecioEdicion> xoPrecios, out string xsError)
        {
            xsError = "";

            using (BD_Entities xoDB = new BD_Entities())
            {
                using (var xoTransaccion = xoDB.Database.BeginTransaction())
                {
                    try
                    {
                        //Agregamos todos los productos levantados
                        foreach (var item in xoPrecios)
                        {
                            var xoNuevo = new AuxPrecios()
                            {
                                Marca = item.IdMarca,
                                Tamanio = item.IdTamanio,
                                Costo = item.Costo,
                                Porcentaje = item.Porcentaje,
                                PrecioVenta = item.PrecioVenta
                            };

                            xoDB.AuxPrecios.Add(xoNuevo);
                        }

                        //Guardamos los precios nuevos en la auxiliar
                        xoDB.SaveChanges();

                        //Actualizamos los precios por SP
                        var retVal = new SqlParameter("@RetVal", SqlDbType.Int);
                        retVal.Direction = ParameterDirection.ReturnValue;

                        xoDB.Database.ExecuteSqlCommand("exec spActualizarPrecios", retVal);

                        if ((int)retVal.Value != 0)
                        {
                            xsError = "Error al querer modificar los precios";
                            xoTransaccion.Rollback();
                        }
                        else
                        {
                            //Limpiamos la tabla para usarla vacía la próxima vez
                            xoDB.Database.ExecuteSqlCommand("DELETE FROM AuxPrecios", retVal);

                            if ((int)retVal.Value != 0)
                            {
                                xsError = "Error al querer modificar los precios";
                                xoTransaccion.Rollback();
                            }
                            else
                            {
                                xoTransaccion.Commit();
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        xsError = ex.Message;
                        xoTransaccion.Rollback();
                    }
                }

            }
        }

        public List<spGetProductosEdicion> ObtenerProductosEdicion(out string xsError)
        {
            xsError = "";
            List<spGetProductosEdicion> loResultado = null;

            using (BD_Entities xoDB = new BD_Entities())
            {
                try
                {
                    loResultado = xoDB.Database.SqlQuery<spGetProductosEdicion>("exec spGetProductosEdicion").ToList();
                }
                catch (Exception ex)
                {
                    xsError = ex.Message;
                }
            }

            return loResultado;
        }

        #endregion

        #region MARCAS

        public void GuardarMarca(MarcaForm xoMarca, out string xsError)
        {
            xsError = "";

            using (BD_Entities xoDB = new BD_Entities())
            {
                try
                {
                    var loMarca = xoDB.marca.Find(xoMarca.Id);

                    if (loMarca != null)
                    {
                        loMarca.mar_nombre = xoMarca.Nombre;
                    }
                    else
                    {
                        var _marca = xoDB.marca.FirstOrDefault(x => x.mar_nombre.ToLower().Equals(xoMarca.Nombre));

                        if (_marca != null)
                            xsError = "Ya existe ésta marca";
                        else
                        {
                            xoDB.marca.Add(new marca() { mar_nombre = xoMarca.Nombre });
                        }
                    }

                    if (xsError == "")
                    {
                        xoDB.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    xsError = ex.Message;
                }
            }
        }

        public void HabilitarMarca(int xiId, out string xsError)
        {
            xsError = "";

            using (BD_Entities xoDB = new BD_Entities())
            {
                try
                {
                    var xoMarca = xoDB.marca.Find(xiId);

                    if (xoMarca != null)
                    {
                        xoMarca.mar_delet = "N";
                        xoDB.SaveChanges();
                    }
                    else
                        xsError = "La marca seleccionada no existe";
                }
                catch (Exception ex)
                {
                    xsError = ex.Message;
                }
            }
        }

        public void EliminarMarca(int xiId, out string xsError)
        {
            xsError = "";

            using (BD_Entities xoDB = new BD_Entities())
            {
                try
                {
                    var xoMarca = xoDB.marca.Find(xiId);

                    if (xoMarca != null)
                    {
                        if (xoDB.producto.FirstOrDefault(x => x.prod_marca == xoMarca.mar_id && (x.prod_delet ?? "N") == "S") == null)
                        {
                            xoMarca.mar_delet = "S";
                            xoDB.SaveChanges();
                        }
                        else
                            xsError = "Debe eliminar el/los productos asociados antes de eliminar la marca";
                    }
                    else
                        xsError = "La marca seleccionada no existe";
                }
                catch (Exception ex)
                {
                    xsError = ex.Message;
                }
            }
        }

        #endregion

        #region MODELOS

        public void GuardarModelo(ModeloForm xoModelo, out string xsError)
        {
            xsError = "";

            using (BD_Entities xoDB = new BD_Entities())
            {
                try
                {
                    var loModelo = xoDB.modelo.FirstOrDefault(x => x.mod_id == xoModelo.Id && x.mod_marca == xoModelo.IdMarca);

                    if (loModelo != null)
                    {
                        loModelo.mod_nombre = xoModelo.Nombre;
                    }
                    else
                    {
                        var _modelo = xoDB.modelo.FirstOrDefault(x => x.mod_nombre.ToLower().Equals(xoModelo.Nombre));

                        if (_modelo != null)
                            xsError = "Ya existe éste modelo";
                        else
                        {
                            xoDB.modelo.Add(new modelo() { mod_nombre = xoModelo.Nombre, mod_marca = xoModelo.IdMarca });
                        }
                    }

                    if (xsError == "")
                    {
                        xoDB.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    xsError = ex.Message;
                }
            }
        }

        public void HabilitarModelo(int xiId, out string xsError)
        {
            xsError = "";

            using (BD_Entities xoDB = new BD_Entities())
            {
                try
                {
                    var xoModelo = xoDB.modelo.Find(xiId);

                    if (xoModelo != null)
                    {
                        xoModelo.mod_delet = "N";
                        xoDB.SaveChanges();
                    }
                    else
                        xsError = "El modelo seleccionado no existe";
                }
                catch (Exception ex)
                {
                    xsError = ex.Message;
                }
            }
        }

        public void EliminarModelo(int xiId, out string xsError)
        {
            xsError = "";

            using (BD_Entities xoDB = new BD_Entities())
            {
                try
                {
                    var xoModelo = xoDB.modelo.Find(xiId);

                    if (xoModelo != null)
                    {
                        if (xoDB.producto.FirstOrDefault(x => x.prod_modelo == xoModelo.mod_id && (x.prod_delet ?? "N") == "S") == null)
                        {
                            xoModelo.mod_delet = "S";
                            xoDB.SaveChanges();
                        }
                        else
                            xsError = "Debe eliminar el/los producto asociado antes de eliminar el modelo";
                    }
                    else
                        xsError = "El modelo seleccionado no existe";
                }
                catch (Exception ex)
                {
                    xsError = ex.Message;
                }
            }
        }

        #endregion

        #region TAMAÑOS

        public void GuardarTamanio(TamanioForm xoTamanio, out string xsError)
        {
            xsError = "";

            using (BD_Entities xoDB = new BD_Entities())
            {
                try
                {
                    var loTamanio = xoDB.tamanio.Find(xoTamanio.Id);

                    if (loTamanio != null)
                        loTamanio.tam_descripcion = xoTamanio.Descripcion;
                    else
                    {
                        var _tamanio = xoDB.tamanio.FirstOrDefault(x => x.tam_descripcion.ToLower().Equals(xoTamanio.Descripcion));

                        if (_tamanio != null)
                            xsError = "Ya existe éste tamaño";
                        else
                            xoDB.tamanio.Add(new tamanio() { tam_descripcion = xoTamanio.Descripcion });
                    }

                    if (xsError == "")
                        xoDB.SaveChanges();
                }
                catch (Exception ex)
                {
                    xsError = ex.Message;
                }
            }
        }

        public void HabilitarTamanio(int xiId, out string xsError)
        {
            xsError = "";

            using (BD_Entities xoDB = new BD_Entities())
            {
                try
                {
                    var xoTamanio = xoDB.tamanio.Find(xiId);

                    if (xoTamanio != null)
                    {
                        xoTamanio.tam_delet = "N";
                        xoDB.SaveChanges();
                    }
                    else
                        xsError = "El tamanio seleccionado no existe";
                }
                catch (Exception ex)
                {
                    xsError = ex.Message;
                }
            }
        }

        public void EliminarTamanio(int xiId, out string xsError)
        {
            xsError = "";

            using (BD_Entities xoDB = new BD_Entities())
            {
                try
                {
                    var xoTamanio = xoDB.tamanio.Find(xiId);

                    if (xoTamanio != null)
                    {
                        if (xoDB.producto.FirstOrDefault(x => x.prod_tamanio == xoTamanio.tam_id && (x.prod_delet ?? "N") == "S") == null)
                        {
                            xoTamanio.tam_delet = "S";
                            xoDB.SaveChanges();
                        }
                        else
                        {
                            xsError = "Elimine el/los productos asociados para poder eliminar el tamaño";
                        }
                    }
                    else
                        xsError = "El tamaño seleccionado no existe";
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
