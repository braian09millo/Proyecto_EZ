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
    
    public partial class producto
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public producto()
        {
            this.pedido_detalle = new HashSet<pedido_detalle>();
            this.precio_detalle = new HashSet<precio_detalle>();
        }
    
        public int prod_id { get; set; }
        public int prod_marca { get; set; }
        public int prod_modelo { get; set; }
        public int prod_envase { get; set; }
        public int prod_tamanio { get; set; }
        public int prod_tipo { get; set; }
        public Nullable<int> prod_pack { get; set; }
        public string prod_delet { get; set; }
    
        public virtual envase envase { get; set; }
        public virtual marca marca { get; set; }
        public virtual modelo modelo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<pedido_detalle> pedido_detalle { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<precio_detalle> precio_detalle { get; set; }
        public virtual tamanio tamanio { get; set; }
        public virtual tipo tipo { get; set; }
    }
}
