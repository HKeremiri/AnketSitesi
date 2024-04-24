using AnketSitesi.Models;

namespace AnketSitesi.ViewModels
{
    public class SurveyAnswersViewModel
    {
        public Anket Anket { get; set; }
        
        public List<Cevaplar> CevapList { get; set; }

        public CevaplamaDurumu CevapDurumuList { get; set; }

        public List<Sorular> SoruList { get; set; }

    }
}
