using AnketSitesi.Models;

namespace AnketSitesi.ViewModels
{
    public class AdminPanelIndexViewModel
    {

        public List<Anket> AnketListesi { get; set; }

        public List<AppUser> UserList {  get; set; }

        public List<CevaplamaDurumu> CevapListesi { get; set; }

    }
}
