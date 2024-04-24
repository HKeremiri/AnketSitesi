using AnketSitesi.Models;

namespace AnketSitesi.ViewModels
{
    public class SurveyReply
    {
        public Anket AnketListele {  get; set; }

        public List<Sorular> SoruListele { get; set; }

        public List<Secenekler> SecenekListele { get; set; }

        public  List<Cevaplar> Cevaplar { get; set; }

        public CevaplamaDurumu Cevaplamadurumu { get; set; }

        

    }
}
