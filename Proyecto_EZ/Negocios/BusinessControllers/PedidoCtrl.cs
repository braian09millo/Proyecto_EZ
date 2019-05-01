using Datos;
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
            
        public void GuardarPedido(int xiPedido, string xsEstado, int xiCliente, string xsUsuario, decimal xdTotal, decimal xdFacturado, List<Pedido> xoProductos, out string xsError)
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
                        //Actualizamos el pedido
                        xoPedido.ped_cliente = xiCliente;
                        xoPedido.ped_monto = Math.Round(xdTotal, 2);
                        xoPedido.ped_resto = Math.Round(xdFacturado, 2);
                        xoPedido.ped_estado = xsEstado;
                        xoPedido.ped_repartidor = xsUsuario;

                        //Borramos todos los items del pedido
                        var items = xoDB.detalle_pedido.Where(x => x.det_pedido == xiPedido).ToList();
                        xoDB.detalle_pedido.RemoveRange(items);

                        xoDB.SaveChanges();
                    }
                    else
                    {
                        //Primero guardamos la cabecera del pedido
                        var xoCabeceraPedido = new pedido()
                        {
                            ped_cliente = xiCliente,
                            ped_fecha = DateTime.Today,
                            ped_monto = Math.Round(xdTotal, 2),
                            ped_resto = Math.Round(xdFacturado, 2),
                            ped_estado = "C",
                            ped_repartidor = xsUsuario
                        };

                        xoDB.pedido.Add(xoCabeceraPedido);
                        xoDB.SaveChanges();

                        //Obtenemos el identity de la cabecera
                        xiIdPedido = xoCabeceraPedido.ped_id;

                    }

                    //Para cada producto guardamos el detalle
                    foreach (var item in xoProductos)
                    {
                        var xoPedidoDetalle = new detalle_pedido()
                        {
                            det_pedido = xoPedido != null ? xiPedido : xiIdPedido,
                            det_producto = item.IdProducto,
                            det_cantidad = item.Cantidad,
                            det_precio = item.Precio,
                            det_monto = item.Monto
                        };

                        xoDB.detalle_pedido.Add(xoPedidoDetalle);
                    }

                    xoDB.SaveChanges();
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

                    xoPedidos = xoResultado.Select(x => new [] {
                        x.IdPedido.ToString(),
                        x.Cliente,
                        x.Fecha.ToString("dd-MM-yyyy"),
                        x.EstadoDescripcion,
                        "$ " + string.Format("{0:0.##}", x.Monto),
                        "$ " + string.Format("{0:0.##}", x.Facturado),
                        x.Repartidor,
                        x.Estado,
                        x.IdCliente.ToString(),
                        JsonConvert.SerializeObject(x.PedidoDetalle)}).ToList();
                }
                catch (Exception ex)
                {
                    xsError = ex.Message;
                }
            }

            return xoPedidos;
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
    }
}
