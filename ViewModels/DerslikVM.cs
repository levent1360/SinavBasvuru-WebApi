using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SinavBasvuruApi.ViewModels
{
    public class DerslikVM
    {
        public string DerslikId { get; set; }
        public string DerslikAd { get; set; }
        public string DerslikKat { get; set; }
        public string DerslikFakulteId { get; set; }
        public string DerslikFakulteAd { get; set; }
        public int DerslikKapasite { get; set; }
        public int DerslikAktif { get; set; }
    }
}