using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class RendicionForm
    {
        public int id { get; set; }
        public DateTime desde { get; set; }
        public DateTime hasta { get; set; }
        public decimal total { get; set; }
    }
}
