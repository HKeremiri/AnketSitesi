using System.ComponentModel.DataAnnotations;

namespace AnketSitesi.ViewModels
{
    public class SurveyCreateViewModel
    {


      
      

        [Required]

        public string AnketAdi { get; set; }

        [Required]
        public string AnketDetay { get; set; }

        [Required]
        public bool AnketVisibility { get; set; }

        [Display(Name = "Oluşturan Kullanıcı")]
        public string CreatedBy { get; set; }

        [Display(Name = "Oluşturulma Tarihi")]
        public DateTime SurveyCreateDate { get; set; } = DateTime.Now;

      

    }
}
