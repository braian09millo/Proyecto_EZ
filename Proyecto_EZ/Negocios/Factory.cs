using Negocios.BusinessControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocios
{
    public class Factory
    {
        public ClienteCtrl GetCtrlCliente()
        {
            return ClienteCtrl.GetInstancia();
        }

        public ProductoCtrl GetCtrlProducto()
        {
            return ProductoCtrl.GetInstancia();
        }

        public PedidoCtrl GetCtrlPedido()
        {
            return PedidoCtrl.GetInstancia();
        }

        public UsuarioCtrl GetCtrlUsuario()
        {
            return UsuarioCtrl.GetInstancia();
        }

        public GastosCtrl GetCtrlGasto()
        {
            return GastosCtrl.GetInstancia();
        }

        public HomeCtrl GetCtrlHome()
        {
            return HomeCtrl.GetInstancia();
        }
    }
}
