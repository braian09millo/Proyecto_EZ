using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class PrecioEdicion
    {
        public int IdMarca { get; set; }
        public int IdTamanio { get; set; }
        public decimal Porcentaje { get; set; }
        public decimal Costo { get; set; }
        public decimal PrecioVenta { get; set; }
    }
}
