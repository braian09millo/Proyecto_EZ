using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class ProductoForm
    {
        public int Id { get; set; }
        public int Marca { get; set; }
        public int Modelo { get; set; }
        public int Tamanio { get; set; }
        public int Tipo { get; set; }
        public int CantidadPack { get; set; }
        public decimal Costo { get; set; }
        public decimal Porcentaje { get; set; }
        public decimal PrecioVenta { get; set; }
    }
}
