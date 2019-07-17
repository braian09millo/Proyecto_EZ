using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class spGetFacturacionMensual
    {
        public DateTime Dia { get; set; }
        public int IDRemito { get; set; }
        public string Nombre { get; set; }
        public int Packs { get; set; }
        public decimal costo { get; set; }
        public decimal MontoRemito { get; set; }
        public decimal Debe { get; set; }
        public decimal Facturado { get; set; }
        public string Repartidor { get; set; }
        public decimal Rendido { get; set; }
    }
}
