using AnketSitesi.Models;

namespace AnketSitesi.ViewModels
{
    public class AddQuestions
    {
        public Anket Anket { get; set; }
        public Sorular Sorular { get; set; }

        public List<Sorular>? SoruList { get; set; }

    }

   

}
