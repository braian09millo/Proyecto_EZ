//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
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
            this.detalle_pedido = new HashSet<detalle_pedido>();
            this.precio_detalle = new HashSet<precio_detalle>();
        }
    
        public int prod_id { get; set; }
        public int prod_marca { get; set; }
        public int prod_modelo { get; set; }
        public int prod_tamanio { get; set; }
        public int prod_tipo { get; set; }
        public Nullable<int> prod_pack { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<detalle_pedido> detalle_pedido { get; set; }
        public virtual marca marca { get; set; }
        public virtual modelo modelo { get; set; }
        public virtual tamanio tamanio { get; set; }
        public virtual tipo tipo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<precio_detalle> precio_detalle { get; set; }
    }
}
