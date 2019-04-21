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


        #endregion
    }
}
