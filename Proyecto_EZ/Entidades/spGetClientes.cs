﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class spGetClientes
    {
        public int cli_id { get; set; }
        public string cli_nombre { get; set; }
        public int cli_proid { get; set; }
        public string cli_provincia { get; set; }
        public int cli_locid { get; set; }
        public string cli_localidad { get; set; }
        public string cli_direccion { get; set; }
        public string cli_mail { get; set; }
        public string cli_telefono { get; set; }
        public string cli_celular { get; set; }
        public string cli_celular2 { get; set; }
        public string cli_delet { get; set; }
    }
}
