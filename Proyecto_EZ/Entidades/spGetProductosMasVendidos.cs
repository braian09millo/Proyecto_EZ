using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class spGetProductosMasVendidos
    {
        public int IdProducto { get; set; }
        public string Producto { get; set; }
        public int CantidadPacks { get; set; }
        public long Ranking { get; set; }
        public decimal Porcentaje { get; set; }
        public int VentasUnitarias { get; set; }
        public decimal TotalFacturado { get; set; }
    }
}
