﻿using Datos;
using Entidades;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocios.BusinessControllers
{
    public class PedidoCtrl
    {
        #region SINGLETON

        private static PedidoCtrl _PedidoCtrl;

        public static PedidoCtrl GetInstancia()
        {
            if (_PedidoCtrl == null)
                _PedidoCtrl = new PedidoCtrl();

            return _PedidoCtrl;
        }

        #endregion

        #region COMPORTAMIENTO

        public void GuardarPedido(int xiPedido, DateTime xdFechaEntrega, string xsEstado, int xiCliente, string xsUsuario, decimal xdTotal, decimal xdFacturado, List<Pedido> xoProductos, string xsAplicaDesc, decimal xdDescuento, int? xiVuelta, out string xsError)
        {
            xsError = "";
            int xiIdPedido = 0;

            using (BD_Entities xoDB = new BD_Entities())
            {
                try
                {
                    var xoPedido = xoDB.pedido.Find(xiPedido);

                    if (xoPedido != null)
                    {
                        if (xoPedido.ped_estado.Trim().Equals("E") &&
                            DateTime.Today > xdFechaEntrega &&
                            xsEstado.Equals("C"))
                        {
                            xsError = "Si el pedido pasa a estado CARGADO, la fecha de entrega no puede ser menor a " + DateTime.Today.ToString("dd/MM/yyy");
                        }
                        else
                        {
                            //Actualizamos el pedido
                            xoPedido.ped_cliente = xiCliente;
                            xoPedido.ped_monto = Math.Round(xdTotal);
                            xoPedido.ped_factu = Math.Round(xdFacturado);
                            xoPedido.ped_estado = xsEstado;
                            xoPedido.ped_repartidor = xsUsuario;
                            xoPedido.ped_fecha = xdFechaEntrega;
                            xoPedido.ped_apdes = xsAplicaDesc;
                            xoPedido.ped_descu = xdDescuento;
                            xoPedido.ped_vuelta = (byte?)xiVuelta;

                            //Borramos todos los items del pedido
                            var items = xoDB.pedido_detalle.Where(x => x.det_pedido == xiPedido).ToList();
                            xoDB.pedido_detalle.RemoveRange(items);

                            xoDB.SaveChanges();
                        }
                    }
                    else
                    {
                        //Primero guardamos la cabecera del pedido
                        var xoCabeceraPedido = new pedido()
                        {
                            ped_cliente = xiCliente,
                            ped_fecha = xdFechaEntrega,
                            ped_fechaCarga = DateTime.Now,
                            ped_monto = Math.Round(xdTotal),
                            ped_factu = Math.Round(xdFacturado),
                            ped_estado = "C",
                            ped_repartidor = xsUsuario,
                            ped_apdes = xsAplicaDesc,
                            ped_descu = xdDescuento,
                            ped_vuelta = (byte?)xiVuelta
                    };

                        xoDB.pedido.Add(xoCabeceraPedido);
                        xoDB.SaveChanges();

                        //Obtenemos el identity de la cabecera
                        xiIdPedido = xoCabeceraPedido.ped_id;

                    }

                    if (xsError == "")
                    {
                        //Para cada producto guardamos el detalle
                        foreach (var item in xoProductos)
                        {
                            var xoPedidoDetalle = new pedido_detalle()
                            {
                                det_pedido = xoPedido != null ? xiPedido : xiIdPedido,
                                det_producto = item.IdProducto,
                                det_cantidad = item.Cantidad,
                                det_precio = Math.Round(item.Precio, 0),
                                det_monto = Math.Round(item.Monto, 0)
                            };

                            xoDB.pedido_detalle.Add(xoPedidoDetalle);
                        }

                        xoDB.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    xsError = ex.Message;
                }
            }
        }

        public object ObtenerPedidos(DateTime? xdFechaDesde, DateTime? xdFechaHasta, int? xiCliente, string xsEstado, string xsUsuario, out string xsError)
        {
            xsError = "";
            object xoPedidos = null;

            using (BD_Entities xoDB = new BD_Entities())
            {
                try
                {
                    var xoResultado = xoDB.Database.SqlQuery<spGetPedidos>("exec spGetPedidos @FechaDesde, @FechaHasta, @Cliente, @Estado, @Usuario",
                                        new SqlParameter("@FechaDesde", xdFechaDesde == null ? SqlDateTime.Null : (DateTime)xdFechaDesde),
                                        new SqlParameter("@FechaHasta", xdFechaHasta == null ? SqlDateTime.Null : (DateTime)xdFechaHasta),
                                        new SqlParameter("@Cliente", xiCliente == null ? SqlInt32.Null : (int)xiCliente),
                                        new SqlParameter("@Estado", xsEstado == null ? SqlString.Null : xsEstado),
                                        new SqlParameter("@Usuario", xsUsuario == null ? SqlString.Null : xsUsuario)).ToList();

                    foreach (var item in xoResultado)
                    {
                        item.PedidoDetalle = ObtenerDetallePedido(item.IdPedido, out xsError);
                    }

                    xoPedidos = xoResultado.Select(x => new[] {
                        x.IdPedido.ToString(),
                        x.Cliente + " - " + x.ClienteDireccion,
                        x.Fecha.ToString("dd-MM-yyyy"),
                        x.EstadoDescripcion,
                        "$ " + string.Format("{0:0.##}", x.Monto),
                        "$ " + string.Format("{0:0.##}", x.Facturado),
                        "$ " + string.Format("{0:0.##}", x.Monto-x.Facturado),
                        //"$ " + string.Format("{0:0.##}", x.Debe),
                        x.Repartidor,
                        x.Vuelta == 0 ? "" : x.Vuelta.ToString(),
                        x.Estado,
                        x.IdCliente.ToString(),
                        x.IdRepartidor,
                        JsonConvert.SerializeObject(x.PedidoDetalle),
                        JsonConvert.SerializeObject(x)}).ToList();
                }
                catch (Exception ex)
                {
                    xsError = ex.Message;
                }
            }

            return xoPedidos;
        }

        public void EliminarPedido(int xiId, out string xsError)
        {
            xsError = "";

            using (BD_Entities xoDB = new BD_Entities())
            {
                try
                {
                    //Primero eliminamos el detalle del pedido
                    var loPedidoDetalle = xoDB.pedido_detalle.Where(x => x.det_pedido == xiId).ToList();
                    xoDB.pedido_detalle.RemoveRange(loPedidoDetalle);
                    xoDB.SaveChanges();

                    //Ahora la cabecera del pedido
                    var loPedidoCabecera = xoDB.pedido.Find(xiId);
                    xoDB.pedido.Remove(loPedidoCabecera);
                    xoDB.SaveChanges();
                }
                catch (Exception ex)
                {
                    xsError = ex.Message;
                }
            }
        }

        public List<spGetPedidoDetalle> ObtenerDetallePedido(int xiPedido, out string xsError)
        {
            xsError = "";
            var lstDetalle = new List<spGetPedidoDetalle>();

            using (BD_Entities xoDB = new BD_Entities())
            {
                try
                {
                    lstDetalle = xoDB.Database.SqlQuery<spGetPedidoDetalle>("exec spGetPedidoDetalle @Pedido",
                                              new SqlParameter("@Pedido", xiPedido)).ToList();
                }
                catch (Exception ex)
                {
                    xsError = ex.Message;
                }
            }

            return lstDetalle;
        }

        #endregion

        #region INFORMES
        public List<spRptRemito> ObtenerPedidosRpt(int xiPedido)
        {
            var xoResultado = new List<spRptRemito>();

            using (BD_Entities xoDB = new BD_Entities())
            {
                try
                {
                    xoResultado = xoDB.Database.SqlQuery<spRptRemito>("exec spGetRptPedido @Pedido",
                                               new SqlParameter("@Pedido", xiPedido)).ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return xoResultado;
        }



        public List<spGetFacturacionMensual> ObtenerFacturacionMensualRpt(DateTime xdFechaDesde, DateTime xdFechaHasta, string xsRepartidor)
        {
            var xoResultado = new List<spGetFacturacionMensual>();
            if (xsRepartidor == "") xsRepartidor = null;

            using (BD_Entities xoDB = new BD_Entities())
            {
                try
                {
                    xoResultado = xoDB.Database.SqlQuery<spGetFacturacionMensual>("exec spRptFacturacionMensual @FechaDesde, @FechaHasta, @Repartidor",
                                               new SqlParameter("@FechaDesde", xdFechaDesde),
                                               new SqlParameter("@FechaHasta", xdFechaHasta),
                                               new SqlParameter("@Repartidor", xsRepartidor == null ? SqlString.Null : xsRepartidor)).ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return xoResultado;
        }

        public List<spGetFacturacionResultado> ObtenerFacturacionResultadoRpt(DateTime xdFechaDesde, DateTime xdFechaHasta, string xsRepartidor)
        {
            var xoResultado = new List<spGetFacturacionResultado>();
            if (xsRepartidor == "") xsRepartidor = null;

            using (BD_Entities xoDB = new BD_Entities())
            {
                try
                {
                    xoResultado = xoDB.Database.SqlQuery<spGetFacturacionResultado>("exec spFacturacionResultado @FechaDesde, @FechaHasta, @Repartidor",
                                               new SqlParameter("@FechaDesde", xdFechaDesde),
                                               new SqlParameter("@FechaHasta", xdFechaHasta),
                                               new SqlParameter("@Repartidor", xsRepartidor == null ? SqlString.Null : xsRepartidor)).ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return xoResultado;
        }

        public List<spRptRemitoRepartidor> ObtenerRemitoRepartidorRpt(DateTime xdFecha, string xsRepartidor, int? xiVuelta)
        {
            var xoResultado = new List<spRptRemitoRepartidor>();
            if (xsRepartidor == "") xsRepartidor = null;

            using (BD_Entities xoDB = new BD_Entities())
            {
                try
                {
                    xoResultado = xoDB.Database.SqlQuery<spRptRemitoRepartidor>("exec spGetRemitos @Fecha, @Repartidor, @Vuelta",
                                               new SqlParameter("@Fecha", xdFecha),
                                               new SqlParameter("@Repartidor", xsRepartidor == null ? SqlString.Null : xsRepartidor),
                                               new SqlParameter("@Vuelta", xiVuelta == null ? SqlByte.Null : (byte)xiVuelta)).ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return xoResultado;
        }

        #endregion
    }
}
