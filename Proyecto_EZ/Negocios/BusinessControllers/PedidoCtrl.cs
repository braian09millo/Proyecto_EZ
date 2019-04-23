using Datos;
using Entidades;
using System;
using System.Collections.Generic;
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
            
        public void AgregarPedido(int xiCliente, decimal xdTotal, List<Pedido> xoProductos, out string xsError)
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
                        ped_monto = Math.Round(xdTotal, 2)
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

        #endregion
    }
}
