using SinavBasvuruApi.Models;
using SinavBasvuruApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SinavBasvuruApi.Controllers
{
    public class ServisController : ApiController
    {
        SinavBasvuruEntities db = new SinavBasvuruEntities();
        Sonuc sonuc = new Sonuc();


        #region Fakulte

        [HttpGet]
        [Route("api/fakulteliste")]
        public List<FakulteVM> FakulteListe()
        {
            List<FakulteVM> fakulteliste = db.FAKULTE.Select(x => new FakulteVM()
            {
                FakulteId = x.FakulteId,
                FakulteAd = x.FakulteAd,
                FakulteDerslikSayisi=x.DERSLIK.Count()
            }).ToList();

            return fakulteliste;
        }

        [HttpGet]
        [Route("api/fakultebyid/{FakulteId}")]
        public FakulteVM FakulteById(string FakulteId)
        {
            FakulteVM fakulte = db.FAKULTE.Where(s=>s.FakulteId==FakulteId).Select(x => new FakulteVM()
            {
                FakulteId = x.FakulteId,
                FakulteAd = x.FakulteAd
            }).SingleOrDefault();

            return fakulte;
        }

        [HttpPost]
        [Route("api/fakulteekle")]
        public Sonuc FakulteEkle(FAKULTE fakulte)
        {
            FAKULTE yenifakute = new FAKULTE()
            {
                FakulteId = Guid.NewGuid().ToString(),
                FakulteAd = fakulte.FakulteAd,
            };

            if (fakulte==null)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Girilen fakülte kayıtlıdır";
                return sonuc;

            }

            if (db.FAKULTE.Count(s=> s.FakulteAd==fakulte.FakulteAd)>0)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Girilen fakülte kayıtlıdır";
                return sonuc;
            }

            db.FAKULTE.Add(yenifakute);
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Fakülte başarıyla eklendi.";
            return sonuc;
        }

        [HttpPut]
        [Route("api/fakulteduzenle")]
        public Sonuc FakulteDuzenle(FAKULTE fakulte)
        {
            FAKULTE duzenlefakute = db.FAKULTE.Where(s => s.FakulteId == fakulte.FakulteId).SingleOrDefault();
            FAKULTE fakultekontrol = db.FAKULTE.Where(s => s.FakulteAd == fakulte.FakulteAd && s.FakulteId != fakulte.FakulteId).SingleOrDefault();

            if (duzenlefakute==null)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Kayıt bulunamadı";
                return sonuc;
            }
            if (db.FAKULTE.Count(s => s.FakulteAd.ToLower() == fakulte.FakulteAd.ToLower() && s.FakulteId != fakulte.FakulteId)>0)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Aynı ad ile kayıt bulunmaktadır.";
                return sonuc;
            }
            duzenlefakute.FakulteAd = fakulte.FakulteAd;
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Fakülte başarıyla düzenlendi.";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/fakultesil/{FakulteId}")]
        public Sonuc FakulteSil(string FakulteId)
        {
            FAKULTE silfakulte = db.FAKULTE.Where(s => s.FakulteId == FakulteId).SingleOrDefault();
            
            if (silfakulte == null)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Kayıt bulunamadı";
                return sonuc;
            }
            if (db.DERSLIK.Count(s => s.DerslikFakulteId == FakulteId) > 0)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Derslik bulunan Fakülte silinemez.";
                return sonuc;
            }
            if (db.OGRETIMELEMANI.Count(s=>s.OEFakulteId==FakulteId)>0)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Fakülteye kayıtlı öğretim elemanı bulunmaktadır. Silinemez.";
                return sonuc;
            }
            if (db.OGRENCI.Count(s => s.OgrenciFakulteId == FakulteId) > 0)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Fakülteye kayıtlı öğrenci bulunmaktadır. Silinemez.";
                return sonuc;
            }
            db.FAKULTE.Remove(silfakulte);
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Fakülte başarıyla silindi.";
            return sonuc;
        }
        #endregion

        #region Derslik

        [HttpGet]
        [Route("api/derslikliste")]
        public List<DerslikVM> DerslikListe()
        {
            List<DerslikVM> derslikliste = db.DERSLIK.Select(x => new DerslikVM()
            {
                DerslikId = x.DerslikId,
                DerslikAd = x.DerslikAd,
                DerslikFakulteId= x.DerslikFakulteId,
                DerslikFakulteAd=x.FAKULTE.FakulteAd,
                DerslikKapasite=x.DerslikKapasite,
                DerslikAktif=x.DerslikAktif,
                DerslikKat=x.DerslikKat
            }).ToList();

            return derslikliste;
        }

        [HttpGet]
        [Route("api/derslikbyid/{DerslikId}")]
        public DerslikVM DerslikById(string DerslikId)
        {
            DerslikVM derslik = db.DERSLIK.Where(s => s.DerslikId == DerslikId).Select(x => new DerslikVM()
            {
                DerslikId = x.DerslikId,
                DerslikAd = x.DerslikAd,
                DerslikFakulteId = x.DerslikFakulteId,
                DerslikFakulteAd = x.FAKULTE.FakulteAd,
                DerslikKapasite = x.DerslikKapasite,
                DerslikAktif = x.DerslikAktif,
                DerslikKat = x.DerslikKat
            }).SingleOrDefault();

            return derslik;
        }

        [HttpGet]
        [Route("api/derslikbyfakulteid/{FakulteId}")]
        public List<DerslikVM> DerslikByFakulteId(string FakulteId)
        {
            List<DerslikVM> derslik = db.DERSLIK.Where(s => s.DerslikFakulteId == FakulteId).Select(x => new DerslikVM()
            {
                DerslikId = x.DerslikId,
                DerslikAd = x.DerslikAd,
                DerslikFakulteId = x.DerslikFakulteId,
                DerslikFakulteAd = x.FAKULTE.FakulteAd,
                DerslikKapasite = x.DerslikKapasite,
                DerslikAktif = x.DerslikAktif,
                DerslikKat = x.DerslikKat
            }).ToList();


                return derslik;
            
        }

        [HttpPost]
        [Route("api/derslikekle")]
        public Sonuc DerslikEkle(DERSLIK derslik)
        {
            DERSLIK yeniderslik = new DERSLIK()
            { 
                DerslikId = Guid.NewGuid().ToString(),
                DerslikAd = derslik.DerslikAd,
                DerslikFakulteId = derslik.DerslikFakulteId,
                DerslikKapasite = derslik.DerslikKapasite,
                DerslikKat = derslik.DerslikKat,
                DerslikAktif = derslik.DerslikAktif
            };

            if (db.DERSLIK.Count(s => s.DerslikAd == derslik.DerslikAd && s.DerslikFakulteId==derslik.DerslikFakulteId) > 0)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Girilen derslik bilgisi kayıtlıdır";
                return sonuc;
            }

            db.DERSLIK.Add(yeniderslik);
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Derslik başarıyla eklendi.";
            return sonuc;
        }

        [HttpPut]
        [Route("api/derslikduzenle")]
        public Sonuc DerslikDuzenle(DERSLIK derslik)
        {
            DERSLIK duzenlederslik = db.DERSLIK.Where(s => s.DerslikId == derslik.DerslikId).SingleOrDefault();
            if (duzenlederslik == null)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Kayıt bulunamadı";
                return sonuc;
            }

            if (db.DERSLIK.Count(s => s.DerslikAd.ToLower() == derslik.DerslikAd.ToLower() && s.DerslikFakulteId == derslik.DerslikFakulteId && s.DerslikId!=derslik.DerslikId) > 0)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Aynı ad ile kayıt bulunmaktadır.";
                return sonuc;
            }
            duzenlederslik.DerslikAd = derslik.DerslikAd;
            duzenlederslik.DerslikFakulteId = derslik.DerslikFakulteId;
            duzenlederslik.DerslikKapasite = derslik.DerslikKapasite;
            duzenlederslik.DerslikKat = derslik.DerslikKat;
            duzenlederslik.DerslikAktif = derslik.DerslikAktif;
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Derslik başarıyla düzenlendi.";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/dersliksil/{DerslikId}")]
        public Sonuc DerslikSil(string DerslikId)
        {
            DERSLIK silderslik = db.DERSLIK.Where(s => s.DerslikId == DerslikId).SingleOrDefault();
            if (silderslik == null)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Kayıt bulunamadı";
                return sonuc;
            }
            db.DERSLIK.Remove(silderslik);
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Derslik başarıyla silindi.";
            return sonuc;
        }
        #endregion

        #region OgretimElemani

        [HttpGet]
        [Route("api/ogretimelemaniliste")]
        public List<OgretimElemaniVM> OgretimElemaniListe()
        {
            List<OgretimElemaniVM> ogretimelemaniliste = db.OGRETIMELEMANI.Select(s => new OgretimElemaniVM()
            {
                OEId = s.OEId,
                OEAd = s.OEAd,
                OESoyad = s.OESoyad,
                OEUnvan = s.OEUnvan,
                OEFakulteId = s.OEFakulteId,
                OEFakulteAd = s.FAKULTE.FakulteAd,
                OEEposta = s.OEEposta,
                OETel = s.OETel,
                OEKimlikNo=s.OEKimlikNo
            }).ToList();
            return ogretimelemaniliste;
        }

        [HttpGet]
        [Route("api/ogretimelemanibyid/{OEId}")]
        public OgretimElemaniVM OgretimElemaniById(string OEId)
        {
            OgretimElemaniVM ogretimelemani = db.OGRETIMELEMANI.Where(x=>x.OEId==OEId).Select(s => new OgretimElemaniVM()
            {
                OEId = s.OEId,
                OEAd = s.OEAd,
                OESoyad = s.OESoyad,
                OEUnvan = s.OEUnvan,
                OEFakulteId = s.OEFakulteId,
                OEFakulteAd = s.FAKULTE.FakulteAd,
                OEEposta = s.OEEposta,
                OETel = s.OETel,
                OEKimlikNo = s.OEKimlikNo
            }).SingleOrDefault();

            return ogretimelemani;
        }

        [HttpPost]
        [Route("api/ogretimelemaniekle")]
        public Sonuc OgretimElemaniEkle(OGRETIMELEMANI ogretimelemani)
        {
            OGRETIMELEMANI yeniogretimelemani = new OGRETIMELEMANI()
            { 
                OEId= Guid.NewGuid().ToString(),
                OEAd=ogretimelemani.OEAd,
                OESoyad=ogretimelemani.OESoyad,
                OEFakulteId=ogretimelemani.OEFakulteId,
                OEKimlikNo=ogretimelemani.OEKimlikNo,
                OEUnvan=ogretimelemani.OEUnvan,
                OEEposta=ogretimelemani.OEEposta,
                OETel=ogretimelemani.OETel
            };

            if (db.OGRETIMELEMANI.Count(s => s.OEKimlikNo == ogretimelemani.OEKimlikNo) > 0)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Girilen kimlik numarasına kayıtlı öğretim elemanı bulunmaktadır.";
                return sonuc;
            }

            db.OGRETIMELEMANI.Add(yeniogretimelemani);
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Öğretim elemanı başarıyla eklendi.";
            return sonuc;
        }

        [HttpPut]
        [Route("api/ogretimelemaniduzenle")]
        public Sonuc OgretimElemaniDuzenle(OGRETIMELEMANI ogretimelemani)
        {
            OGRETIMELEMANI duzenleogretimelemani = db.OGRETIMELEMANI.Where(s => s.OEId == ogretimelemani.OEId).SingleOrDefault();
            if (duzenleogretimelemani == null)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Kayıt bulunamadı";
                return sonuc;
            }

            duzenleogretimelemani.OEAd = ogretimelemani.OEAd;
            duzenleogretimelemani.OESoyad = ogretimelemani.OESoyad;
            duzenleogretimelemani.OEUnvan = ogretimelemani.OEUnvan;
            duzenleogretimelemani.OETel = ogretimelemani.OETel;
            duzenleogretimelemani.OEFakulteId = ogretimelemani.OEFakulteId;
            duzenleogretimelemani.OEEposta = ogretimelemani.OEEposta;
            duzenleogretimelemani.OEKimlikNo = ogretimelemani.OEKimlikNo;
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Öğretim elemanı başarıyla düzenlendi.";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/ogretimelemanisil/{OEId}")]
        public Sonuc OgretimElemaniSil(string OEId)
        {
            OGRETIMELEMANI silogretimelemani = db.OGRETIMELEMANI.Where(s => s.OEId == OEId).SingleOrDefault();
            if (silogretimelemani == null)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Kayıt bulunamadı";
                return sonuc;
            }
            db.OGRETIMELEMANI.Remove(silogretimelemani);
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Öğretim elemanı başarıyla silindi.";
            return sonuc;
        }
        #endregion

        #region Ogrenci

        [HttpGet]
        [Route("api/ogrenciliste")]
        public List<OgrenciVM> OgrenciListe()
        {
            List<OgrenciVM> ogrenciliste = db.OGRENCI.Select(s => new OgrenciVM()
            {
                OgrenciId=s.OgrenciId,
                OgrenciAd=s.OgrenciAd,
                OgrenciSoyad=s.OgrenciSoyad,
                OgrenciNo=s.OgrenciNo,
                OgrenciTC=s.OgrenciTC,
                OgrenciFakulteId=s.OgrenciFakulteId,
                OgrenciFakulteAd=s.FAKULTE.FakulteAd,
                OgrenciBolum=s.OgrenciBolum,
                OgrenciSinif=s.OgrenciSinif,
                OgrenciKayitTuru=s.OgrenciKayitTuru,
                OgrenciDerece=s.OgrenciDerece,
                OgrenciTel=s.OgrenciTel,
                OgrenciMail=s.OgrenciMail,
                OgrenciAktif=s.OgrenciAktif
            }).ToList();

            return ogrenciliste;
        }

        [HttpGet]
        [Route("api/ogrencibyid/{OgrenciId}")]
        public OgrenciVM OgrenciById(string OgrenciId)
        {
            OgrenciVM ogrenci = db.OGRENCI.Where(x => x.OgrenciId == OgrenciId).Select(s => new OgrenciVM()
            {
                OgrenciId = s.OgrenciId,
                OgrenciAd = s.OgrenciAd,
                OgrenciSoyad = s.OgrenciSoyad,
                OgrenciNo = s.OgrenciNo,
                OgrenciTC = s.OgrenciTC,
                OgrenciFakulteId = s.OgrenciFakulteId,
                OgrenciFakulteAd = s.FAKULTE.FakulteAd,
                OgrenciBolum = s.OgrenciBolum,
                OgrenciSinif = s.OgrenciSinif,
                OgrenciKayitTuru = s.OgrenciKayitTuru,
                OgrenciDerece = s.OgrenciDerece,
                OgrenciTel = s.OgrenciTel,
                OgrenciMail = s.OgrenciMail,
                OgrenciAktif = s.OgrenciAktif
            }).SingleOrDefault();

            return ogrenci;
        }

        [HttpPost]
        [Route("api/ogrenciekle")]
        public Sonuc OgrenciEkle(OGRENCI ogrenci)
        {
            OGRENCI yeniogrenci = new OGRENCI()
            {
                OgrenciId = Guid.NewGuid().ToString(),
                OgrenciAd=ogrenci.OgrenciAd,
                OgrenciSoyad = ogrenci.OgrenciSoyad,
                OgrenciNo = ogrenci.OgrenciNo,
                OgrenciTC = ogrenci.OgrenciTC,
                OgrenciFakulteId = ogrenci.OgrenciFakulteId,
                OgrenciBolum = ogrenci.OgrenciBolum,
                OgrenciSinif = ogrenci.OgrenciSinif,
                OgrenciKayitTuru = ogrenci.OgrenciKayitTuru,
                OgrenciDerece = ogrenci.OgrenciDerece,
                OgrenciTel = ogrenci.OgrenciTel,
                OgrenciMail = ogrenci.OgrenciMail,
                OgrenciAktif = ogrenci.OgrenciAktif
            };

            if (db.OGRENCI.Count(s => s.OgrenciNo == ogrenci.OgrenciNo) > 0)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Girilen öğrenci numarasına kayıtlı öğrenci bulunmaktadır.";
                return sonuc;
            }
            else if (db.OGRENCI.Count(s => s.OgrenciTC == ogrenci.OgrenciTC) > 0)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Girilen kimlik numarasına kayıtlı öğrenci bulunmaktadır.";
                return sonuc;
            }
            else if (db.OGRENCI.Count(s => s.OgrenciMail == ogrenci.OgrenciMail) > 0)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Girilen e-posta adresi başka bir öğrencide kayıtlıdır.";
                return sonuc;
            }
            else if (db.OGRENCI.Count(s => s.OgrenciTel == ogrenci.OgrenciTel) > 0)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Girilen telefon numarası başka bir öğrencide kayıtlıdır.";
                return sonuc;
            }
            db.OGRENCI.Add(yeniogrenci);
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Öğrenci başarıyla eklendi.";
            return sonuc;
        }

        [HttpPut]
        [Route("api/ogrenciduzenle")]
        public Sonuc OgrenciDuzenle(OGRENCI ogrenci)
        {
            OGRENCI duzenleogrenci = db.OGRENCI.Where(s => s.OgrenciId == ogrenci.OgrenciId).SingleOrDefault();


            OGRENCI baskaogrenciNo = db.OGRENCI.Where(s => s.OgrenciNo == ogrenci.OgrenciNo).SingleOrDefault();

            OGRENCI baskaogrenciTC = db.OGRENCI.Where(s => s.OgrenciTC == ogrenci.OgrenciTC).SingleOrDefault();
            OGRENCI baskaogrenciMail = db.OGRENCI.Where(s => s.OgrenciMail == ogrenci.OgrenciMail).SingleOrDefault();
            OGRENCI baskaogrenciTel = db.OGRENCI.Where(s => s.OgrenciTel == ogrenci.OgrenciTel).SingleOrDefault();
            if (duzenleogrenci == null)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Kayıt bulunamadı";
                return sonuc;
            }

            if (db.OGRENCI.Count(s => s.OgrenciNo == ogrenci.OgrenciNo && s.OgrenciId!=ogrenci.OgrenciId) > 0)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Girilen öğrenci numarasına kayıtlı öğrenci bulunmaktadır.";
                return sonuc;
            }
            else if (db.OGRENCI.Count(s => s.OgrenciTC == ogrenci.OgrenciTC && s.OgrenciId != ogrenci.OgrenciId) > 0)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Girilen kimlik numarasına kayıtlı öğrenci bulunmaktadır.";
                return sonuc;
            }
            else if (db.OGRENCI.Count(s => s.OgrenciMail == ogrenci.OgrenciMail && s.OgrenciId != ogrenci.OgrenciId) > 0)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Girilen e-posta adresi başka bir öğrencide kayıtlıdır.";
                return sonuc;
            }
            else if (db.OGRENCI.Count(s => s.OgrenciTel == ogrenci.OgrenciTel && s.OgrenciId != ogrenci.OgrenciId) > 0)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Girilen telefon numarası başka bir öğrencide kayıtlıdır.";
                return sonuc;
            }

            //else if (duzenleogrenci.OgrenciNo != baskaogrenciNo.OgrenciNo && baskaogrenciNo != null)
            //{
            //    sonuc.Islem = false;
            //    sonuc.Mesaj = "Girilen öğrenci numarasına kayıtlı öğrenci bulunmaktadır.";
            //    return sonuc;
            //}
            //else if (duzenleogrenci.OgrenciTC != baskaogrenciTC.OgrenciTC && baskaogrenciTC !=null)
            //{
            //    sonuc.Islem = false;
            //    sonuc.Mesaj = "Girilen kimlik numarasına kayıtlı öğrenci bulunmaktadır.";
            //    return sonuc;
            //}
            //else if (duzenleogrenci.OgrenciMail != baskaogrenciMail.OgrenciMail && baskaogrenciMail != null)
            //{
            //    sonuc.Islem = false;
            //    sonuc.Mesaj = "Girilen e-posta adresi başka bir öğrencide kayıtlıdır.";
            //    return sonuc;
            //}
            //else if (duzenleogrenci.OgrenciTel != baskaogrenciTel.OgrenciTel && baskaogrenciTel != null)
            //{
            //    sonuc.Islem = false;
            //    sonuc.Mesaj = "Girilen telefon numarası başka bir öğrencide kayıtlıdır.";
            //    return sonuc;
            //}

            duzenleogrenci.OgrenciAd = ogrenci.OgrenciAd;
            duzenleogrenci.OgrenciSoyad = ogrenci.OgrenciSoyad;
            duzenleogrenci.OgrenciNo = ogrenci.OgrenciNo;
            duzenleogrenci.OgrenciTC = ogrenci.OgrenciTC;
            duzenleogrenci.OgrenciFakulteId = ogrenci.OgrenciFakulteId;
            duzenleogrenci.OgrenciBolum = ogrenci.OgrenciBolum;
            duzenleogrenci.OgrenciSinif = ogrenci.OgrenciSinif;
            duzenleogrenci.OgrenciKayitTuru = ogrenci.OgrenciKayitTuru;
            duzenleogrenci.OgrenciDerece = ogrenci.OgrenciDerece;
            duzenleogrenci.OgrenciTel = ogrenci.OgrenciTel;
            duzenleogrenci.OgrenciMail = ogrenci.OgrenciMail;
            duzenleogrenci.OgrenciAktif = ogrenci.OgrenciAktif;
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Öğrenci başarıyla düzenlendi.";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/ogrencisil/{OgrenciId}")]
        public Sonuc OgrenciSil(string OgrenciId)
        {
            OGRENCI silogrenci = db.OGRENCI.Where(s => s.OgrenciId == OgrenciId).SingleOrDefault();
            if (silogrenci == null)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Kayıt bulunamadı";
                return sonuc;
            }
            db.OGRENCI.Remove(silogrenci);
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Öğrenci başarıyla silindi.";
            return sonuc;
        }

        #endregion

        #region Ogrencigiris
        [HttpGet]
        [Route("api/ogrencigiris/{OgrenciTC}/{OgrenciNo}")]

        public OgrenciGiris ogrenciGiris(string OgrenciTC, string OgrenciNo)
        {
            OgrenciGiris og = db.OGRENCI.Where(s=>s.OgrenciTC == OgrenciTC && s.OgrenciNo == OgrenciNo).Select(x => new OgrenciGiris()
            {
                OgrenciId=x.OgrenciId
            }).SingleOrDefault();

          return og;
        }

        #endregion

        #region Sınav

        [HttpGet]
        [Route("api/sinavlistele")]
        public List<SinavVM> SinavListe()
        {
            List<SinavVM> sinavliste = db.SINAV.Select(x => new SinavVM()
            {
                SinavId=x.SinavId,
                SinavAd=x.SinavAd,
                SinavDonem=x.SinavDonem,
                SinavZamani=x.SinavZamani,
                SinavBasvuruBaslama=x.SinavBasvuruBaslama,
                SinavBasvuruBitis=x.SinavBasvuruBitis,
                SinavDili=x.SinavDili,
                SinavTuru=x.SinavTuru,
                SinavIptal=x.SinavIptal,
                SinavTamam=x.SinavTamam,
                SinavAciklama=x.SinavAciklama,
                SinavBasvuruSayisi=x.BASVURU.Count()
            }).OrderBy(t=>t.SinavTamam).ThenByDescending(s=>s.SinavZamani).ToList();

            return sinavliste;
        }

        [HttpGet]
        [Route("api/basvurulacaksinavlistele")]
        public List<SinavVM> BasvurulacakSinavListe()
        {
            DateTime simdi = DateTime.Now;

            List<SinavVM> sinavliste = db.SINAV.ToList().Where(s=>DateTime.Parse(s.SinavBasvuruBitis)>=simdi && DateTime.Parse(s.SinavBasvuruBaslama) <= simdi).Select(x => new SinavVM()
            {
                SinavId = x.SinavId,
                SinavAd = x.SinavAd,
                SinavDonem = x.SinavDonem,
                SinavZamani = x.SinavZamani,
                SinavBasvuruBaslama = x.SinavBasvuruBaslama,
                SinavBasvuruBitis = x.SinavBasvuruBitis,
                SinavDili = x.SinavDili,
                SinavTuru = x.SinavTuru,
                SinavIptal = x.SinavIptal,
                SinavTamam = x.SinavTamam,
                SinavAciklama = x.SinavAciklama,
                SinavBasvuruSayisi = x.BASVURU.Count()
            }).OrderBy(t => t.SinavTamam).ThenByDescending(s => s.SinavZamani).ToList();

            return sinavliste;
        }

        [HttpGet]
        [Route("api/basvurubitensinavlistele")]
        public List<SinavVM> BasvurubitenSinavListe()
        {
            DateTime simdi = DateTime.Now;

            List<SinavVM> sinavliste = db.SINAV.ToList().Where(s => DateTime.Parse(s.SinavBasvuruBitis) <= simdi).Select(x => new SinavVM()
            {
                SinavId = x.SinavId,
                SinavAd = x.SinavAd,
                SinavDonem = x.SinavDonem,
                SinavZamani = x.SinavZamani,
                SinavBasvuruBaslama = x.SinavBasvuruBaslama,
                SinavBasvuruBitis = x.SinavBasvuruBitis,
                SinavDili = x.SinavDili,
                SinavTuru = x.SinavTuru,
                SinavIptal = x.SinavIptal,
                SinavTamam = x.SinavTamam,
                SinavAciklama = x.SinavAciklama,
                SinavBasvuruSayisi = x.BASVURU.Count()
            }).OrderBy(t => t.SinavTamam).ThenByDescending(s => s.SinavZamani).ToList();

            return sinavliste;
        }

        [HttpGet]
        [Route("api/sinavbyid/{SinavId}")]
        public SinavVM SinavById(string SinavId)
        {
            SinavVM sinav = db.SINAV.Where(s => s.SinavId == SinavId).Select(x => new SinavVM()
            {
                SinavId = x.SinavId,
                SinavAd = x.SinavAd,
                SinavDonem = x.SinavDonem,
                SinavZamani = x.SinavZamani,
                SinavBasvuruBaslama = x.SinavBasvuruBaslama,
                SinavBasvuruBitis = x.SinavBasvuruBitis,
                SinavDili = x.SinavDili,
                SinavTuru = x.SinavTuru,
                SinavIptal = x.SinavIptal,
                SinavTamam = x.SinavTamam,
                SinavAciklama = x.SinavAciklama,
                SinavBasvuruSayisi = x.BASVURU.Count()
            }).SingleOrDefault();

            return sinav;
        }


        [HttpPost]
        [Route("api/sinavekle")]
        public Sonuc SinavEkle(SINAV sinav)
        {

            DateTime simdi = DateTime.Now;
            SINAV yenisinav = new SINAV()
            {
                SinavId = Guid.NewGuid().ToString(),
                SinavAd = sinav.SinavAd,
                SinavDonem = sinav.SinavDonem,
                SinavZamani = sinav.SinavZamani,
                SinavBasvuruBaslama = sinav.SinavBasvuruBaslama,
                SinavBasvuruBitis = sinav.SinavBasvuruBitis,
                SinavDili = sinav.SinavDili,
                SinavTuru = sinav.SinavTuru,
                SinavIptal = sinav.SinavIptal,
                SinavTamam = sinav.SinavTamam,
                SinavAciklama = sinav.SinavAciklama
            };

            if (DateTime.Parse(yenisinav.SinavZamani) < simdi)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Sınav için daha ileri bir tarih girilmelidir.";
                return sonuc;
            }

            if (db.SINAV.Count(s => s.SinavAd == sinav.SinavAd && s.SinavZamani == sinav.SinavZamani) > 0)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Girilen sınav kayıtlıdır";
                return sonuc;
            }


            db.SINAV.Add(yenisinav);
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Sınav başarıyla eklendi.";
            return sonuc;
        }

        [HttpPut]
        [Route("api/sinavduzenle")]
        public Sonuc SınavDuzenle(SINAV sinav)
        {
            SINAV duzenlesinav = db.SINAV.Where(s => s.SinavId == sinav.SinavId).SingleOrDefault();
            if (duzenlesinav == null)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Kayıt bulunamadı";
                return sonuc;
            }
            duzenlesinav.SinavAd = sinav.SinavAd;
            duzenlesinav.SinavDonem = sinav.SinavDonem;
            duzenlesinav.SinavZamani = sinav.SinavZamani;
            duzenlesinav.SinavBasvuruBaslama = sinav.SinavBasvuruBaslama;
            duzenlesinav.SinavBasvuruBitis = sinav.SinavBasvuruBitis;
            duzenlesinav.SinavDili = sinav.SinavDili;
            duzenlesinav.SinavTuru = sinav.SinavTuru;
            duzenlesinav.SinavIptal = sinav.SinavIptal;
            duzenlesinav.SinavTamam = sinav.SinavTamam;
            duzenlesinav.SinavAciklama = sinav.SinavAciklama;
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Sınav başarıyla düzenlendi.";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/sinavsil/{SinavId}")]
        public Sonuc SinavSil(string SinavId)
        {
            SINAV silsinav = db.SINAV.Where(s => s.SinavId == SinavId).SingleOrDefault();
            var sinavkayitsayisi = db.BASVURU.Count(x => x.BasvuruSinavId == SinavId);
            if (silsinav == null)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Kayıt bulunamadı";
                return sonuc;
            }
            if (sinavkayitsayisi > 0)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Sınava kayıtlı "+ sinavkayitsayisi+" öğrenci bulunmaktadır. Sınav silinemez.";
                return sonuc;
            }
            db.SINAV.Remove(silsinav);
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Sınav başarıyla silindi.";
            return sonuc;
        }

        #endregion

        #region Basvuru

        [HttpGet]
        [Route("api/basvurulistele")]
        public List<BasvuruVM> BasvuruListe()
        {
            List<BasvuruVM> basvuruliste = db.BASVURU.Select(x => new BasvuruVM()
            {
                BasvuruId=x.BasvuruId,
                BasvuruSinavId=x.BasvuruSinavId,
                BasvuruSinavAd=x.SINAV.SinavAd,
                BasvuruSinavZamani=x.SINAV.SinavZamani,
                BasvuruOgrenciId=x.BasvuruOgrenciId,
                BasvuruOgrenciNo=x.OGRENCI.OgrenciNo,
                BasvuruOgrenciTC=x.OGRENCI.OgrenciTC,
                BasvuruOgrenciAd=x.OGRENCI.OgrenciAd,
                BasvuruOgrenciSoyad=x.OGRENCI.OgrenciSoyad,
                BasvuruOgrenciFakulteId=x.OGRENCI.OgrenciFakulteId,
                BasvuruOgrenciFakulteAd=x.OGRENCI.FAKULTE.FakulteAd,
                BasvuruOgrenciBolum=x.OGRENCI.OgrenciBolum,
                BasvuruOgrenciDerece=x.OGRENCI.OgrenciDerece,
                BasvuruOgrenciSinif=x.OGRENCI.OgrenciSinif,
                BasvuruOgrenciKayitTuru=x.OGRENCI.OgrenciKayitTuru,
                BasvuruOgrenciMail=x.OGRENCI.OgrenciMail,
                BasvuruOgrenciTel=x.OGRENCI.OgrenciTel,
                BasvuruTarihi=x.BasvuruTarihi,
                BasvuruIptalTarihi=x.BasvuruIptalTarihi,
                BasvuruDuzenlemeTarihi=x.BasvuruDuzenlemeTarihi,
                BasvuruDerslikId=x.BasvuruDerslikId,
                BasvuruDerslikAd=x.DERSLIK.DerslikAd,
                BasvuruDerslikKat=x.DERSLIK.DerslikKat,
                BasvuruIptal=x.BasvuruIptal
            }).ToList();

            return basvuruliste;
        }

        [HttpGet]
        [Route("api/basvurubysinavid/{SinavId}")]
        public List<BasvuruVM> BasvuruById(string SinavId)
        {
            List<BasvuruVM> basvuru = db.BASVURU.Where(s => s.BasvuruSinavId == SinavId).Select(x => new BasvuruVM()
            {
                BasvuruId = x.BasvuruId,
                BasvuruSinavId = x.BasvuruSinavId,
                BasvuruSinavAd = x.SINAV.SinavAd,
                BasvuruSinavZamani = x.SINAV.SinavZamani,
                BasvuruOgrenciId = x.BasvuruOgrenciId,
                BasvuruOgrenciNo = x.OGRENCI.OgrenciNo,
                BasvuruOgrenciTC = x.OGRENCI.OgrenciTC,
                BasvuruOgrenciAd = x.OGRENCI.OgrenciAd,
                BasvuruOgrenciSoyad = x.OGRENCI.OgrenciSoyad,
                BasvuruOgrenciFakulteId = x.OGRENCI.OgrenciFakulteId,
                BasvuruOgrenciFakulteAd = x.OGRENCI.FAKULTE.FakulteAd,
                BasvuruOgrenciBolum = x.OGRENCI.OgrenciBolum,
                BasvuruOgrenciDerece = x.OGRENCI.OgrenciDerece,
                BasvuruOgrenciSinif = x.OGRENCI.OgrenciSinif,
                BasvuruOgrenciKayitTuru = x.OGRENCI.OgrenciKayitTuru,
                BasvuruOgrenciMail = x.OGRENCI.OgrenciMail,
                BasvuruOgrenciTel = x.OGRENCI.OgrenciTel,
                BasvuruTarihi = x.BasvuruTarihi,
                BasvuruIptalTarihi = x.BasvuruIptalTarihi,
                BasvuruDuzenlemeTarihi = x.BasvuruDuzenlemeTarihi,
                BasvuruDerslikId = x.BasvuruDerslikId,
                BasvuruDerslikAd = x.DERSLIK.DerslikAd,
                BasvuruDerslikKat = x.DERSLIK.DerslikKat,
                BasvuruIptal = x.BasvuruIptal
            }).ToList();

            return basvuru;
        }

        [HttpGet]
        [Route("api/basvurubyogrenciid/{OgrenciId}")]
        public List<BasvuruVM> BasvuruByOgrenciId(string OgrenciId)
        {
            List<BasvuruVM> basvuru = db.BASVURU.Where(s => s.BasvuruOgrenciId == OgrenciId).Select(x => new BasvuruVM()
            {
                BasvuruId = x.BasvuruId,
                BasvuruSinavId = x.BasvuruSinavId,
                BasvuruSinavAd = x.SINAV.SinavAd,
                BasvuruSinavZamani = x.SINAV.SinavZamani,
                BasvuruOgrenciId = x.BasvuruOgrenciId,
                BasvuruOgrenciNo = x.OGRENCI.OgrenciNo,
                BasvuruOgrenciTC = x.OGRENCI.OgrenciTC,
                BasvuruOgrenciAd = x.OGRENCI.OgrenciAd,
                BasvuruOgrenciSoyad = x.OGRENCI.OgrenciSoyad,
                BasvuruOgrenciFakulteId = x.OGRENCI.OgrenciFakulteId,
                BasvuruOgrenciFakulteAd = x.OGRENCI.FAKULTE.FakulteAd,
                BasvuruOgrenciBolum = x.OGRENCI.OgrenciBolum,
                BasvuruOgrenciDerece = x.OGRENCI.OgrenciDerece,
                BasvuruOgrenciSinif = x.OGRENCI.OgrenciSinif,
                BasvuruOgrenciKayitTuru = x.OGRENCI.OgrenciKayitTuru,
                BasvuruOgrenciMail = x.OGRENCI.OgrenciMail,
                BasvuruOgrenciTel = x.OGRENCI.OgrenciTel,
                BasvuruTarihi = x.BasvuruTarihi,
                BasvuruIptalTarihi = x.BasvuruIptalTarihi,
                BasvuruDuzenlemeTarihi = x.BasvuruDuzenlemeTarihi,
                BasvuruDerslikId = x.BasvuruDerslikId,
                BasvuruDerslikAd = x.DERSLIK.DerslikAd,
                BasvuruDerslikKat = x.DERSLIK.DerslikKat,
                BasvuruIptal = x.BasvuruIptal
            }).ToList();

            return basvuru;
        }

        [HttpPost]
        [Route("api/basvuruekle")]
        public Sonuc BasvuruEkle(BASVURU basvuru)
        {
            DateTime simdi = DateTime.Now;

            SINAV sinavkontrol = db.SINAV.Where(s => s.SinavId == basvuru.BasvuruSinavId).FirstOrDefault();

            BASVURU yenibasvuru = new BASVURU()
            {
                BasvuruId = Guid.NewGuid().ToString(),
                BasvuruSinavId = basvuru.BasvuruSinavId,
                BasvuruOgrenciId = basvuru.BasvuruOgrenciId,
                BasvuruTarihi = basvuru.BasvuruTarihi,
                BasvuruIptalTarihi = basvuru.BasvuruIptalTarihi,
                BasvuruDuzenlemeTarihi = basvuru.BasvuruDuzenlemeTarihi,
                BasvuruDerslikId = basvuru.BasvuruDerslikId,
                BasvuruIptal = basvuru.BasvuruIptal
            };
            OGRENCI ogrenciaktifkontrol = db.OGRENCI.Where(x => x.OgrenciId == basvuru.BasvuruOgrenciId).SingleOrDefault();

            BASVURU ogrencibasvurukontrol = db.BASVURU.Where(x => x.BasvuruOgrenciId == basvuru.BasvuruOgrenciId && x.BasvuruSinavId == basvuru.BasvuruSinavId).SingleOrDefault();

            if (ogrenciaktifkontrol.OgrenciAktif==0)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Aktiflik durumu pasif olan öğrenciler sınava başvuru yapamaz.";
                return sonuc;
            }

            if (ogrencibasvurukontrol!=null)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Sınav başvurusu zaten yapılmış.";
                return sonuc;
            }
            if (DateTime.Parse(sinavkontrol.SinavZamani) < simdi)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Sınav süresi geçen sınava basvuru yapılamaz.";
                return sonuc;
            }
            if (DateTime.Parse(sinavkontrol.SinavBasvuruBitis) < simdi)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Başvuru süresi biten sınava basvuru yapılamaz.";
                return sonuc;
            }
            if (DateTime.Parse(sinavkontrol.SinavBasvuruBaslama) > simdi)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Başvuru süresi başladıktan sonra sınava basvuru yapabilirsiniz.";
                return sonuc;
            }



            db.BASVURU.Add(yenibasvuru);
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Yeni başvuru başarıyla eklendi.";
            return sonuc;
        }

        [HttpPut]
        [Route("api/basvuruduzenle")]
        public Sonuc BasvuruDuzenle(BASVURU basvuru)
        {
            BASVURU duzenlebasvuru = db.BASVURU.Where(s => s.BasvuruId == basvuru.BasvuruId).SingleOrDefault();
            if (duzenlebasvuru == null)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Kayıt bulunamadı";
                return sonuc;
            }

            duzenlebasvuru.BasvuruOgrenciId = basvuru.BasvuruOgrenciId;
            duzenlebasvuru.BasvuruDerslikId = basvuru.BasvuruDerslikId;
            duzenlebasvuru.BasvuruDuzenlemeTarihi = basvuru.BasvuruDuzenlemeTarihi;
            duzenlebasvuru.BasvuruIptal = basvuru.BasvuruIptal;
            duzenlebasvuru.BasvuruIptalTarihi = basvuru.BasvuruIptalTarihi;
            duzenlebasvuru.BasvuruSinavId = basvuru.BasvuruSinavId;
          
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Sınav başarıyla düzenlendi.";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/basvurusil/{BasvuruId}")]
        public Sonuc BasvuruSil(string BasvuruId)
        {
            DateTime simdi = DateTime.Now;
            BASVURU silbasvuru = db.BASVURU.Where(s => s.BasvuruId == BasvuruId).SingleOrDefault();
           
            if (silbasvuru == null)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Kayıt bulunamadı";
                return sonuc;
            }
            if (DateTime.Parse(silbasvuru.SINAV.SinavBasvuruBitis)<simdi)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Başvuru süresi biten başvurular iptal edilemez";
                return sonuc;
            }
            if (DateTime.Parse(silbasvuru.SINAV.SinavZamani) < simdi)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Sınav süresi geçen başvurular iptal edilemez";
                return sonuc;
            }


            db.BASVURU.Remove(silbasvuru);
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Başvuru başarıyla silindi.";
            return sonuc;
        }
        #endregion

        #region ÖğretimElemaniDerslik

        [HttpGet]
        [Route("api/ogretimelemaniderslikliste")]

        public List<OgretimElemaniDerslikVM> OEDerslikListe()
        {
            List<OgretimElemaniDerslikVM> oederslikliste = db.OGRETIMELEMANIDERSLIK.Select(x => new OgretimElemaniDerslikVM()
            {
                OEDId = x.OEDId,
                OEDerslikId =x.OEDerslikId, 
                OEDerslikAd=x.DERSLIK.DerslikAd,
                OEDOEId=x.OEDOEId,
                OEDOEAd=x.OGRETIMELEMANI.OEAd,
                OEDOESoyad=x.OGRETIMELEMANI.OESoyad,
                OEDOEUnvan=x.OGRETIMELEMANI.OEUnvan,
                OESinavId=x.OESinavId,
                OESinavAd=x.SINAV.SinavAd,
                OESinavTarih=x.SINAV.SinavZamani
            }).ToList();

            return oederslikliste;
        }

        [HttpGet]
        [Route("api/ogretimelemaniderslikbyid/{OEDId}")]
        public OgretimElemaniDerslikVM OEDerslikListeById(string OEDId)
        {
            OgretimElemaniDerslikVM oederslik = db.OGRETIMELEMANIDERSLIK.Where(s => s.OEDId == OEDId).Select(x=> new OgretimElemaniDerslikVM()
            {
                OEDId=x.OEDId,
                OEDOEId=x.OEDOEId,
                OEDOEAd=x.OGRETIMELEMANI.OEAd,
                OEDOESoyad=x.OGRETIMELEMANI.OESoyad,
                OEDOEUnvan=x.OGRETIMELEMANI.OEUnvan,
                OEDerslikId=x.OEDerslikId,
                OEDerslikAd=x.DERSLIK.DerslikAd,
                OESinavId=x.OESinavId,
                OESinavAd=x.SINAV.SinavAd,
                OESinavTarih=x.SINAV.SinavZamani
            }).SingleOrDefault();
            
            return oederslik;
        }

        [HttpPost]
        [Route("api/ogretimelemaniderslikekle")]
        public Sonuc OEDerslikEkle(OGRETIMELEMANIDERSLIK oederslik)
        {
            OGRETIMELEMANIDERSLIK yenioederslik = new OGRETIMELEMANIDERSLIK()
            {
                OEDId=Guid.NewGuid().ToString(),
                OEDOEId=oederslik.OEDOEId,
                OESinavId=oederslik.OESinavId,
                OEDerslikId=oederslik.OEDerslikId
            };

            var oedkontrol = db.OGRETIMELEMANIDERSLIK.Count(s => s.OESinavId == oederslik.OESinavId && s.OEDOEId == oederslik.OEDOEId);

            if (oedkontrol > 0)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Girilen sınav kayıtlıdır";
                return sonuc;
            }

            db.OGRETIMELEMANIDERSLIK.Add(yenioederslik);
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Öğretime elemanına derslik başarıyla eklendi.";
            return sonuc;
        }

        [HttpPut]
        [Route("api/oederslikduzenle")]
        public Sonuc OEDerslikduzenle(OGRETIMELEMANIDERSLIK oederslik)
        {
            OGRETIMELEMANIDERSLIK duzenleoederslik = db.OGRETIMELEMANIDERSLIK.Where(s => s.OEDId == oederslik.OEDId).SingleOrDefault();
            if (duzenleoederslik == null)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Kayıt bulunamadı";
                return sonuc;
            }
            duzenleoederslik.OEDId = oederslik.OEDId;
            duzenleoederslik.OEDOEId = oederslik.OEDOEId;
            duzenleoederslik.OESinavId = oederslik.OESinavId;
            duzenleoederslik.OEDerslikId = oederslik.OEDerslikId;
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Sınav başarıyla düzenlendi.";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/oedersliksil/{OEDId}")]
        public Sonuc OEDerslikSil(string OEDId)
        {
            OGRETIMELEMANIDERSLIK siloederslik = db.OGRETIMELEMANIDERSLIK.Where(s => s.OEDId == OEDId).SingleOrDefault();

            if (siloederslik == null)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Kayıt bulunamadı";
                return sonuc;
            }

            db.OGRETIMELEMANIDERSLIK.Remove(siloederslik);
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Öğretim elemanı derslik bilgisi başarıyla silindi.";
            return sonuc;
        }

        #endregion

        #region KullaniciGirisi

        [HttpGet]
        [Route("api/kullanicigirisiliste")]
        public List<KullaniciGirisVM> KullaniciGirisListe()
        {
            List<KullaniciGirisVM> kullaniciliste = db.KULLANCIGIRIS.Select(x => new KullaniciGirisVM()
            {
                KullaniciId = x.KullaniciId,
                KGOEId = x.KGOEId,
                KGOEAd=x.OGRETIMELEMANI.OEAd,
                KGOESoyad=x.OGRETIMELEMANI.OESoyad,
                KGOEDKullaniciAdi=x.KGOEDKullaniciAdi,
                KGOEDSifre=x.KGOEDSifre,
                KGYetki=x.KGYetki
            }).ToList();

            return kullaniciliste;
        }

        [HttpGet]
        [Route("api/kullanicigirisibyid/{KullaniciId}")]
        public KullaniciGirisVM KullaniciGirisById(string KullaniciId)
        {
            KullaniciGirisVM kullanici = db.KULLANCIGIRIS.Where(s=>s.KullaniciId==KullaniciId).Select(x => new KullaniciGirisVM()
            {
                KullaniciId = x.KullaniciId,
                KGOEId = x.KGOEId,
                KGOEAd = x.OGRETIMELEMANI.OEAd,
                KGOESoyad = x.OGRETIMELEMANI.OESoyad,
                KGOEDKullaniciAdi = x.KGOEDKullaniciAdi,
                KGOEDSifre = x.KGOEDSifre,
                KGYetki = x.KGYetki
            }).SingleOrDefault();

            return kullanici;
        }

        [HttpPost]
        [Route("api/kullaniciekle")]
        public Sonuc kullaniciekle(KULLANCIGIRIS kullanici)
        {
            KULLANCIGIRIS yenikullanici = new KULLANCIGIRIS()
            {
                KullaniciId = Guid.NewGuid().ToString(),
                KGOEId = kullanici.KGOEId,
                KGOEDKullaniciAdi = kullanici.KGOEDKullaniciAdi,
                KGOEDSifre = kullanici.KGOEDSifre,
                KGYetki = kullanici.KGYetki
            };

            if (db.KULLANCIGIRIS.Count(s=>s.KGOEId==kullanici.KGOEId)>0)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Kullanici zaten kayıtlıdır.";
                return sonuc;
            }

            sonuc.Islem = true;
            sonuc.Mesaj = "Kullanici başarıyla eklendi.";
            return sonuc;
        }
        [HttpPut]
        [Route("api/kullaniciduzenle")]
        public Sonuc KullaniciDuzenle(KULLANCIGIRIS kullanici)
        {
            KULLANCIGIRIS duzenlekullanici = db.KULLANCIGIRIS.Where(s => s.KullaniciId == kullanici.KullaniciId).SingleOrDefault();

            if (duzenlekullanici==null)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Kayıt bulunamadı.";
                return sonuc;
            }

            duzenlekullanici.KullaniciId = kullanici.KullaniciId;
            duzenlekullanici.KGOEId = kullanici.KGOEId;
            duzenlekullanici.KGOEDKullaniciAdi = kullanici.KGOEDKullaniciAdi;
            duzenlekullanici.KGOEDSifre = kullanici.KGOEDSifre;
            duzenlekullanici.KGYetki = kullanici.KGYetki;

            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Kullanici başarıyla düzenlendi.";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/kullanicisil/{KullaniciId}")]
        public Sonuc KullaniciSil(string KullaniciId)
        {
            KULLANCIGIRIS silkullanici = db.KULLANCIGIRIS.Where(s => s.KullaniciId == KullaniciId).SingleOrDefault();
            if (silkullanici==null)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Kayıt bulunamadı.";
                return sonuc;
            }
            db.KULLANCIGIRIS.Remove(silkullanici);
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Kullanici başarıyla silindi.";
            return sonuc;
        }
        #endregion

        #region İstatistikler

        [HttpGet]
        [Route("api/istatistikler")]

        public  StatsVM Stats()
        {
            StatsVM istatistik = new StatsVM();


            istatistik.FakulteSayisi = db.FAKULTE.Count();
            istatistik.DerslikSayisi = db.DERSLIK.Count();
            istatistik.OgrenciSayisi = db.OGRENCI.Count();
            istatistik.OgretimElemaniSayisi = db.OGRETIMELEMANI.Count();
            
            return istatistik;
        }
        #endregion
    }
}
