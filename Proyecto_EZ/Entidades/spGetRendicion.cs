using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class spGetRendicion
    {
        public int IdRendicion { get; set; }
        public DateTime Desde { get; set; }
        public DateTime Hasta { get; set; }
        public decimal Costo { get; set; }
        public List<spGetRendicionDetalle> RendicionDetalle { get; set; }
    }
}
