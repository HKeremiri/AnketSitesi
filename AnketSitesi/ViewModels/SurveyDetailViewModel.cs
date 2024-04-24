using AnketSitesi.Models;

namespace AnketSitesi.ViewModels
{
    public class SurveyDetailViewModel
    {
        public List<CevaplamaDurumu> CevaplamaDurumuList { get; set; }

        public Anket Anket { get; set; }
        
    }
}
