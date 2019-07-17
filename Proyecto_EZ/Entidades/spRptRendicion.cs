using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class spRptRendicion
    {
        public int Cantidad { get; set; }
        public String Descripcion { get; set; }
        public decimal Precio { get; set; }
        public decimal Total { get; set; }
        public DateTime Desde { get; set; }
        public DateTime Hasta { get; set; }
    }
}
