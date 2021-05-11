using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SinavBasvuruApi.ViewModels
{
    public class OgrenciVM
    {
        public string OgrenciId { get; set; }
        public string OgrenciNo { get; set; }
        public string OgrenciTC { get; set; }
        public string OgrenciAd { get; set; }
        public string OgrenciSoyad { get; set; }
        public string OgrenciMail { get; set; }
        public string OgrenciTel { get; set; }
        public string OgrenciFakulteId { get; set; }
        public string OgrenciFakulteAd { get; set; }
        public string OgrenciBolum { get; set; }
        public string OgrenciSinif { get; set; }
        public string OgrenciKayitTuru { get; set; }
        public string OgrenciDerece { get; set; }
        public int OgrenciAktif { get; set; }
    }
}