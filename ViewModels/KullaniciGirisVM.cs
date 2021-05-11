using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SinavBasvuruApi.ViewModels
{
    public class KullaniciGirisVM
    {
        public string KullaniciId { get; set; }
        public string KGOEId { get; set; }
        public string KGOEAd { get; set; }
        public string KGOESoyad { get; set; }
        public string KGOEDKullaniciAdi { get; set; }
        public string KGOEDSifre { get; set; }
        public string KGYetki { get; set; }
    }
}