//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SinavBasvuruApi.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class DERSLIK
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DERSLIK()
        {
            this.BASVURU = new HashSet<BASVURU>();
            this.OGRETIMELEMANIDERSLIK = new HashSet<OGRETIMELEMANIDERSLIK>();
        }
    
        public string DerslikId { get; set; }
        public string DerslikAd { get; set; }
        public string DerslikKat { get; set; }
        public string DerslikFakulteId { get; set; }
        public int DerslikKapasite { get; set; }
        public int DerslikAktif { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BASVURU> BASVURU { get; set; }
        public virtual FAKULTE FAKULTE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OGRETIMELEMANIDERSLIK> OGRETIMELEMANIDERSLIK { get; set; }
    }
}
