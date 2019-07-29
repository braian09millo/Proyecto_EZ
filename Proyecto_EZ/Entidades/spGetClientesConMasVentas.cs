using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class spGetClientesConMasVentas
    {
        public string Cliente { get; set; }
        public int CantidadPedidos { get; set; }
        public decimal MontoVentas { get; set; }
        public decimal Debe { get; set; }
        public decimal Facturado { get; set; }
        public DateTime UltimoPedido { get; set; }  
        public long Ranking { get; set; }
    }
}
