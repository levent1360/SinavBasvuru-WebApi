using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SinavBasvuruApi.ViewModels
{
    public class BasvuruVM
    {
        public string BasvuruId { get; set; }
        public string BasvuruOgrenciId { get; set; }
        public string BasvuruOgrenciNo { get; set; }
        public string BasvuruOgrenciTC { get; set; }
        public string BasvuruOgrenciAd { get; set; }
        public string BasvuruOgrenciSoyad { get; set; }
        public string BasvuruOgrenciMail { get; set; }
        public string BasvuruOgrenciTel { get; set; }
        public string BasvuruOgrenciFakulteId { get; set; }
        public string BasvuruOgrenciFakulteAd { get; set; }
        public string BasvuruOgrenciBolum { get; set; }
        public string BasvuruOgrenciSinif { get; set; }
        public string BasvuruOgrenciKayitTuru { get; set; }
        public string BasvuruOgrenciDerece { get; set; }
        public string BasvuruTarihi { get; set; }
        public string BasvuruDuzenlemeTarihi { get; set; }
        public int BasvuruIptal { get; set; }
        public string BasvuruIptalTarihi { get; set; }
        public string BasvuruSinavId { get; set; }
        public string BasvuruSinavAd { get; set; }
        public string BasvuruSinavZamani { get; set; }
        public string BasvuruDerslikId { get; set; }
        public string BasvuruDerslikAd { get; set; }
        public string BasvuruDerslikKat { get; set; }
    }
}