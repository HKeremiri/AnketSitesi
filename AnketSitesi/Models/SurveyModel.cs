using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnketSitesi.Models
{
    public class Anket
    {
        [Key]
        public int AnketId { get; set; }

        public string AnketAdi { get; set; }
        public string AnketDetay { get; set; }
        public bool AnketVisibility { get; set; }

        [Display(Name = "Oluşturan Kullanıcı")]
        public string CreatedBy { get; set; }

        [Display(Name = "Oluşturulma Tarihi")]
        public DateTime SurveyCreateDate { get; set; } = DateTime.Now;

      public ICollection<Sorular>? Sorulars { get; set; }

        public  ICollection<CevaplamaDurumu>? CevaplamaDurumus { get; set; }

    }



    public class Sorular
    {
        [Key]
        public int SoruId { get; set; }

        public Anket Anket { get; set; }

        public int AnketId { get; set; }

       
        public string Soru { get; set; }

        public string SoruTipi { get; set; }

        public ICollection<Secenekler>? seceneklers { get; set; }

        public ICollection<Cevaplar>? Cevaplars { get; set; }

        


    }

    public class Secenekler
    {
        [Key]
        public int SecenekId { get; set; }

        public Sorular Sorular { get; set; }

        public int SorularId { get; set; }

        public string Secenek { get; set; }
     


   
    }



    public class Cevaplar

    {
        [Key]
       public int CevapId {  get; set; }
       
       public string UserName { get; set; }
        public Sorular Sorular { get; set; }

        public int SorularId { get; set; }

        public string Cevap { get; set; }
      



    }
    public class CevaplamaDurumu
    {
        [Key]
        public int CevapDurumuId { get; set; }  
        public Anket Anket { get; set;}


        public int AnketId { get; set; }

        public string UserName { get; set;}

		public DateTime OnayTarihi { get; set; } = DateTime.Now;

		public bool Onay { get; set; } = false;
        }



    }




