using AnketSitesi.Models;
using AnketSitesi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection.Metadata;
using System.Security;


namespace AnketSitesi.Controllers
{
    [Authorize]
    public class SurveyController : Controller
    {


        private readonly Context _context;



        public SurveyController(Context context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult SurveyCreate()
        {

            return View();
        }


        [HttpPost]
        public IActionResult SurveyCreate(SurveyCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                // ViewModel'den Anket nesnesine verileri aktar
                var newAnket = new Anket
                {
                    CreatedBy = model.CreatedBy,
                    AnketAdi = model.AnketAdi,
                    AnketDetay = model.AnketDetay,
                    AnketVisibility = model.AnketVisibility,
                    SurveyCreateDate = model.SurveyCreateDate,





                };
                _context.Add(newAnket);

                _context.SaveChanges();
                // Anket başarıyla oluşturuldu, yönlendime
                int AnketId = newAnket.AnketId;
                var url = Url.Action("SurveyJoin", "Survey", new { AnketId });

                return RedirectToAction("AddQuestions", "Survey", new { AnketId });
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);

            }

            // ModelState geçerli değilse, hata mesajları ile birlikte aynı sayfaya geri dön.
            return View(model);
        }

        [HttpPost]
        public IActionResult GlobalDeleteSurvey(int AnketId)
        {
            var anket = _context.Ankets.Find(AnketId);
            if (anket == null)
            {
                return NotFound();
            }

            _context.Ankets.Remove(anket);
            _context.SaveChanges();



            return RedirectToAction("GlobalSurveys");
        }

        [HttpPost]
        public IActionResult MySurveyDeleteSurvey(int AnketId)
        {
            var anket = _context.Ankets.Find(AnketId);
            if (anket == null)
            {
                return NotFound();
            }

            _context.Ankets.Remove(anket);
            _context.SaveChanges();



            return RedirectToAction("MySurvey");
        }



        [HttpGet]
        public IActionResult SurveyEdit(int Id)
        {
            // Anketi bul
            var existingAnket = _context.Ankets.FirstOrDefault(a => a.AnketId == Id);

            ViewBag.anket = existingAnket;

            if (existingAnket == null)
            {
                // Anket bulunamazsa uygun bir işlem gerçekleştirin (örneğin, hata mesajı gösterip bir sayfaya yönlendirme)
                return RedirectToAction("Index"); // veya başka bir sayfaya yönlendirme
            }

            // Anketi düzenleme view'ine yönlendir
            return View(existingAnket);
        }

        [HttpPost]
        public IActionResult SurveyEdit(Anket model)
        {


            var existingAnket = _context.Ankets.FirstOrDefault(a => a.AnketId == model.AnketId);

            if (existingAnket != null)
            {

                existingAnket.CreatedBy = model.CreatedBy;
                existingAnket.AnketAdi = model.AnketAdi;
                existingAnket.AnketDetay = model.AnketDetay;
                existingAnket.AnketVisibility = model.AnketVisibility;






                _context.SaveChanges();


                return View(existingAnket);
            }


            return NotFound();
        }


        public async Task<IActionResult> GlobalSurveys()
        {

            var anketler = await _context.Ankets.Where(a => a.AnketVisibility == true).ToListAsync();





            return View(anketler);


        }

        public async Task<IActionResult> MySurvey()

        {
            var anketler = await _context.Ankets.Where(a => a.CreatedBy == User.Identity.Name).ToListAsync();





            return View(anketler);
        }

