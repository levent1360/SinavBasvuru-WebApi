using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SinavBasvuruApi.ViewModels
{
    public class OgretimElemaniDerslikVM
    {
        public string OEDId { get; set; }
        public string OEDerslikId { get; set; }
        public string OEDerslikAd { get; set; }
        public string OESinavId { get; set; }
        public string OESinavAd { get; set; }
        public string OESinavTarih { get; set; }
        public string OEDOEId { get; set; }
        public string OEDOEAd { get; set; }
        public string OEDOESoyad { get; set; }
        public string OEDOEUnvan { get; set; }
    }
}