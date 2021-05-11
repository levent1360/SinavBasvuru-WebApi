using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SinavBasvuruApi.ViewModels
{
    public class SinavVM
    {
        public string SinavId { get; set; }
        public string SinavAd { get; set; }
        public string SinavDonem { get; set; }
        public string SinavZamani { get; set; }
        public string SinavTuru { get; set; }
        public string SinavDili { get; set; }
        public int SinavTamam { get; set; }
        public int SinavIptal { get; set; }
        public string SinavAciklama { get; set; }
        public string SinavBasvuruBaslama { get; set; }
        public string SinavBasvuruBitis { get; set; }
        public int SinavBasvuruSayisi { get; set; }
    }
}