        public IActionResult SurveyCreated(int AnketId, string url)
        {

            ViewBag.AnketId = AnketId;

            string link = $"http://localhost:5261{url}";
            ViewBag.Link = link;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SurveyEdite(int Id)
        {
            var anket1 = await _context.Ankets.FindAsync(Id);
            return View(anket1);

        }


        public IActionResult Search()
        {
            return View();
        }

        [HttpPost]

        public IActionResult Search(Anket model)
        {
            var anket = _context.Ankets.FirstOrDefault(a => a.AnketId == model.AnketId);

            string UserName = User.Identity.Name;
            try
            {
                anket = _context.Ankets.FindAsync(model.AnketId).Result;
            }
            catch (Exception)
            {
                return RedirectToAction("Index2", "Home");
            }


            if (anket != null)
            {
                return RedirectToAction("SurveyJoin", new { Id = anket.AnketId });
            }
            else
            {
                return RedirectToAction("Index2", "Home");
            }
        }

        [HttpGet]
        public IActionResult SurveyDetails(int Id)
        {
            // Anketi bul
            var existingAnket = _context.Ankets.FirstOrDefault(a => a.AnketId == Id);

            var users = _context.CevaplamaDurumus.Where(a => a.AnketId == Id).Where(a => a.Onay == true).ToList();


            ViewBag.anket = existingAnket;

            if (existingAnket == null)
            {

                return RedirectToAction("Index");
            }
            var model = new SurveyDetailViewModel
            {
                Anket = existingAnket,
                CevaplamaDurumuList = users,
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult SurveyJoin(SurveyReply model, int Id)
        {



            var existingAnket = _context.Ankets.FirstOrDefault(a => a.AnketId == Id);

            var cevaplandimi = _context.CevaplamaDurumus
         .Any(a => a.AnketId == existingAnket.AnketId && a.Onay == true && a.UserName == User.Identity.Name);




            if (cevaplandimi == true)
            {

                ViewBag.Mesaj = "Bu anketi zaten daha önce cevapladınız";

            }

            if (existingAnket == null)
            {
                return RedirectToAction("Index");
            }

            // Soruları bul
            var sorular = _context.Sorulars
                .Where(s => s.AnketId == existingAnket.AnketId)
                .ToList();

            // Sorulara ait tüm seçenekleri bul
            var secenekler = new List<Secenekler>();
            foreach (var soru in sorular)
            {
                var soruSecenekler = _context.Seceneklers
                    .Where(sec => sec.SorularId == soru.SoruId)
                    .ToList();

                secenekler.AddRange(soruSecenekler);
            }



            var allsurvey = new SurveyReply
            {
                AnketListele = existingAnket,
                SoruListele = sorular,
                SecenekListele = secenekler,
                Cevaplar = model.Cevaplar
            };


            return View(allsurvey);
        }
        [HttpPost]
        public IActionResult SurveyJoin(SurveyReply model)
        {
            foreach (var cevap in model.Cevaplar)
            {

                if (cevap.Cevap != "false")
                {


                    var kaydetme = new Cevaplar
                    {
                        Cevap = cevap.Cevap,
                        UserName = cevap.UserName,
                        SorularId = cevap.SorularId,


                    };
                    _context.Cevaplars.Add(kaydetme);

                    _context.SaveChanges();

                }

            }

            var onaylama = new CevaplamaDurumu
            {
                AnketId = model.Cevaplamadurumu.AnketId,
                Onay = model.Cevaplamadurumu.Onay,
                UserName = model.Cevaplamadurumu.UserName,
                OnayTarihi = model.Cevaplamadurumu.OnayTarihi

            };

            _context.CevaplamaDurumus.Add(onaylama);

            _context.SaveChanges();



            return RedirectToAction("SurveyJoinSucceed");
        }

        public IActionResult SurveyJoinSucceed()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AddQuestions(int anketId)
        {

            var anket = _context.Ankets.Find(anketId);
            var anketlist = _context.Ankets.Find(anketId);

            if (anket == null)
            {
                return NotFound();
            }


            var sorularList = _context.Sorulars.Where(s => s.AnketId == anketId).ToList();

            var model = new AddQuestions
            {
                Anket = anket,
                SoruList = sorularList,



            };

            return View(model);

        }


        [HttpPost]

        public async Task<IActionResult> AddQuestions(AddQuestions model)
        {


            var anket = model.Anket;

            var sorular = new Sorular
            {
                Soru = model.Sorular.Soru,
                SoruTipi = model.Sorular.SoruTipi,
                AnketId = anket.AnketId
            };

            var modell = new AddQuestions
            {
                Anket = model.Anket,
                SoruList = model.SoruList,
                Sorular = sorular

            };

            _context.Sorulars.Add(sorular);
            await _context.SaveChangesAsync();

            return RedirectToAction("AddQuestions", new { anketId = anket.AnketId });




        }





        [HttpPost]
        public IActionResult DeleteQuestion(int SoruId, int AnketId)
        {
            var soru = _context.Sorulars.Find(SoruId);
            if (soru == null)
            {
                return NotFound();
            }

            _context.Sorulars.Remove(soru);
            _context.SaveChanges();



            return RedirectToAction("AddQuestions", new { AnketId });
        }

        [HttpGet]

        public async Task<IActionResult> AddOptions(int SoruId)
        {

            var soru = _context.Sorulars.Find(SoruId);
            var sorulist = _context.Sorulars.Find(SoruId);

            if (soru == null)
            {
                return NotFound();
            }


            var seceneklerlist = _context.Seceneklers.Where(s => s.SorularId == SoruId).ToList();


            var model = new AddOptionsViewModel
            {
                Sorular = soru,
                SeceneklerList = seceneklerlist



            };

            return View(model);

        }


        [HttpPost]

        public async Task<IActionResult> AddOptions(AddOptionsViewModel model)
        {


            var sorular = model.Sorular;

            var secenekler = new Secenekler
            {
                Secenek = model.Secenekler.Secenek,
                SorularId = sorular.SoruId

            };

            var modell = new AddOptionsViewModel
            {
                Sorular = model.Sorular,
                Secenekler = secenekler,
                SeceneklerList = model.SeceneklerList

            };



            _context.Seceneklers.Add(secenekler);
            await _context.SaveChangesAsync();

            return RedirectToAction("AddOptions", new { soruId = sorular.SoruId });




        }

        [HttpPost]
        public IActionResult DeleteOption(int SecenekId, int SoruId)
        {
            var secenek = _context.Seceneklers.Find(SecenekId);
            if (secenek == null)
            {
                return NotFound();
            }

            _context.Seceneklers.Remove(secenek);
            _context.SaveChanges();



            return RedirectToAction("AddOptions", new { SoruId });
        }

        [HttpGet]
        public IActionResult SurveyAnswers(int AnketId, string UserName)
        {
            var anket = _context.Ankets.Find(AnketId);

            if (anket == null)
            {
                return RedirectToAction("SurveyCreate");
            }
            var sorular = _context.Sorulars.Where(s => s.AnketId == AnketId).ToList();

            var cevaplar = _context.Cevaplars.Where(c => c.UserName == UserName).ToList();

            var cevapdurumu = _context.CevaplamaDurumus
         .FirstOrDefault(c => c.AnketId == AnketId && c.UserName == UserName);

            if (cevapdurumu == null)
            {

                return RedirectToAction("SurveyCreate");
            }

            var model = new SurveyAnswersViewModel
            {
                Anket = anket,
                SoruList = sorular,
                CevapList = cevaplar,
                CevapDurumuList = cevapdurumu

            };

            return View(model);
        }




    }

}