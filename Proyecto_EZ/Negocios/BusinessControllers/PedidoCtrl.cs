using Datos;
using Entidades;
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
            
        public void AgregarPedido(int xiCliente, string xsUsuario, decimal xdTotal, decimal xdFacturado, List<Pedido> xoProductos, out string xsError)
        {
            xsError = "";

            using (BD_Entities xoDB = new BD_Entities())
            {
                try
                {
                    //Primero guardamos la cabecera del pedido
                    var xoCabeceraPedido = new pedido()
                    {
                        ped_cliente = xiCliente,
                        ped_fecha = DateTime.Now,
                        ped_monto = Math.Round(xdTotal, 2),
                        ped_resto = Math.Round(xdFacturado, 2),
                        ped_estado = "C",
                        ped_repartidor = xsUsuario
                    };

                    xoDB.pedido.Add(xoCabeceraPedido);
                    xoDB.SaveChanges();

                    //Obtenemos el identity de la cabecera
                    int xiIdPedido = xoCabeceraPedido.ped_id;

                    //Para cada producto guardamos el detalle
                    foreach (var item in xoProductos)
                    {
                        var xoPedidoDetalle = new detalle_pedido()
                        {
                            det_pedido = xiIdPedido,
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

        public object ObtenerPedidos(DateTime? xdFecha, int? xiCliente, string xsEstado, string xsUsuario, out string xsError)
        {
            xsError = "";
            object xoPedidos = null;

            using (BD_Entities xoDB = new BD_Entities())
            {
                try
                {
                    var xoResultado = xoDB.Database.SqlQuery<spGetPedidos>("exec spGetPedidos @Fecha, @Cliente, @Estado, @Usuario",
                                        new SqlParameter("@Fecha", xdFecha == null ? SqlDateTime.Null : (DateTime)xdFecha),
                                        new SqlParameter("@Cliente", xiCliente == null ? SqlInt32.Null : (int)xiCliente),
                                        new SqlParameter("@Estado", xsEstado == null ? SqlString.Null : xsEstado),
                                        new SqlParameter("@Usuario", xsUsuario == null ? SqlString.Null : xsUsuario)).ToList();
                    xoPedidos = xoResultado.Select(x => new [] {
                        x.IdPedido.ToString(),
                        x.Cliente,
                        x.Fecha.ToString("dd-MM-yyyy"),
                        "$ " + string.Format("{0:0.##}", x.Monto)}).ToList();
                }
                catch (Exception ex)
                {
                    xsError = ex.Message;
                }
            }

            return xoPedidos;
        }

        #endregion
    }
}
