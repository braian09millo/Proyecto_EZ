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
    
    public partial class cliente
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public cliente()
        {
            this.pedido = new HashSet<pedido>();
            this.remito = new HashSet<remito>();
        }
    
        public int cli_id { get; set; }
        public string cli_nombre { get; set; }
        public int cli_provincia { get; set; }
        public int cli_localidad { get; set; }
        public string cli_direccion { get; set; }
        public string cli_telefono { get; set; }
        public string cli_celular { get; set; }
        public string cli_celular2 { get; set; }
        public string cli_coordenadas { get; set; }
        public string cli_mail { get; set; }
        public string cli_delet { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<pedido> pedido { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<remito> remito { get; set; }
        public virtual localidad localidad { get; set; }
        public virtual provincia provincia { get; set; }
    }
}
