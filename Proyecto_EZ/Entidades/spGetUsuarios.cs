using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class spGetUsuarios
    {
        public string usu_usuario { get; set; }
        public string usu_grupoDesc { get; set; }
        public string usu_nombre { get; set; }
        public string usu_apellido { get; set; }
        public DateTime? usu_fecha_acceso { get; set; }
        public string usu_password { get; set; }
        public byte usu_grupo { get; set; }
        public string usu_delet { get; set; }
    }
}
