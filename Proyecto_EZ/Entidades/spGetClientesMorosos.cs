using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class spGetClientesMorosos
    {
        public int IdCliente { get; set; }
        public string Cliente { get; set; }
        public decimal Monto { get; set; }
        public DateTime UltimoPedido { get; set; }
        public long Ranking { get; set; }
        public string Telefono { get; set; }
    }
}
