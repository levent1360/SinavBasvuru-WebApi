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
    
    public partial class OGRENCI
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OGRENCI()
        {
            this.BASVURU = new HashSet<BASVURU>();
        }
    
        public string OgrenciId { get; set; }
        public string OgrenciNo { get; set; }
        public string OgrenciTC { get; set; }
        public string OgrenciAd { get; set; }
        public string OgrenciSoyad { get; set; }
        public string OgrenciMail { get; set; }
        public string OgrenciTel { get; set; }
        public string OgrenciFakulteId { get; set; }
        public string OgrenciBolum { get; set; }
        public string OgrenciSinif { get; set; }
        public string OgrenciKayitTuru { get; set; }
        public string OgrenciDerece { get; set; }
        public int OgrenciAktif { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BASVURU> BASVURU { get; set; }
        public virtual FAKULTE FAKULTE { get; set; }
    }
}
