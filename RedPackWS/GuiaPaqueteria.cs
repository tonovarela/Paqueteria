//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RedPackWS
{
    using System;
    using System.Collections.Generic;
    
    public partial class GuiaPaqueteria
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GuiaPaqueteria()
        {
            this.PaqueteEnvio = new HashSet<PaqueteEnvio>();
        }
    
        public Nullable<byte> id_paqueteria { get; set; }
        public Nullable<byte> id_tipoServicio { get; set; }
        public string numero_guia { get; set; }
        public bool asignada { get; set; }
    
        public virtual Paqueteria Paqueteria { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PaqueteEnvio> PaqueteEnvio { get; set; }
    }
}
