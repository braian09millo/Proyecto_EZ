using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class spGetPedidos
    {
        public int IdPedido { get; set; }
        public string Cliente { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Monto { get; set; }
    }
}
