using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class spGetProductosEdicion
    {
        public int IdMarca { get; set; }
		public string Marca { get; set; }
        public int IdTamanio { get; set; }
        public string Tamanio { get; set; }
        public decimal Costo { get; set; }
        public decimal PrecioVenta { get; set; }
        public decimal Porcentaje { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}
