using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class spRptRemitoRepartidor
    {
        public string Repartidor { get; set; }
        public DateTime Fecha { get; set; }
        public string Cliente { get; set; }
        public string Direccion { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Tamanio { get; set; }
        public int Cantidad { get; set; }
        public decimal Monto { get; set; }
    }
}
