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
    
    public partial class usuario
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public usuario()
        {
            this.pedido = new HashSet<pedido>();
        }
    
        public string usu_usuario { get; set; }
        public string usu_password { get; set; }
        public byte usu_grupo { get; set; }
        public string usu_nombre { get; set; }
        public string usu_apellido { get; set; }
        public Nullable<System.DateTime> usu_fecha_acceso { get; set; }
        public string usu_delet { get; set; }
    
        public virtual grupo grupo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<pedido> pedido { get; set; }
    }
}
