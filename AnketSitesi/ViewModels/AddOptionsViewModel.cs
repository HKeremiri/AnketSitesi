using AnketSitesi.Models;

namespace AnketSitesi.ViewModels
{
    public class AddOptionsViewModel
    {
        public Sorular Sorular { get; set; }

        public Secenekler Secenekler { get; set; }

        public List<Secenekler> SeceneklerList { get; set; }



    }
}
