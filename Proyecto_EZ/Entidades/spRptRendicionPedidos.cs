using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class spRptRendicionPedidos
    {
        public string FechaRendicion { get; set; }
        public string Cliente { get; set; }
        public DateTime FechaPedido { get; set; }
        public decimal Costo { get; set; }
        public string Rendido { get; set; }
    }
}
