//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Datos
{
    using System;
    using System.Collections.Generic;
    
    public partial class remito
    {
        public int rem_numero { get; set; }
        public int rem_cliente { get; set; }
        public int rem_pedido { get; set; }
    
        public virtual cliente cliente { get; set; }
        public virtual pedido pedido { get; set; }
    }
}
