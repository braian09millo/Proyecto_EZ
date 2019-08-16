using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class spRptRemito
    {
        public int IdPedido { get; set; }
        public string Cliente { get; set; }
        public string Direccion { get; set; }
        public DateTime Fecha { get; set; }
        public string ProductoDescripcion { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal Monto { get; set; }
        public decimal Descuento { get; set; }
    }
}
